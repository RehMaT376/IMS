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
    public class Sell_StockController : Controller
    {
        private InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();

        // GET: Sell_Stock
        public ActionResult Index()
        {
            var sell_Stock = db.Sell_Stock.Include(s => s.Product).Include(s => s.Supplier);
            return View(sell_Stock.ToList());
        }

        // GET: Sell_Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell_Stock sell_Stock = db.Sell_Stock.Find(id);
            if (sell_Stock == null)
            {
                return HttpNotFound();
            }
            return View(sell_Stock);
        }

        // GET: Sell_Stock/Create
        public ActionResult Create(int pid,int sid,int buyrate,int sellrate)
        {
            ViewBag.productid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name");

            ViewBag.pid = pid;
            ViewBag.sid = sid;
            ViewBag.buyrate = buyrate;
            ViewBag.sellrate = sellrate;

            return View();
        }

        // POST: Sell_Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_quantity,buying_rate,selling_rate,productid,supplierid")] Sell_Stock sell_Stock)
        {
            if (ModelState.IsValid)
            {
                int pid = (int)sell_Stock.productid;
                int sid = (int)sell_Stock.supplierid;
                Inventory obj = new Inventory();
                int stockid = obj.CheckProduct(pid, sid);
                if (stockid != 0)
                {
                    int remQuantity = obj.CheckInventory(pid, sid);
                    if (remQuantity >= sell_Stock.product_quantity)
                    {

                        db.Sell_Stock.Add(sell_Stock);
                        db.SaveChanges();
                        obj.SellProduct(pid, sid, sell_Stock.product_quantity);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("OutOfStock");
                    }
                }
                else
                {
                    return RedirectToAction("ProductNotForSale");
                }
            }

            ViewBag.productid = new SelectList(db.Product, "id", "name", sell_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sell_Stock.supplierid);
            return View(sell_Stock);
        }

        // GET: Sell_Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell_Stock sell_Stock = db.Sell_Stock.Find(id);
            if (sell_Stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", sell_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sell_Stock.supplierid);
            return View(sell_Stock);
        }

        // POST: Sell_Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_quantity,selling_date,buying_rate,selling_rate,productid,supplierid")] Sell_Stock sell_Stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sell_Stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", sell_Stock.productid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sell_Stock.supplierid);
            return View(sell_Stock);
        }

        // GET: Sell_Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell_Stock sell_Stock = db.Sell_Stock.Find(id);
            if (sell_Stock == null)
            {
                return HttpNotFound();
            }
            return View(sell_Stock);
        }

        // POST: Sell_Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sell_Stock sell_Stock = db.Sell_Stock.Find(id);
            db.Sell_Stock.Remove(sell_Stock);
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

        public ActionResult OutOfStock()
        {
            return View();
        }
        public ActionResult ProductNotForSale()
        {
            return View();
        }
    }
}
