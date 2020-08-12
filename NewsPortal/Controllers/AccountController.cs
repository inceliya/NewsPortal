using NewsPortal.BLL.Entities;
using NewsPortal.BLL.Repositories;
using NewsPortal.BLL.Services;
using NewsPortal.BLL.UnitOfWork;
using NewsPortal.DAL.Repositories;
using NewsPortal.ExceptionLogger;
using NewsPortal.Filters;
using NewsPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsPortal.Controllers
{
    [ExceptionLogger]
    [Culture]
    public class AccountController : Controller
    {
        private LoginService LoginService;

        public AccountController(IUnitOfWorkFactory unitOfWorkFactory, ILoginRepository loginRepository)
        {
            LoginService = new LoginService(unitOfWorkFactory, loginRepository);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(model.Password);
                byte[] hashBytes = new MD5CryptoServiceProvider().ComputeHash(passwordBytes);
                string hash = ByteArrayToString(hashBytes);
                var login = LoginService.GetUserByLogin(model.UserName);
                if (login != null && hash == login.Password)
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

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public ActionResult ChangeCulture(string language)
        {
            List<string> cultures = new List<string>() { "uk", "en", "ru" };
            if (!cultures.Contains(language))
            {
                language = "en";
            }
            return RedirectToAction("Login", new { language });
        }
    }
}