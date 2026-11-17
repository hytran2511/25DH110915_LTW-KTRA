using System.Linq;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Controllers
{
    public class AdminHomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index()
        {
            // 1. Đếm tổng số lượng
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalProducts = db.Products.Count();

            // 2. Tính tổng doanh thu (Ép kiểu nullable decimal để tránh lỗi nếu chưa có đơn hàng nào)
            decimal totalRevenue = db.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;
            ViewBag.TotalRevenue = totalRevenue;

            // 3. Lấy 5 đơn hàng mới nhất để hiển thị nhanh
            var recentOrders = db.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}