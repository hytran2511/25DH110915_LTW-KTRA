using _25DH110915_LTW.Models;
using System.Linq;
using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Hàm lấy giỏ hàng hiện tại từ Session
        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Hành động Thêm sản phẩm vào giỏ
        public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                GetCart().AddProduct(product);
            }
            return RedirectToAction("Index"); // Chuyển tới trang xem giỏ hàng
        }

        // Hành động Xem giỏ hàng
        public ActionResult Index()
        {
            return View(GetCart());
        }
        // Hành động Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int id)
        {
            // Lấy giỏ hàng từ Session
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                // Gọi hàm xóa sản phẩm dựa vào ID
                cart.RemoveProduct(id);
                // Cập nhật lại Session
                Session["Cart"] = cart;
            }
            // Tải lại trang Giỏ hàng để cập nhật bảng lưới
            return RedirectToAction("Index");
        }
        // Hành động Cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int newQuantity)
        {
            // Lấy giỏ hàng từ Session
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                // Gọi hàm cập nhật số lượng
                cart.UpdateQuantity(id, newQuantity);
                Session["Cart"] = cart;
            }
            // Tải lại trang Giỏ hàng
            return RedirectToAction("Index");
        }

    }
}