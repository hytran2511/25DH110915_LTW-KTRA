using System.Data.Entity;
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
            var allProducts = db.Products.Include(p => p.Category).Include(p => p.OrderDetails).ToList();

            // 1. Danh sách hàng tồn: Số lượng tồn > 0
            ViewBag.InStock = allProducts.Where(p => (p.Quantity ?? 0) > 0).OrderByDescending(p => p.Quantity).ToList();

            // 2. Danh sách hàng cần nhập: Số lượng tồn == 0 (hoặc null)
            ViewBag.OutOfStock = allProducts.Where(p => (p.Quantity ?? 0) <= 0).ToList();

            return View();
        }
    }
}
