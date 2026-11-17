using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models;
using _25DH110915_LTW.Models.ViewModel;

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Home (Trang thống kê)
        public ActionResult Index()
        {
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalRevenue = db.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;

            var chartData = db.Categories.Select(c => new CategoryStatVM
            {
                CategoryName = c.CategoryName,
                ProductCount = c.Products.Count,
                MaxPrice = c.Products.Max(p => (decimal?)p.ProductPrice) ?? 0,
                MinPrice = c.Products.Min(p => (decimal?)p.ProductPrice) ?? 0,
                AvgPrice = c.Products.Average(p => (decimal?)p.ProductPrice) ?? 0
            }).ToList();

            ViewBag.ChartData = chartData;
            return View(chartData);
        }
    }
}
