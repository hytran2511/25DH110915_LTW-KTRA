using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _25DH110915_LTW.Models.ViewModel
{
    public class CheckoutVM
    {
        public List<CartItem> CartItems { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Họ tên người nhận")]
        public string CustomerName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        public string Username { get; set; }

        // Các thuộc tính khác của đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
