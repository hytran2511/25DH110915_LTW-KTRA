using System;
using System.Linq;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Controllers
{
    public class AdminUserController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH ADMIN
        public ActionResult Index()
        {
            // Lọc danh sách chỉ hiển thị những ai là Quản trị viên
            var admins = db.Users.Where(u => u.UserRole == "Admin" || u.UserRole == "A" || u.UserRole == "Quản trị").ToList();
            return View(admins);
        }

        // 2. THÊM TÀI KHOẢN ADMIN MỚI
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username đã tồn tại chưa
                if (db.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(user);
                }

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // 3. SỬA TÀI KHOẢN
        public ActionResult Edit(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var userInDb = db.Users.SingleOrDefault(u => u.Username == user.Username);
                if (userInDb != null)
                {
                    userInDb.Password = user.Password;
                    userInDb.UserRole = user.UserRole;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // 4. XÓA TÀI KHOẢN
        public ActionResult Delete(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.Username == id);
            if (user != null)
            {
                // Không cho phép tự xóa chính mình (nếu cần) - tùy chọn nâng cao
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}