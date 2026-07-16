using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers
{
    [AllowAnonymous] 
    public class LoginController : Controller
    {
        WriterLoginManager wm = new WriterLoginManager(new EfWriterDal());
        AdminManager am = new AdminManager(new EfAdminDal());
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin p)
        {
            var adminuserinfo = am.GetAdmin(p.AdminUserName, p.AdminPassword);

            if (adminuserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(adminuserinfo.AdminUserName, false);
                Session["AdminUserName"] = adminuserinfo.AdminUserName;
                return RedirectToAction("Index", "Panel");
            }
            else
            {
                ViewBag.ErrorMessage = "Hatalı kullanıcı adı veya şifre girdiniz!";
                return View();
            }
        }
        [HttpGet]
        public ActionResult WriterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WriterLogin(Writer p)
        {
            var writeruserinfo = wm.GetWriter(p.WriterMail, p.WriterPassword);

            if (writeruserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(writeruserinfo.WriterMail, false);
                Session["WriterMail"] = writeruserinfo.WriterMail;
                return RedirectToAction("HomePage", "Default");
            }
            else
            {
                ViewBag.ErrorMessage = "Hatalı kullanıcı adı veya şifre girdiniz!";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("HomePage", "Default");
        }
    }
}