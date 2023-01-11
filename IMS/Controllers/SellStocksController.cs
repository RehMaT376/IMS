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
    public class SellStocksController : Controller
    {
        private InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();
        // GET: SellStocks
        public ActionResult Index()
        {
            var sellStock = db.SellStock.Include(s => s.Product).Include(s => s.Product1).Include(s => s.Supplier);
            return View(sellStock.ToList());
        }

        // GET: SellStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellStock sellStock = db.SellStock.Find(id);
            if (sellStock == null)
            {
                return HttpNotFound();
            }
            return View(sellStock);
        }

        // GET: SellStocks/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Product, "id", "name");
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name");
            return View();
        }

        // POST: SellStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_quantity,selling_date,selling_rate,productid,supplierid,buying_rate")] SellStock sellStock)
        {
            if (ModelState.IsValid)
            {
                int pid = (int)sellStock.productid;
                int sid = (int)sellStock.supplierid;
                Inventory obj = new Inventory();
                int stockid = obj.CheckProduct(pid, sid);
                if (stockid!=0)
                {
                    int remQuantity = obj.CheckInventory(pid, sid);
                    if (remQuantity > sellStock.product_quantity)
                    {

                        db.SellStock.Add(sellStock);
                        db.SaveChanges();
                        obj.SellProduct(pid, sid, sellStock.product_quantity);
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

            ViewBag.productid = new SelectList(db.Product, "id", "name", sellStock.productid);
            ViewBag.supplierid = new SelectList(db.Product, "id", "name", sellStock.supplierid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sellStock.supplierid);
            return View(sellStock);
        }

        // GET: SellStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellStock sellStock = db.SellStock.Find(id);
            if (sellStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", sellStock.productid);
            ViewBag.supplierid = new SelectList(db.Product, "id", "name", sellStock.supplierid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sellStock.supplierid);
            return View(sellStock);
        }

        // POST: SellStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_quantity,selling_date,selling_rate,productid,supplierid,buying_rate")] SellStock sellStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.Product, "id", "name", sellStock.productid);
            ViewBag.supplierid = new SelectList(db.Product, "id", "name", sellStock.supplierid);
            ViewBag.supplierid = new SelectList(db.Supplier, "id", "name", sellStock.supplierid);
            return View(sellStock);
        }

        // GET: SellStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellStock sellStock = db.SellStock.Find(id);
            if (sellStock == null)
            {
                return HttpNotFound();
            }
            return View(sellStock);
        }

        // POST: SellStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SellStock sellStock = db.SellStock.Find(id);
            db.SellStock.Remove(sellStock);
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
