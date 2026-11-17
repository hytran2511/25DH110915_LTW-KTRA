using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminUserController : Controller
    {
        public ActionResult Index() => RedirectToAction("Index", "Users", new { area = "Admin" });
        public ActionResult Create() => RedirectToAction("Create", "Users", new { area = "Admin" });
        public ActionResult Edit(string id) => RedirectToAction("Edit", "Users", new { area = "Admin", id });
        public ActionResult Delete(string id) => RedirectToAction("Delete", "Users", new { area = "Admin", id });
    }
}
