using System;
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
                orders = orders.Where(o => o.PaymentStatus == paymentStatus);
            }

            // 4. Lọc theo Ngày đặt hàng
            if (!string.IsNullOrEmpty(orderDate))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(orderDate, out parsedDate))
                {
                    // Chỉ lấy phần ngày (bỏ qua giờ phút giây) để so sánh
                    orders = orders.Where(o => DbFunctions.TruncateTime(o.OrderDate) == parsedDate.Date);
                }
            }

            // 5. Sắp xếp đơn hàng mới nhất lên đầu
            orders = orders.OrderByDescending(o => o.OrderDate);

            // 6. Phân trang 15 đơn hàng / trang
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            // Lưu lại các tiêu chí tìm kiếm để hiển thị trên View
            ViewBag.SearchName = searchName;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.OrderDate = orderDate;

            return View(orders.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Kết nối bảng Order -> Customer và bảng Order -> OrderDetails -> Product
            Order order = db.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderDetails.Select(od => od.Product))
                            .SingleOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}