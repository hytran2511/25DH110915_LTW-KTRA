using _25DH110915_LTW.Models; 
using System.Linq;
using System.Web.Mvc;
using PagedList;
using _25DH110915_LTW.Models.ViewModel;

namespace _25DH110915_LTW.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo Database Context
        private MyStoreEntities db = new MyStoreEntities();

        // Cập nhật hàm Index nhận thêm tham số searchTerm và page
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            // Lấy danh sách sản phẩm, chỉ lấy những sản phẩm còn hàng (Quantity > 0)
            var products = db.Products.Where(p => p.Quantity > 0).AsQueryable();

            // 1. TÌM KIẾM
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                // Tìm theo tên, mô tả hoặc tên danh mục
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                                               p.ProductDecription.Contains(searchTerm) ||
                                               p.Category.CategoryName.Contains(searchTerm));
            }

            // 2. ĐOẠN CODE LIÊN QUAN TỚI PHÂN TRANG
            int pageNumber = page ?? 1; // Nếu page null thì mặc định là trang 1
            int pageSize = 6; // Số sản phẩm mỗi trang

            // Lấy top 10 sản phẩm bán chạy nhất cho phần Nổi bật
            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            // Lấy 20 sản phẩm mới nhất (hoặc ít bán nhất) và phân trang
            model.NewProducts = products.OrderBy(p => p.ProductID).Take(20).ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        // GET: /Home/Search
        public ActionResult Search(string keyword)
        {
            // Lấy toàn bộ sản phẩm
            var products = db.Products.AsQueryable();

            // Nếu người dùng có nhập từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                // Tìm những sản phẩm có tên chứa từ khóa (không phân biệt hoa thường)
                products = products.Where(p => p.ProductName.Contains(keyword));
                ViewBag.Keyword = keyword; // Lưu lại từ khóa để hiển thị ra View
            }

            return View(products.ToList());
        }
    }
}