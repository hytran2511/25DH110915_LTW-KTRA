using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Users (Chỉ lưu giữ danh sách người quản trị)
        public ActionResult Index()
        {
            var adminUsers = db.Users
                .Where(u => u.UserRole.Trim() == "Admin" || u.UserRole.Trim() == "A" || u.UserRole.Trim() == "Quản trị")
                .ToList();
            return View(adminUsers);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View(new User { UserRole = "Admin" });
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,UserRole")] User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserRole))
            {
                user.UserRole = "Admin";
            }

            if (ModelState.IsValid)
            {
                string uname = user.Username.Trim();
                var existingUser = db.Users.SingleOrDefault(u => u.Username == uname);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại, vui lòng chọn tên khác!");
                    return View(user);
                }

                user.Username = uname;
                user.Password = user.Password.Trim();
                db.Users.Add(user);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã thêm tài khoản Quản trị viên mới thành công!";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = (user.Password ?? "").Trim();
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                var userInDb = db.Users.Find(user.Username);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }

                userInDb.Password = (user.Password ?? "").Trim();
                userInDb.UserRole = string.IsNullOrWhiteSpace(user.UserRole) ? "Admin" : user.UserRole.Trim();
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã cập nhật tài khoản Quản trị viên thành công!";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra ràng buộc dữ liệu với bảng Customer
            if (db.Customers.Any(c => c.Username == id))
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản này vì đang liên kết với hồ sơ khách hàng!";
                return RedirectToAction("Delete", new { id = id });
            }

            db.Users.Remove(user);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Đã xóa tài khoản Quản trị viên thành công.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
