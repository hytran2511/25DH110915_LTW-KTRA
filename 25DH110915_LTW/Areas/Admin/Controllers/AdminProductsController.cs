using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _25DH110915_LTW.Models;

namespace _25DH110915_LTW.Areas.Admin.Controllers
{
    public class AdminProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/AdminProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).OrderByDescending(p => p.ProductID);
            return View(products.ToList());
        }

        // GET: Admin/AdminProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products
                                .Include(p => p.Category)
                                .Include(p => p.OrderDetails)
                                .SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(new Product { Quantity = 10 });
        }

        // POST: Admin/AdminProducts/Create (Hỗ trợ Upload ảnh sản phẩm + kiểm tra DataAnnotation)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage,Quantity")] Product product, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadImage.FileName);
                string imagesFolder = Server.MapPath("~/Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                string filePath = Path.Combine(imagesFolder, fileName);
                uploadImage.SaveAs(filePath);
                product.ProductImage = fileName;
            }
            else if (string.IsNullOrWhiteSpace(product.ProductImage))
            {
                product.ProductImage = "tam-ly.jpg";
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã thêm hàng hóa mới thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5 (Hỗ trợ Upload ảnh sản phẩm + kiểm tra DataAnnotation)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage,Quantity")] Product product, HttpPostedFileBase uploadImage)
        {
            var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductID == product.ProductID);

            if (uploadImage != null && uploadImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadImage.FileName);
                string imagesFolder = Server.MapPath("~/Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                string filePath = Path.Combine(imagesFolder, fileName);
                uploadImage.SaveAs(filePath);
                product.ProductImage = fileName;
            }
            else if (string.IsNullOrWhiteSpace(product.ProductImage) && existingProduct != null)
            {
                product.ProductImage = existingProduct.ProductImage;
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Đã cập nhật hàng hóa thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5 (Kiểm tra ràng buộc dữ liệu trước khi xóa)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var orderDetailsWithProduct = db.OrderDetails.Where(od => od.ProductID == id).ToList();
            if (orderDetailsWithProduct.Count > 0)
            {
                TempData["ErrorMessage"] = "Không thể xóa Sản phẩm '" + product.ProductName + "' vì đã phát sinh giao dịch trong " + orderDetailsWithProduct.Count + " chi tiết đơn hàng!";
                return RedirectToAction("Delete", new { id = product.ProductID });
            }

            db.Products.Remove(product);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Đã xóa Sản phẩm thành công.";
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
