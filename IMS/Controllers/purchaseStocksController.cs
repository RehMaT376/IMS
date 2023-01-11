using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMS;
using IMS.Models;

namespace IMS.Controllers
{
    public class purchaseStocksController : Controller
    {
        private InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();

        // GET: purchaseStocks
        public ActionResult Index()
        {
            var purchaseStock = db.purchaseStock.Include(p => p.Product).Include(p => p.Supplier);
            return View(purchaseStock.ToList());
        }

        // GET: purchaseStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseStock purchaseStock = db.purchaseStock.Find(id);
            if (purchaseStock == null)
            {
                return HttpNotFound();
            }
            return View(purchaseStock);
        }

        // GET: purchaseStocks/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name");
            return View();
        }

        // POST: purchaseStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_quantity,buying_date,buying_rate,productid,supplierid,selling_rate")] purchaseStock purchaseStock)
        {
            if (ModelState.IsValid)
            {
                if(purchaseStock.product_quantity>0)
                {
                    db.purchaseStock.Add(purchaseStock);
                    db.SaveChanges();
                    TempData["pid"] = purchaseStock.productid;
                    TempData["sid"] = purchaseStock.supplierid;
                    int pid = (int)purchaseStock.productid;
                    int sid = (int)purchaseStock.supplierid;
                    Inventory inv = new Inventory();
                    inv.AddProduct(pid, sid, purchaseStock.product_quantity, purchaseStock.buying_rate, purchaseStock.selling_rate);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("QuantityError");
                }
            }

            ViewBag.productid = new SelectList(db.Product, "id", "name", purchaseStock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchaseStock.supplierid);
            return View(purchaseStock);
        }

        // GET: purchaseStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseStock purchaseStock = db.purchaseStock.Find(id);
            if (purchaseStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", purchaseStock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchaseStock.supplierid);
            return View(purchaseStock);
        }

        // POST: purchaseStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_quantity,buying_date,buying_rate,productid,supplierid,selling_rate")] purchaseStock purchaseStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", purchaseStock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchaseStock.supplierid);
            return View(purchaseStock);
        }

        // GET: purchaseStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseStock purchaseStock = db.purchaseStock.Find(id);
            if (purchaseStock == null)
            {
                return HttpNotFound();
            }
            return View(purchaseStock);
        }

        // POST: purchaseStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchaseStock purchaseStock = db.purchaseStock.Find(id);
            db.purchaseStock.Remove(purchaseStock);
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

        public ActionResult QuantityError()
        {
            return View();
        }
    }
}
