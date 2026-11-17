using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models; 

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            // 1. Lấy dữ liệu thống kê cơ bản hiển thị trên các thẻ (Card)
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalCustomers = db.Customers.Count();

            // 2. Chuẩn bị dữ liệu cho biểu đồ Google Chart (Thống kê số lượng SP theo Danh mục)
            // Lấy tên danh mục và đếm số sản phẩm bên trong danh mục đó
            var chartData = db.Categories.Select(c => new
            {
                CategoryName = c.CategoryName,
                ProductCount = c.Products.Count
            }).ToList();

            // Truyền dữ liệu dạng List sang View để vẽ biểu đồ
            ViewBag.ChartData = chartData;

            return View();
        }
    }
}