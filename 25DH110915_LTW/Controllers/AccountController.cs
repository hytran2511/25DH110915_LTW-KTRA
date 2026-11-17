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

        // 2. POST: Xử lý Đăng ký (Kiểm tra dữ liệu nhập DataAnnotation, trùng tài khoản/SĐT/Email, lưu User và Customer)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer cust, string txtUsername, string txtPassword, string txtConfirmPassword)
        {
            txtUsername = (txtUsername ?? "").Trim();
            txtPassword = (txtPassword ?? "").Trim();

            if (string.IsNullOrEmpty(txtUsername) || txtUsername.Length < 3 || txtUsername.Length > 50)
            {
                ViewBag.ErrorRegister = "Tên đăng nhập phải từ 3 đến 50 ký tự!";
                return View(cust);
            }

            if (string.IsNullOrEmpty(txtPassword) || txtPassword.Length < 3 || txtPassword.Length > 50)
            {
                ViewBag.ErrorRegister = "Mật khẩu phải từ 3 đến 50 ký tự!";
                return View(cust);
            }

            if (!string.IsNullOrEmpty(txtConfirmPassword) && txtPassword != txtConfirmPassword.Trim())
            {
                ViewBag.ErrorRegister = "Mật khẩu xác nhận không khớp!";
                return View(cust);
            }

            // Kiểm tra trùng Username
            var checkUser = db.Users.FirstOrDefault(u => u.Username == txtUsername);
            if (checkUser != null)
            {
                ViewBag.ErrorRegister = "Đăng ký thất bại: Tên đăng nhập '" + txtUsername + "' đã tồn tại!";
                return View(cust);
            }

            // Kiểm tra trùng Email hoặc SĐT nếu có
            if (!string.IsNullOrWhiteSpace(cust.CustomerEmail) && db.Customers.Any(c => c.CustomerEmail == cust.CustomerEmail))
            {
                ViewBag.ErrorRegister = "Đăng ký thất bại: Email này đã được sử dụng cho tài khoản khác!";
                return View(cust);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorRegister = "Đăng ký thất bại: Vui lòng kiểm tra lại các trường thông tin hợp lệ!";
                return View(cust);
            }

            // Bước 1: Lưu tài khoản mới vào bảng User
            User newUser = new User
            {
                Username = txtUsername,
                Password = txtPassword,
                UserRole = "C"
            };
            db.Users.Add(newUser);

            // Bước 2: Lưu thông tin cá nhân vào bảng Customer
            cust.Username = txtUsername;
            db.Customers.Add(cust);

            db.SaveChanges();
            TempData["RegisterSuccess"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // 3. GET: Đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // 4. POST: Xử lý Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string txtUsername, string txtPassword)
        {
            txtUsername = (txtUsername ?? "").Trim();
            txtPassword = (txtPassword ?? "").Trim();

            if (string.IsNullOrEmpty(txtUsername) || string.IsNullOrEmpty(txtPassword))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!";
                ViewBag.ErrorLogin = ViewBag.Error;
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Username == txtUsername);

            if (user != null && user.Password != null && user.Password.Trim() == txtPassword)
            {
                string role = (user.UserRole ?? "").Trim();
                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("A", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("Quản trị", StringComparison.OrdinalIgnoreCase))
                {
                    Session["Admin"] = user;
                    return RedirectToAction("Index", "Orders", new { area = "Admin" });
                }
                else
                {
                    var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
                    if (customer != null)
                    {
                        Session["Customer"] = customer;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Tài khoản chưa được liên kết với thông tin khách hàng!";
                        ViewBag.ErrorLogin = ViewBag.Error;
                        return View();
                    }
                }
            }

            ViewBag.Error = "Đăng nhập thất bại: Tên đăng nhập hoặc mật khẩu không chính xác!";
            ViewBag.ErrorLogin = ViewBag.Error;
            return View();
        }

        // 5. Đăng xuất
        public ActionResult Logout()
        {
            Session.Remove("Customer");
            Session.Remove("Admin");
            return RedirectToAction("Index", "Home");
        }

        // 6. GET: Hiển thị trang Hồ sơ khách hàng (ProfileInfo)
        public ActionResult ProfileInfo()
        {
            var currentCustomer = Session["Customer"] as Customer;
            if (currentCustomer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customerInfo = db.Customers.Find(currentCustomer.CustomerID);
            return View(customerInfo);
        }

        // POST: Account/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(Customer model)
        {
            var sessionCustomer = Session["Customer"] as Customer;
            if (sessionCustomer == null) return RedirectToAction("Login");

            var customerInDb = db.Customers.SingleOrDefault(c => c.CustomerID == sessionCustomer.CustomerID);
            if (customerInDb != null)
            {
                customerInDb.CustomerName = model.CustomerName;
                customerInDb.CustomerAddress = model.CustomerAddress;
                customerInDb.CustomerPhone = model.CustomerPhone;
                customerInDb.CustomerEmail = model.CustomerEmail;

                db.SaveChanges();
                Session["Customer"] = customerInDb;
                TempData["SuccessMessage"] = "Cập nhật thông tin hồ sơ khách hàng thành công!";
            }

            return RedirectToAction("ProfileInfo");
        }

        // GET: Account/AddressBook
        public ActionResult AddressBook()
        {
            var customer = Session["Customer"] as Customer;
            if (customer == null) return RedirectToAction("Login");

            var customerInDb = db.Customers.SingleOrDefault(c => c.CustomerID == customer.CustomerID);
            return View(customerInDb);
        }

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
            var customer = Session["Customer"] as Customer;
            if (customer == null) return RedirectToAction("Login");

            var userInDb = db.Users.SingleOrDefault(u => u.Username == customer.Username);
            if (userInDb == null)
            {
                ViewBag.Error = "Lỗi hệ thống: Không tìm thấy tài khoản!";
                return View();
            }

            if ((userInDb.Password ?? "").Trim() != (OldPassword ?? "").Trim())
            {
                ViewBag.Error = "Mật khẩu cũ không chính xác!";
                return View();
            }

            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            userInDb.Password = (NewPassword ?? "").Trim();
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ProfileInfo");
        }
    }
}
