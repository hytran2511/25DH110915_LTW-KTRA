using System;
using System.ComponentModel.DataAnnotations;

namespace _25DH110915_LTW.Models
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public class CategoryMetadata
    {
        [Display(Name = "Mã danh mục")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống!")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên danh mục phải từ 2 đến 100 ký tự!")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }

    public class ProductMetadata
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủng loại / danh mục!")]
        [Display(Name = "Chủng loại")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Tên sản phẩm phải từ 2 đến 200 ký tự!")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống!")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Mô tả sản phẩm phải từ 5 đến 2000 ký tự!")]
        [Display(Name = "Mô tả")]
        public string ProductDecription { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống!")]
        [Range(1000, 1000000000, ErrorMessage = "Đơn giá phải từ 1.000 đến 1.000.000.000 VNĐ!")]
        [DataType(DataType.Currency)]
        [Display(Name = "Đơn giá")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Hình ảnh")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Số lượng tồn không được để trống!")]
        [Range(0, 100000, ErrorMessage = "Số lượng tồn phải từ 0 đến 100.000!")]
        [Display(Name = "Số lượng tồn")]
        public Nullable<int> Quantity { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }

    public class UserMetadata
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự!")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mật khẩu phải từ 3 đến 50 ký tự!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống!")]
        [StringLength(50, ErrorMessage = "Vai trò tối đa 50 ký tự!")]
        [Display(Name = "Vai trò")]
        public string UserRole { get; set; }
    }

    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }

    public class CustomerMetadata
    {
        [Display(Name = "Mã khách hàng")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Họ và tên khách hàng không được để trống!")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ và tên phải từ 2 đến 100 ký tự!")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Số điện thoại phải từ 7 đến 15 chữ số!")]
        [RegularExpression(@"^[0-9+\-\s()]{7,15}$", ErrorMessage = "Số điện thoại chỉ được chứa chữ số (7-15 số)!")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng!")]
        [StringLength(100, ErrorMessage = "Email tối đa 100 ký tự!")]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Địa chỉ phải từ 3 đến 255 ký tự!")]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
    }

    public partial class Order
    {
        public string AddressDelivery
        {
            get { return ShippingAddress; }
            set { ShippingAddress = value; }
        }
    }
}
