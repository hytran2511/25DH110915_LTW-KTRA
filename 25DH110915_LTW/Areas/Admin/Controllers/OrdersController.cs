using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders
        public ActionResult Index(string searchName, string paymentStatus, string orderDate, int? page)
        {
            // 1. Lấy danh sách đơn hàng kèm thông tin Khách hàng
            var orders = db.Orders.Include(o => o.Customer).AsQueryable();

            // 2. Lọc theo Tên khách hàng
            if (!string.IsNullOrEmpty(searchName))
            {
                orders = orders.Where(o => o.Customer.CustomerName.Contains(searchName));
            }

            // 3. Lọc theo Trạng thái thanh toán
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                orders = orders.Where(o => o.PaymentStatus.Contains(paymentStatus));
            }

            // 4. Lọc theo Ngày đặt hàng
            if (!string.IsNullOrEmpty(orderDate))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(orderDate, out parsedDate))
                {
                    DateTime targetDate = parsedDate.Date;
                    orders = orders.Where(o => DbFunctions.TruncateTime(o.OrderDate) == targetDate);
                }
            }

            // 5. Sắp xếp đơn hàng mới nhất lên đầu
            orders = orders.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderID);

            // 6. Phân trang 15 đơn hàng / trang (theo đúng yêu cầu PPT)
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            // Lưu lại các tiêu chí tìm kiếm để hiển thị trên View
            ViewBag.SearchName = searchName;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.OrderDate = orderDate;

            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Orders/DetailedOrder/5 (Theo đúng tên View yêu cầu trong PPT)
        public ActionResult DetailedOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderDetails.Select(od => od.Product))
                            .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View("DetailedOrder", order);
        }

        // GET: Admin/Orders/Details/5 (Hỗ trợ cả đường dẫn Details và DetailedOrder)
        public ActionResult Details(int? id)
        {
            return DetailedOrder(id);
        }

        // GET: Admin/Orders/UpdateStatus/5
        public ActionResult UpdateStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders
                            .Include(o => o.Customer)
                            .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string PaymentStatus, string DeliveryMethod)
        {
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(PaymentStatus))
            {
                order.PaymentStatus = PaymentStatus;
            }
            if (!string.IsNullOrEmpty(DeliveryMethod))
            {
                order.DeliveryMethod = DeliveryMethod;
            }

            db.SaveChanges();
            TempData["SuccessMessage"] = "Đã cập nhật trạng thái đơn hàng #" + id + " thành công!";
            return RedirectToAction("DetailedOrder", new { id = order.OrderID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
