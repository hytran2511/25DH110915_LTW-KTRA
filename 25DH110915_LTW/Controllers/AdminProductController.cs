using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models; 

namespace _25DH110915_LTW.Controllers
{
    public class AdminProductController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // 1. HIỂN THỊ DANH SÁCH SẢN PHẨM
        // GET: AdminProduct/Index
        public ActionResult Index()
        {
            // Lấy danh sách sản phẩm, sắp xếp theo ID giảm dần (sản phẩm mới nhất lên đầu)
            var products = db.Products.OrderByDescending(p => p.ProductID).ToList();
            return View(products);
        }
        // 2. THÊM SẢN PHẨM MỚI
        // GET: AdminProduct/Create
        public ActionResult Create()
        {
            // Lấy danh sách danh mục từ DB để hiển thị dạng DropDownList (menu xổ xuống)
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: AdminProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Lưu ý: Có thêm tham số HttpPostedFileBase uploadImage để nhận file ảnh từ form
        public ActionResult Create(_25DH110915_LTW.Models.Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng có chọn file ảnh không
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    // Lấy tên file
                    string fileName = System.IO.Path.GetFileName(uploadImage.FileName);

                    // Tạo đường dẫn lưu file vào thư mục Images của project
                    // (Bạn đảm bảo project của mình có thư mục tên là "Images" nhé)
                    string path = Server.MapPath("~/Images/" + fileName);

                    // Lưu file lên server
                    uploadImage.SaveAs(path);

                    // Lưu tên file vào cột ProductImage trong Database
                    product.ProductImage = fileName;
                }

                // Thêm sản phẩm vào DB và lưu lại
                db.Products.Add(product);
                db.SaveChanges();

                // Trở về trang Danh sách
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, load lại DropDownList và hiển thị form
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }
        // ================= 3. SỬA SẢN PHẨM =================
        // GET: AdminProduct/Edit/5
        public ActionResult Edit(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();

            // Load danh sách danh mục để chọn lại
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: AdminProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(_25DH110915_LTW.Models.Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var productInDb = db.Products.SingleOrDefault(p => p.ProductID == product.ProductID);
                if (productInDb != null)
                {
                    // Cập nhật thông tin chữ
                    productInDb.ProductName = product.ProductName;
                    productInDb.ProductPrice = product.ProductPrice;
                    productInDb.Quantity = product.Quantity;
                    productInDb.CategoryID = product.CategoryID;

                    // (Lưu ý: Nếu bảng Product không có ProductDescription thì xóa dòng bên dưới đi)
                    // productInDb.ProductDescription = product.ProductDescription; 

                    // Nếu có chọn ảnh mới thì mới cập nhật ảnh, không thì giữ nguyên ảnh cũ
                    if (uploadImage != null && uploadImage.ContentLength > 0)
                    {
                        string fileName = System.IO.Path.GetFileName(uploadImage.FileName);
                        string path = Server.MapPath("~/Images/" + fileName);
                        uploadImage.SaveAs(path);
                        productInDb.ProductImage = fileName;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // ================= 4. XÓA SẢN PHẨM =================
        // GET: AdminProduct/Delete/5
        public ActionResult Delete(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        // POST: AdminProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}