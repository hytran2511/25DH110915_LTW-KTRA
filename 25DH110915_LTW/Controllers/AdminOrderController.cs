using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Controllers
{
    public class AdminOrderController : Controller
    {
        // Khởi tạo Database. 
        // (Lưu ý: Nếu tên DbContext của bạn khác MyStoreEntities thì hãy đổi lại cho đúng nhé)
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH & TÌM KIẾM ĐƠN HÀNG
        // Tham số nhận từ thanh tìm kiếm
        public ActionResult Index(string searchName, string searchStatus, string searchDate)
        {
            // Bắt đầu với toàn bộ danh sách đơn hàng
            var orders = db.Orders.AsQueryable();

            // 1. Lọc theo tên khách hàng
            if (!string.IsNullOrEmpty(searchName))
            {
                orders = orders.Where(o => o.Customer.CustomerName.Contains(searchName));
            }

            // 2. Lọc theo trạng thái thanh toán
            if (!string.IsNullOrEmpty(searchStatus))
            {
                orders = orders.Where(o => o.PaymentStatus == searchStatus);
            }

            // 3. Lọc theo ngày đặt hàng
            if (!string.IsNullOrEmpty(searchDate))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(searchDate, out parsedDate))
                {
                    // Đã bỏ .HasValue và .Value
                    orders = orders.Where(o => o.OrderDate.Year == parsedDate.Year &&
                                               o.OrderDate.Month == parsedDate.Month &&
                                               o.OrderDate.Day == parsedDate.Day);
                }
            }

            // Sắp xếp đơn hàng mới nhất lên đầu
            var finalOrders = orders.OrderByDescending(o => o.OrderDate).ToList();

            // Lưu lại từ khóa để giữ chữ trên ô tìm kiếm sau khi load lại trang
            ViewBag.CurrentFilter = searchName;

            return View(finalOrders);
        }

        // ================= XEM CHI TIẾT ĐƠN HÀNG =================
        // GET: AdminOrder/Details/5
        public ActionResult Details(int id)
        {
            // Tìm đơn hàng theo ID
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // 2. HIỂN THỊ FORM CẬP NHẬT TRẠNG THÁI
        // GET: AdminOrder/UpdateStatus/5
        public ActionResult UpdateStatus(int id)
        {
            // Tìm đơn hàng theo ID
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // 3. LƯU TRẠNG THÁI MỚI VÀO DATABASE
        // POST: AdminOrder/UpdateStatus/5
        [HttpPost]
        public ActionResult UpdateStatus(int id, string PaymentStatus)
        {
            var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
            if (order != null)
            {
                // Cập nhật trạng thái mới
                order.PaymentStatus = PaymentStatus;
                db.SaveChanges();
            }
            // Quay về trang danh sách đơn hàng
            return RedirectToAction("Index");
        }
    }
}