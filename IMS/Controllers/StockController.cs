using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMS;

namespace IMS.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();

        // GET: Stock
        public ActionResult Index()
        {
            var stock = db.Stock.Include(s => s.Product).Include(s => s.Supplier);
            return View(stock.ToList());
        }

        // GET: Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name");
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_quantity,buying_rate,selling_rate,productid,supplierid")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stock.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productid = new SelectList(db.Product, "id", "name", stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", stock.supplierid);
            return View(stock);
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", stock.supplierid);
            return View(stock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_quantity,buying_rate,selling_rate,productid,supplierid")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", stock.supplierid);
            return View(stock);
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stock.Find(id);
            db.Stock.Remove(stock);
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
