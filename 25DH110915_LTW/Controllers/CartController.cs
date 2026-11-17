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
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Hành động Thêm sản phẩm vào giỏ (nhận cả số lượng mua từ trang Product Detail)
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                int stock = product.Quantity ?? 0;
                if (stock <= 0)
                {
                    TempData["CartError"] = "Sản phẩm này hiện đã hết hàng!";
                    return RedirectToAction("Index");
                }

                if (quantity < 1)
                {
                    quantity = 1;
                }

                var cart = GetCart();
                var existing = cart.Items.FirstOrDefault(i => i.ProductID == id);
                int currentInCart = existing != null ? existing.Quantity : 0;

                if (currentInCart + quantity > stock)
                {
                    TempData["CartError"] = $"Số lượng mua vượt quá số lượng tồn kho ({stock} sản phẩm). Đã cập nhật mức tối đa có thể mua!";
                }

                cart.AddProduct(product, quantity);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        // Hành động Xem giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            // Đồng bộ số lượng tồn mới nhất từ DB
            foreach (var item in cart.Items)
            {
                var p = db.Products.Find(item.ProductID);
                if (p != null)
                {
                    item.MaxQuantity = p.Quantity ?? 0;
                }
            }
            return View(cart);
        }

        // Hành động Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                cart.RemoveProduct(id);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        // Hành động Cập nhật số lượng (Kiểm tra không cho nhập <= 0 và > số lượng tồn)
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int newQuantity)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                var product = db.Products.Find(id);
                int maxStock = product != null ? (product.Quantity ?? 0) : 100;

                if (newQuantity <= 0)
                {
                    TempData["CartError"] = "Số lượng hàng phải lớn hơn 0! Đã đặt lại số lượng = 1.";
                    newQuantity = 1;
                }
                else if (newQuantity > maxStock)
                {
                    TempData["CartError"] = $"Sản phẩm '{(product != null ? product.ProductName : "")}' chỉ còn {maxStock} sản phẩm trong kho!";
                    newQuantity = maxStock;
                }

                cart.UpdateQuantity(id, newQuantity, maxStock);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                cart.Clear();
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }
    }
}
