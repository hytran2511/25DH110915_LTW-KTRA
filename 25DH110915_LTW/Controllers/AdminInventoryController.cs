using System.Linq;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Controllers
{
    public class AdminInventoryController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: AdminInventory
        public ActionResult Index()
        {
            // 1. Lấy danh sách hàng còn tồn (Số lượng > 0)
            ViewBag.InStock = db.Products.Where(p => p.Quantity > 0).ToList();

            // 2. Lấy danh sách hàng đã hết / cần nhập (Số lượng <= 0 hoặc bằng null)
            ViewBag.OutOfStock = db.Products.Where(p => p.Quantity <= 0 || p.Quantity == null).ToList();

            return View();
        }
    }
}