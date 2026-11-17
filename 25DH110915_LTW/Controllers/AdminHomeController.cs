using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminHomeController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Home", new { area = "Admin" });
    }
}
