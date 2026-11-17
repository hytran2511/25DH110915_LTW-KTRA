using _25DH110915_LTW.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace _25DH110915_LTW.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. GET: Đăng ký
        public ActionResult Register()
        {
            return View();
        }

        // 2. POST: Xử lý Đăng ký (Lưu vào 2 bảng User và Customer)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer cust, string txtUsername, string txtPassword)
        {
            // Kiểm tra xem Username đã tồn tại trong bảng User chưa
            var checkUser = db.Users.FirstOrDefault(u => u.Username == txtUsername);
            if (checkUser == null)
            {
                // Bước 1: Lưu tài khoản mới vào bảng User
                User newUser = new User();
                newUser.Username = txtUsername;
                newUser.Password = txtPassword;
                newUser.UserRole = "Customer"; // Set quyền mặc định là khách hàng
                db.Users.Add(newUser);

                // Bước 2: Lưu thông tin cá nhân vào bảng Customer và liên kết với Username vừa tạo
                cust.Username = txtUsername;
                db.Customers.Add(cust);

                // Lưu cả 2 bảng vào Database
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.ErrorRegister = "Tên đăng nhập này đã có người sử dụng!";
                return View();
            }
        }

        // 3. GET: Đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // 4. POST: Xử lý Đăng nhập
        [HttpPost]
        // BƯỚC 1: Sửa tên tham số ở đây thành txtUsername và txtPassword cho khớp với file HTML
        public ActionResult Login(string txtUsername, string txtPassword)
        {
            // BƯỚC 2: Cập nhật biến trong câu truy vấn
            var user = db.Users.SingleOrDefault(u => u.Username == txtUsername && u.Password == txtPassword);

            if (user != null)
            {
                // Kiểm tra xem UserRole có phải là Admin không
                if (!string.IsNullOrEmpty(user.UserRole) && user.UserRole.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    Session["Admin"] = user; // Lưu session Admin
                    return RedirectToAction("Index", "AdminOrder"); // Đá thẳng vào trang quản lý đơn hàng của Admin
                }
                else
                {
                    // Nếu là khách hàng bình thường
                    var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                    if (customer != null)
                    {
                        Session["Customer"] = customer;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Tài khoản chưa được liên kết với thông tin khách hàng!";
                        return View();
                    }
                }
            }

            // Nếu không tìm thấy user hoặc sai mật khẩu
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác!";
            return View();
        }

        // 5. Đăng xuất
        public ActionResult Logout()
        {
            Session.Remove("Customer");
            return RedirectToAction("Index", "Home");
        }
        // 6. GET: Hiển thị trang Thông tin tài khoản
        public ActionResult ProfileInfo()
        {
            // Lấy thông tin từ Session
            var currentCustomer = Session["Customer"] as _25DH110915_LTW.Models.Customer;

            // Nếu chưa đăng nhập thì đẩy về trang Login
            if (currentCustomer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Gọi Database lấy thông tin mới nhất của user này lên
            var customerInfo = db.Customers.Find(currentCustomer.CustomerID);
            return View(customerInfo);
        }

        // POST: Account/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(_25DH110915_LTW.Models.Customer model)
        {
            var sessionCustomer = Session["Customer"] as _25DH110915_LTW.Models.Customer;
            if (sessionCustomer == null) return RedirectToAction("Login");

            // Lấy thông tin khách hàng từ DB
            var customerInDb = db.Customers.SingleOrDefault(c => c.CustomerID == sessionCustomer.CustomerID);

            if (customerInDb != null)
            {
                // Cập nhật cả 4 trường thông tin
                customerInDb.CustomerName = model.CustomerName;
                customerInDb.CustomerAddress = model.CustomerAddress;
                customerInDb.CustomerPhone = model.CustomerPhone;
                customerInDb.CustomerEmail = model.CustomerEmail;

                db.SaveChanges(); // Lưu vào SQL Server

                // Cập nhật lại Session để tên mới (nếu có đổi) hiển thị đúng trên góc phải màn hình
                Session["Customer"] = customerInDb;

                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            }

            return RedirectToAction("ProfileInfo");
        }
        // ================= KHU VỰC SỔ ĐỊA CHỈ =================
        // GET: Account/AddressBook
        public ActionResult AddressBook()
        {
            var customer = Session["Customer"] as _25DH110915_LTW.Models.Customer;
            if (customer == null) return RedirectToAction("Login");

            // Lấy dữ liệu mới nhất từ DB
            var customerInDb = db.Customers.SingleOrDefault(c => c.CustomerID == customer.CustomerID);
            return View(customerInDb);
        }

        //  KHU VỰC ĐỔI MẬT KHẨU 
        // GET: Account/ChangePassword
        public ActionResult ChangePassword()
        {
            if (Session["Customer"] == null) return RedirectToAction("Login");
            return View();
        }
        // POST: Account/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            var customer = Session["Customer"] as _25DH110915_LTW.Models.Customer;
            if (customer == null) return RedirectToAction("Login");

            // 1. Tìm tài khoản đăng nhập trong bảng User dựa vào Username của khách hàng
            var userInDb = db.Users.SingleOrDefault(u => u.Username == customer.Username);

            if (userInDb == null)
            {
                ViewBag.Error = "Lỗi hệ thống: Không tìm thấy tài khoản!";
                return View();
            }

            // 2. Kiểm tra mật khẩu cũ (So sánh với bảng User)
            if (userInDb.Password != OldPassword)
            {
                ViewBag.Error = "Mật khẩu cũ không chính xác!";
                return View();
            }

            // 3. Kiểm tra mật khẩu mới và xác nhận
            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            // 4. Cập nhật mật khẩu mới vào bảng User và lưu lại
            userInDb.Password = NewPassword;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ProfileInfo");
        }
    }
}