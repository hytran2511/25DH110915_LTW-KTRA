using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using _25DH110915_LTW.Models;
using _25DH110915_LTW.Models.ViewModel;

namespace _25DH110915_LTW.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Trang Product Category: Tìm kiếm, Lọc theo danh mục/khoảng giá/sắp xếp, Phân trang, chỉ lấy sản phẩm có số lượng tồn > 0
        public ActionResult Index(int? categoryId, string searchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder, int? page)
        {
            var products = db.Products
                             .Include(p => p.Category)
                             .Include(p => p.OrderDetails)
                             .Where(p => p.Quantity > 0)
                             .AsQueryable();

            string currentCategoryName = "Tất cả sản phẩm";

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
                var cat = db.Categories.Find(categoryId.Value);
                if (cat != null)
                {
                    currentCategoryName = cat.CategoryName;
                }
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string kw = searchTerm.Trim();
                products = products.Where(p => p.ProductName.Contains(kw) ||
                                               p.ProductDecription.Contains(kw) ||
                                               p.Category.CategoryName.Contains(kw));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }

            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "bestseller":
                    products = products.OrderByDescending(p => p.OrderDetails.Sum(od => (int?)od.Quantity) ?? 0);
                    break;
                default:
                    products = products.OrderByDescending(p => p.ProductID);
                    break;
            }

            int pageNumber = page ?? 1;
            int pageSize = 8;

            var vm = new ProductSearchVM
            {
                CategoryID = categoryId,
                CategoryName = currentCategoryName,
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOrder = sortOrder,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Categories = db.Categories.ToList(),
                Products = products.ToPagedList(pageNumber, pageSize)
            };

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var product = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.OrderDetails)
                            .SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}
