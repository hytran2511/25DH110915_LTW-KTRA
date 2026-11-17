using System.Linq;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Controllers
{
    public class AdminCustomerController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // HIỂN THỊ DANH SÁCH KHÁCH HÀNG
        public ActionResult Index()
        {
            // Sắp xếp khách hàng mới đăng ký lên đầu
            var customers = db.Customers.OrderByDescending(c => c.CustomerID).ToList();
            return View(customers);
        }
        // XEM CHI TIẾT KHÁCH HÀNG 
        // GET: AdminCustomer/Details/5
        public ActionResult Details(int id)
        {
            var customer = db.Customers.SingleOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
    }
}