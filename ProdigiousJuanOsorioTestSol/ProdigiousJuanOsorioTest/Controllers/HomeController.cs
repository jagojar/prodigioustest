using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProdigiousJuanOsorioTest.Models;
using ProdigiousJuanOsorioTest.Source.BusinessLayer;
using System.Web.Security;
using System.Security.Principal;
using System.Threading;
using System.Security.Claims;

namespace ProdigiousJuanOsorioTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }

            return Redirect("Shop");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel lgm)
        {
            if (ModelState.IsValid)
            {
                EmployeeManager em = new EmployeeManager();
                var employee = em.GetEmployee(lgm.Email, lgm.Password);

                if (employee != null)
                {
                    FormsAuthentication.SetAuthCookie(employee.Email, false);                    
                    IPrincipal principal = new ProdigiousPrincipal(employee.Email, new[] { employee.Role });
                    Thread.CurrentPrincipal = principal;

                    if (System.Web.HttpContext.Current != null)
                    {
                        System.Web.HttpContext.Current.User = principal;
                    }                    

                    return Redirect("../Shop");
                }

                ViewBag.Message = "Email or Password are incorrect!!";
            }

            return View("Index");
        }

        
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            IPrincipal principal = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            HttpContext.User = principal;
            Thread.CurrentPrincipal = principal;
            return RedirectToAction("Index", "Home");
        }
    }
}