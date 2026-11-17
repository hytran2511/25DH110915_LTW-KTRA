using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models; 

namespace _25DH110915_LTW.Controllers
{
    public class AdminCategoryController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH DANH MỤC
        // GET: AdminCategory/Index
        public ActionResult Index()
        {
            var categories = db.Categories.OrderByDescending(c => c.CategoryID).ToList();
            return View(categories);
        }

        // 2. THÊM DANH MỤC MỚI
        // GET: AdminCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index"); // Thêm xong quay lại danh sách
            }
            return View(category);
        }
        //  3. SỬA DANH MỤC 
        // GET: AdminCategory/Edit/5
        public ActionResult Edit(int id)
        {
            var category = db.Categories.SingleOrDefault(c => c.CategoryID == id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        // POST: AdminCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var catInDb = db.Categories.SingleOrDefault(c => c.CategoryID == category.CategoryID);
                if (catInDb != null)
                {
                    catInDb.CategoryName = category.CategoryName;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        //  4. XÓA DANH MỤC 
        // GET: AdminCategory/Delete/5
        public ActionResult Delete(int id)
        {
            var category = db.Categories.SingleOrDefault(c => c.CategoryID == id);
            if (category == null) return HttpNotFound();
            return View(category);
        }

        // POST: AdminCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.SingleOrDefault(c => c.CategoryID == id);
            if (category != null)
            {
                // Kiểm tra xem danh mục có đang chứa sản phẩm nào không (Ngăn lỗi xóa Cascade)
                var productCount = db.Products.Count(p => p.CategoryID == id);
                if (productCount > 0)
                {
                    // Trả về thông báo lỗi nếu danh mục vẫn còn hàng hóa
                    TempData["ErrorMessage"] = $"Không thể xóa! Danh mục này đang chứa {productCount} sản phẩm.";
                    return RedirectToAction("Index");
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã xóa danh mục thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}