using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminInventoryController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Inventory", new { area = "Admin" });
    }
}
