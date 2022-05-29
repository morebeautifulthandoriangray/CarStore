using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarStore.Domain.Concrete;
using CarStore.Domain.Entities;
using CarStore.Infrastructure.Abstract;
using CarStore.Models;

namespace CarStore.Controllers
{
    public class AccountController : Controller
    {
        public EFDbContext storeDB = new EFDbContext();
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        [HttpGet]
        public ViewResult Registration()
        {
            return View();
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                bool isSessionNew = Session.IsNewSession;
                ShippingDetails person = storeDB.Orders.Find(model.UserName.ToString());
                if (person != null && CheckPassword(model.Password, person.Password))
                {
                    Session["user"] = person;
                    return Redirect(returnUrl ?? Url.Action("List", "Car"));
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        private bool CheckPassword(string password, string passwordChecker)
        {
            if (password.Length != passwordChecker.Length) return false;
            for(int i = 0; i < password.Length; i++)
            {
                if (password[i] != passwordChecker[i]) return false;
            }
            return true;
        }
    }
}