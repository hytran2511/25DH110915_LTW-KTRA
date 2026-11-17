using System.Collections.Generic;
using System.Linq;

namespace _25DH110915_LTW.Models
{
    // Lớp đại diện cho 1 sản phẩm trong giỏ hàng
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    // Lớp quản lý toàn bộ giỏ hàng
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items => items;

        // Thêm sản phẩm vào giỏ
        public void AddProduct(Product prod, int qty = 1)
        {
            var item = items.FirstOrDefault(s => s.ProductID == prod.ProductID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    ProductID = prod.ProductID,
                    ProductName = prod.ProductName,
                    ProductImage = prod.ProductImage,
                    Price = prod.ProductPrice,
                    Quantity = qty
                });
            }
            else
            {
                item.Quantity += qty; // Nếu đã có thì tăng số lượng
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveProduct(int id)
        {
            items.RemoveAll(s => s.ProductID == id);
        }

        // Tính tổng tiền
        public decimal TotalCartValue()
        {
            return items.Sum(s => s.TotalPrice);
        }

        // Tính tổng số lượng
        public int TotalQuantity()
        {
            return items.Sum(s => s.Quantity);
        }
        // Cập nhật số lượng của sản phẩm trong giỏ
        public void UpdateQuantity(int id, int newQty)
        {
            var item = items.FirstOrDefault(s => s.ProductID == id);
            if (item != null)
            {
                item.Quantity = newQty; // Gán số lượng mới
            }
        }
    }
}