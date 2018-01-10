using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using e_commerce.DAL;
using e_commerce.Models;
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace e_commerce.Controllers
{
    public class ProductImagesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: ProductImages
        public ActionResult Index()
        {
            return View(db.ProductImages.ToList());
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool AllFilesValid = true;
            var ErrorFIles = new StringBuilder();

            if(files.Length!= 0)
            {
                if (files.Length<= 10)
                {
                    foreach (var item in files)
                    {
                        if (ValidateFile(item))
                        {
                            try
                            {
                                SaveFileToDisk(item);
                            }
                            catch (Exception)
                            {
                                AllFilesValid = false;
                                ModelState.AddModelError("FileName", "cannot save to disk");
                            }
                        }
                        else
                        {
                            AllFilesValid = false;
                            ErrorFIles.AppendFormat(", {0}", item.FileName);
                        }  
                    }
                }
                else
                {
                    ModelState.AddModelError("FileName", "the amount of files cannot be more than 10");
                }
            }
            else
            {
                ModelState.AddModelError("FileName", "Please choose a file");
            }

            if (ModelState.IsValid)
            {
                db.ProductImages.Add(new ProductImage { FileName = file.FileName });
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    SqlException inner = ex.InnerException.InnerException as SqlException;
                    if(inner != null && inner.Number== 2601)
                    {
                        ModelState.AddModelError("FileName", "this file already exist");
                    }
                    else
                    {
                        ModelState.AddModelError("FileName", "save error, try again");
                    }
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();
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

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string FileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypd = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) &&
                allowedFileTypd.Contains(FileExtension))
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            var img = new WebImage(file.InputStream);
            if (img.Width > 190)
                img.Resize(190, img.Height);
            img.Save(Constants.ProuctImagePath + file.FileName);
            if (img.Width > 100)
                img.Resize(100, img.Height);
            img.Save(Constants.ProuctThumbnailPath + file.FileName);
        }
    }
}
