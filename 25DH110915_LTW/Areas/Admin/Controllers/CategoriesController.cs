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
    public class CategoriesController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Products).ToList();
            return View(categories);
        }

        // GET: Admin/Categories/Details/5 (Chứa Danh sách sản phẩm liên quan thuộc danh mục)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã thêm danh mục mới thành công.";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            // KIỂM TRA RÀNG BUỘC: Danh mục có chứa sản phẩm nào không?
            var productsInCategory = db.Products.Where(p => p.CategoryID == id).ToList();

            if (productsInCategory.Count > 0)
            {
                TempData["ErrorMessage"] = "Không thể xóa Danh mục '" + category.CategoryName + "' vì đang có " + productsInCategory.Count + " hàng hóa thuộc danh mục này!";
                return RedirectToAction("Delete", new { id = category.CategoryID });
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Đã xóa Danh mục thành công.";
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
