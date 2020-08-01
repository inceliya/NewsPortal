using NewsPortal.ExceptionLogger;
using NewsPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ExceptionLogger]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ExceptionLogger]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (FormsAuthentication.Authenticate(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}