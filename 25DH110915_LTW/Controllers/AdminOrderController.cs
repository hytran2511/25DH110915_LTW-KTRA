using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AdminOrderController : Controller
    {
        public ActionResult Index(string searchName, string searchStatus, string searchDate, int? page)
        {
            return RedirectToAction("Index", "Orders", new { area = "Admin", searchName = searchName, paymentStatus = searchStatus, orderDate = searchDate, page = page });
        }

        public ActionResult Details(int id)
        {
            return RedirectToAction("DetailedOrder", "Orders", new { area = "Admin", id = id });
        }

        public ActionResult DetailedOrder(int id)
        {
            return RedirectToAction("DetailedOrder", "Orders", new { area = "Admin", id = id });
        }

        public ActionResult UpdateStatus(int id)
        {
            return RedirectToAction("UpdateStatus", "Orders", new { area = "Admin", id = id });
        }
    }
}
