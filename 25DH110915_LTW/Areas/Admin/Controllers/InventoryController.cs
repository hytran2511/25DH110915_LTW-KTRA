using System.Linq;
using System.Web.Mvc;
using _25DH110915_LTW.Models; 

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Inventory
        public ActionResult Index()
        {
            // Truy vấn toàn bộ sản phẩm
            var allProducts = db.Products.ToList();

            // Lọc hàng tồn (Số lượng > 0)
            ViewBag.InStock = allProducts.Where(p => p.Quantity > 0).ToList();

            // Lọc hàng cần nhập (Số lượng == 0 hoặc null)
            ViewBag.OutOfStock = allProducts.Where(p => p.Quantity == null || p.Quantity <= 0).ToList();

            return View();
        }
    }
}