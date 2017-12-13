using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using e_commerce.DAL;
using e_commerce.Models;
using e_commerce.Models.ViewModel;
using PagedList;

namespace e_commerce.Controllers
{
    public class ProductsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        //public ActionResult Index()
        //{
        //    var products = db.Products.Include(p => p.Category);
        //    return View(products.ToList());
        //}

        public ActionResult Index(string category,string search, string sortBy, int? page)
        {
            ProductIndexViewModel ViewModel = new ProductIndexViewModel();

            var Products = db.Products.Include(p => p.Category);
            
            if(!string.IsNullOrEmpty(search))
            {
                Products = Products.Where(x => x.Name.Contains(search));
                ViewModel.Search = search;
            }

            ViewModel.CatWithCount = from x in Products
                                     where x.CategoryID != null
                                     group x by x.Category.Name into gp
                                     select new CategoryWithCount
                                     {
                                         CategoryName = gp.Key,
                                         ProductCount = gp.Count()
                                     };

            if (!string.IsNullOrEmpty(category))
            {
                Products = Products.Where(x => x.Category.Name == category);
                ViewModel.Category = category;
            }

            switch (sortBy)
            {
                case "lowest":
                    Products = Products.OrderBy(x => x.Price);
                    break;
                case "highest":
                    Products = Products.OrderByDescending(x => x.Price);
                    break;
                default:
                    Products = Products.OrderBy(x => x.Name);
                    break;
            }

            const int PageItems = 3;
            int CurrentPage = (page ?? 1);
            ViewModel.Products = Products.ToPagedList(CurrentPage, PageItems);
            ViewModel.SortBy = sortBy;
            ViewModel.Sort = new Dictionary<string, string>
            {
                {"Price low to hight", "lowest" },
                {"Price high to low", "highest" }
            };
            return View(ViewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Price,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
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
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Price,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
    }
}
