using _25DH110915_LTW.Models;
using _25DH110915_LTW.Models.ViewModel;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Child Action lấy danh sách Category cho Menu trên _Layout
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = db.Categories.Include(c => c.Products).ToList();
            return PartialView("~/Views/Shared/_CategoryMenu.cshtml", categories);
        }

        // Trang Home: Hiển thị danh mục theo DB, Sản phẩm nổi bật và Sản phẩm mới (chỉ lấy Quantity > 0)
        public ActionResult Index(string searchTerm, int? categoryId, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products
                             .Include(p => p.Category)
                             .Include(p => p.OrderDetails)
                             .Where(p => p.Quantity > 0)
                             .AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                model.SearchTerm = searchTerm.Trim();
                products = products.Where(p => p.ProductName.Contains(model.SearchTerm) ||
                                               p.ProductDecription.Contains(model.SearchTerm) ||
                                               p.Category.CategoryName.Contains(model.SearchTerm));
            }

            int pageNumber = page ?? 1;
            int pageSize = 8;

            // Sản phẩm nổi bật (Bán chạy nhất theo tổng số lượng đã bán trong OrderDetails)
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Sum(od => (int?)od.Quantity) ?? 0)
                .ThenByDescending(p => p.ProductID)
                .Take(5)
                .ToList();

            // Sản phẩm mới nhất có phân trang
            model.NewProducts = products
                .OrderByDescending(p => p.ProductID)
                .ToPagedList(pageNumber, pageSize);

            ViewBag.Categories = db.Categories.Include(c => c.Products).ToList();
            return View(model);
        }

        public ActionResult ProductDetail(int id)
        {
            var product = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.OrderDetails)
                            .SingleOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Đồ án Lập trình Web - Sinh viên: Trần Gia Hy (25DH110915)";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ Trần Gia Hy - 25DH110915";
            return View();
        }

        public ActionResult Search(string keyword)
        {
            return RedirectToAction("Index", "Products", new { searchTerm = keyword });
        }
    }
}
