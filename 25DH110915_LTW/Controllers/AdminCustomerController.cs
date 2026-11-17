using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminCustomerController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Customers", new { area = "Admin" });
        public ActionResult Details(int id) => RedirectToAction("Details", "Customers", new { area = "Admin", id });
    }
}
