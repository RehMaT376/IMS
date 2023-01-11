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
    [Authorize]
    public class purchase_StockController : Controller
    {
        private InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();

        // GET: purchase_Stock
        public ActionResult Index()
        {
            var purchase_Stock = db.purchase_Stock.Include(p => p.Product).Include(p => p.Supplier);
            return View(purchase_Stock.ToList());
        }

        // GET: purchase_Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase_Stock purchase_Stock = db.purchase_Stock.Find(id);
            if (purchase_Stock == null)
            {
                return HttpNotFound();
            }
            return View(purchase_Stock);
        }

        // GET: purchase_Stock/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name");
            return View();
        }

        // POST: purchase_Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_quantity,buying_rate,selling_rate,productid,supplierid")] purchase_Stock purchase_Stock)
        {
            if (ModelState.IsValid)
            {
                if (purchase_Stock.product_quantity > 0)
                {
                    db.purchase_Stock.Add(purchase_Stock);
                    db.SaveChanges();
                    TempData["pid"] = purchase_Stock.productid;
                    TempData["sid"] = purchase_Stock.supplierid;
                    int pid = (int)purchase_Stock.productid;
                    int sid = (int)purchase_Stock.supplierid;
                    Inventory inv = new Inventory();
                    inv.AddProduct(pid, sid, purchase_Stock.product_quantity, purchase_Stock.buying_rate, purchase_Stock.selling_rate);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("QuantityError");
                }
            }

            ViewBag.productid = new SelectList(db.Product, "id", "name", purchase_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchase_Stock.supplierid);
            return View(purchase_Stock);
        }

        // GET: purchase_Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase_Stock purchase_Stock = db.purchase_Stock.Find(id);
            if (purchase_Stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", purchase_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchase_Stock.supplierid);
            return View(purchase_Stock);
        }

        // POST: purchase_Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_quantity,buying_date,buying_rate,selling_rate,productid,supplierid")] purchase_Stock purchase_Stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase_Stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", purchase_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", purchase_Stock.supplierid);
            return View(purchase_Stock);
        }

        // GET: purchase_Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase_Stock purchase_Stock = db.purchase_Stock.Find(id);
            if (purchase_Stock == null)
            {
                return HttpNotFound();
            }
            return View(purchase_Stock);
        }

        // POST: purchase_Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchase_Stock purchase_Stock = db.purchase_Stock.Find(id);
            db.purchase_Stock.Remove(purchase_Stock);
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
