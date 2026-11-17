using _25DH110915_LTW.Models;
using _25DH110915_LTW.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity; 

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

            var model = new CheckoutVM()
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalCartValue(),
                ShippingAddress = customer.CustomerAddress, // Lấy địa chỉ mặc định
                CustomerID = customer.CustomerID,
                Username = customer.Username
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

            // Xử lý lấy địa chỉ (Nếu JS đẩy newShippingAddress lên thì dùng, không thì dùng mặc định)
            string finalAddress = model.ShippingAddress;
            string newAddress = form["newShippingAddress"];
            if (!string.IsNullOrEmpty(newAddress))
            {
                finalAddress = newAddress;
            }

            // Thiết lập PaymentStatus dựa trên PaymentMethod
            string paymentStatus = "Chưa thanh toán";
            switch (model.PaymentMethod)
            {
                case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                case "Paypal": paymentStatus = "Thanh toán paypal"; break;
                case "Mua trước trả sau": paymentStatus = "Trả góp"; break;
                default: paymentStatus = "Chưa thanh toán"; break;
            }

            // A. TẠO ĐƠN HÀNG (Order)
            Order order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalCartValue(),
                ShippingAddress = finalAddress,
                PaymentMethod = model.PaymentMethod,
                DeliveryMethod = model.ShippingMethod, // Lưu Hình thức giao hàng
                PaymentStatus = paymentStatus
            };
            db.Orders.Add(order);
            db.SaveChanges(); // Lưu để lấy mã OrderID phát sinh

            // B. TẠO CHI TIẾT ĐƠN HÀNG & TRỪ TỒN KHO
            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                db.OrderDetails.Add(detail);

                // Trừ tồn kho
                var product = db.Products.Find(item.ProductID);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }
            db.SaveChanges();

            // C. HOÀN TẤT VÀ CHUYỂN HƯỚNG
            if (model.PaymentMethod == "Paypal")
            {
                // Chuyển sang PaypalController để xử lý thanh toán
                return RedirectToAction("PaymentWithPaypal", "Paypal");
            }
            else
            {
                // Thanh toán tiền mặt (COD)
                Session.Remove("Cart");
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
        }

        // 3. GET: Order/OrderSuccess (Trang thông báo thành công)
        public ActionResult OrderSuccess(int id)
        {
            // Truy vấn lấy Đơn hàng và kèm theo Chi tiết đơn hàng
            var order = db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            return View(order);
        }
        // 4. GET: Order/MyOrder (Trang lịch sử đơn hàng của khách)
        public ActionResult MyOrder()
        {
            // Yêu cầu đăng nhập
            var customer = Session["Customer"] as Customer;
            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách đơn hàng của đúng khách hàng đang đăng nhập, sắp xếp từ mới tới cũ
            var orders = db.Orders
                           .Where(o => o.CustomerID == customer.CustomerID)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();

            return View(orders);
        }
        // 5. GET: Order/OrderDetails (Xem chi tiết 1 đơn hàng)
        public ActionResult OrderDetails(int id)
        {
            // Lấy đơn hàng cùng với danh sách chi tiết (OrderDetails)
            var order = db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            // Đảm bảo chỉ chủ nhân đơn hàng mới được xem
            var customer = Session["Customer"] as Customer;
            if (customer == null || order.CustomerID != customer.CustomerID)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(order);
        }
    }
}