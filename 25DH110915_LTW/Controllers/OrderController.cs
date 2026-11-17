using _25DH110915_LTW.Models;
using _25DH110915_LTW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. GET: Order/Checkout (Hiển thị trang thanh toán)
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index", "Home");

            var customer = Session["Customer"] as Customer;
            if (customer == null) return RedirectToAction("Login", "Account");

            // Lấy thông tin mới nhất của khách hàng từ DB để truyền vào phần Thông tin thanh toán
            var customerInDb = db.Customers.Find(customer.CustomerID) ?? customer;

            var model = new CheckoutVM()
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalCartValue(),
                CustomerID = customerInDb.CustomerID,
                CustomerName = customerInDb.CustomerName,
                CustomerPhone = customerInDb.CustomerPhone,
                CustomerEmail = customerInDb.CustomerEmail,
                ShippingAddress = customerInDb.CustomerAddress,
                Username = customerInDb.Username,
                ShippingMethod = "Giao hàng nhanh",
                PaymentMethod = "Tiền mặt"
            };
            return View(model);
        }

        // 2. POST: Order/Checkout (Xử lý lưu đơn hàng)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model, FormCollection form)
        {
            var cart = Session["Cart"] as Cart;
            var customer = Session["Customer"] as Customer;
            if (customer == null) return RedirectToAction("Login", "Account");
            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index", "Home");

            // Xử lý lấy địa chỉ
            string finalAddress = model.ShippingAddress;
            string newAddress = form["newShippingAddress"];
            if (!string.IsNullOrEmpty(newAddress))
            {
                finalAddress = newAddress;
            }
            if (string.IsNullOrWhiteSpace(finalAddress))
            {
                finalAddress = customer.CustomerAddress ?? "TP.HCM";
            }

            // Thiết lập PaymentStatus dựa trên PaymentMethod
            string paymentStatus = "Chờ thanh toán";
            switch (model.PaymentMethod)
            {
                case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                case "Paypal": paymentStatus = "Thanh toán paypal"; break;
                case "Mua trước trả sau": paymentStatus = "Chờ thanh toán"; break;
                default: paymentStatus = "Chờ thanh toán"; break;
            }

            // A. TẠO ĐƠN HÀNG (Order)
            Order order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalCartValue(),
                ShippingAddress = finalAddress,
                PaymentMethod = model.PaymentMethod ?? "Tiền mặt",
                DeliveryMethod = model.ShippingMethod ?? "Giao hàng nhanh",
                PaymentStatus = paymentStatus
            };
            db.Orders.Add(order);
            db.SaveChanges();

            // B. TẠO CHI TIẾT ĐƠN HÀNG & TRỪ TỒN KHO (Tính lại số lượng tồn và số lượng đã bán)
            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Quantity * item.Price
                };
                db.OrderDetails.Add(detail);

                // Trừ tồn kho
                var product = db.Products.Find(item.ProductID);
                if (product != null)
                {
                    int currentStock = product.Quantity ?? 0;
                    product.Quantity = Math.Max(0, currentStock - item.Quantity);
                }
            }
            db.SaveChanges();

            // C. HOÀN TẤT VÀ CHUYỂN HƯỚNG
            if (model.PaymentMethod == "Paypal")
            {
                return RedirectToAction("PaymentWithPaypal", "Paypal");
            }
            else
            {
                Session.Remove("Cart");
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
        }

        // 3. GET: Order/OrderSuccess (Trang thông báo thành công)
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders
                          .Include(o => o.Customer)
                          .Include(o => o.OrderDetails.Select(od => od.Product))
                          .SingleOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            return View(order);
        }

        // 4. GET: Order/MyOrder (Trang lịch sử đơn hàng của khách - Đơn hàng của tôi)
        public ActionResult MyOrder(string searchTerm)
        {
            var customer = Session["Customer"] as Customer;
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Chỉ lấy đơn hàng của đúng khách hàng đang đăng nhập
            var customerOrdersQuery = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails.Select(od => od.Product.Category))
                .Where(o => o.CustomerID == customer.CustomerID);

            bool hasAnyOrders = customerOrdersQuery.Any();
            ViewBag.HasAnyOrders = hasAnyOrders;
            ViewBag.SearchTerm = searchTerm;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string keyword = searchTerm.Trim();
                customerOrdersQuery = customerOrdersQuery.Where(o =>
                    o.OrderID.ToString().Contains(keyword) ||
                    o.OrderDetails.Any(od => od.Product.ProductName.Contains(keyword) ||
                                             od.Product.Category.CategoryName.Contains(keyword))
                );
            }

            var orders = customerOrdersQuery
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.OrderID)
                .ToList();

            return View(orders);
        }

        // 5. MUA LẠI: Thêm sản phẩm vào Giỏ hàng với số lượng = 1
        public ActionResult BuyAgain(int id, int? productId = null)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

            if (productId.HasValue)
            {
                var prod = db.Products.Find(productId.Value);
                if (prod != null)
                {
                    cart.AddProduct(prod, 1);
                }
            }
            else
            {
                var order = db.Orders.Include(o => o.OrderDetails).SingleOrDefault(o => o.OrderID == id);
                if (order != null && order.OrderDetails != null)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        var prod = db.Products.Find(detail.ProductID);
                        if (prod != null)
                        {
                            cart.AddProduct(prod, 1);
                        }
                    }
                }
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index", "Cart");
        }

        // 6. GET: Order/OrderDetails (Xem chi tiết Hóa đơn mua hàng)
        public ActionResult OrderDetails(int id)
        {
            var order = db.Orders
                          .Include(o => o.Customer)
                          .Include(o => o.OrderDetails.Select(od => od.Product))
                          .SingleOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            var customer = Session["Customer"] as Customer;
            if (customer == null || order.CustomerID != customer.CustomerID)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(order);
        }
    }
}
