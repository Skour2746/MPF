using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using mpf.Models;

namespace mpf.Controllers
{
    public class AccountController : Controller
    {
        mpfdbEntities1 dbcontext = new mpfdbEntities1();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(AdminLogin adminLogin)
        {
            if (ModelState.IsValid)
            {
                // Perform authentication logic here (e.g., check username and password against a database)
                bool isAuthenticated = YourAuthenticationLogic(adminLogin.username, adminLogin.pasword);

                if (isAuthenticated)
                {
                    // Redirect to a secured area after successful login
                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                   ViewBag.status=("Invalid username or password");
                }
            }
            return View();
        }
        private bool YourAuthenticationLogic(string username, string pasword)
        {
            // Replace this with your actual authentication logic
            return (username == "admin" && pasword == "admin");
        }
   
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }

    }
}