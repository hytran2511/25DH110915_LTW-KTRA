using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminProductController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "AdminProducts", new { area = "Admin" });
        public ActionResult Details(int id) => RedirectToAction("Details", "AdminProducts", new { area = "Admin", id });
        public ActionResult Create() => RedirectToAction("Create", "AdminProducts", new { area = "Admin" });
        public ActionResult Edit(int id) => RedirectToAction("Edit", "AdminProducts", new { area = "Admin", id });
        public ActionResult Delete(int id) => RedirectToAction("Delete", "AdminProducts", new { area = "Admin", id });
    }
}
