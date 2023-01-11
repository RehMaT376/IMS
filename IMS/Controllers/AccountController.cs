using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMS.Models;
using System.Web.Security;

namespace IMS.Controllers
{
    public class AccountController : Controller
    {
        InventoryManagementSystemEntities db = new InventoryManagementSystemEntities();
        Inventory operations = new Inventory();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            using (var context = new InventoryManagementSystemEntities())
            {
                bool isValid = context.User.Any(x => x.username == model.username && x.password == model.password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.username, false);
                    return RedirectToAction("Index", "Stock");
                }
                ModelState.AddModelError("", "Invalid Username and Password");
                return View();
            }
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(User model)
        {
            if (ModelState.IsValid)
            {
                int a = operations.AddUser(model);
                if (a > 0)
                {
                    ViewBag.AccountCreation = "<script>alert('Account Creation Successfull')</script>";
                    ModelState.Clear();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.AccountCreation = "<script>alert('Account Creation Not Successfull')</script>";
                    return View();
                }
            }
            ModelState.AddModelError("","Please Enter Username And Password");
                return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}