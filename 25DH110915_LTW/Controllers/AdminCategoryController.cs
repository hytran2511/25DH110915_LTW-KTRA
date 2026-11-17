using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminCategoryController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Categories", new { area = "Admin" });
        public ActionResult Details(int id) => RedirectToAction("Details", "Categories", new { area = "Admin", id });
        public ActionResult Create() => RedirectToAction("Create", "Categories", new { area = "Admin" });
        public ActionResult Edit(int id) => RedirectToAction("Edit", "Categories", new { area = "Admin", id });
        public ActionResult Delete(int id) => RedirectToAction("Delete", "Categories", new { area = "Admin", id });
    }
}
