using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using MVCProjeKampi.Filters;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize(Roles = "BasAdmin")]
    public class AuthorizationController : Controller
    {
        AdminManager adminmanager = new AdminManager(new EfAdminDal());
        AdminValidator adminValidator = new AdminValidator();
        public ActionResult Index()
        {
            var adminvalues = adminmanager.GetList();
            return View(adminvalues);
        }
        // Rol listesi (dropdown için)
        private List<SelectListItem> RolListesi()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Text = "Baş Admin", Value = "BasAdmin" },
        new SelectListItem { Text = "Moderatör", Value = "Moderator" },
        new SelectListItem { Text = "Kategori Yöneticisi", Value = "KategoriYoneticisi" },
        new SelectListItem { Text = "Yazar Yöneticisi", Value = "YazarYoneticisi" },
        new SelectListItem { Text = "Destek Yöneticisi", Value = "DestekYoneticisi" },
        new SelectListItem { Text = "İçerik Editörü", Value = "IcerikEditoru" }
    };
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            ViewBag.roller = RolListesi();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(Admin p)
        {
            ValidationResult results = adminValidator.Validate(p);
            if (results.IsValid)
            {
                p.AdminPassword = HashHelper.CreateHash(p.AdminPassword);
                adminmanager.AdminAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                ViewBag.roller = RolListesi();      // ŞART: dropdown
                return View(p);
            }
        }

        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            ViewBag.roller = RolListesi();
            var value = adminmanager.GetByID(id);
            return View(value);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(Admin p, string yeniSifre)
        {
            var mevcut = adminmanager.GetByID(p.AdminId);
            mevcut.AdminUserName = p.AdminUserName;
            mevcut.AdminRole = p.AdminRole;
            if (!string.IsNullOrWhiteSpace(yeniSifre))
                mevcut.AdminPassword = HashHelper.CreateHash(yeniSifre);

            ValidationResult results = adminValidator.Validate(mevcut);
            if (results.IsValid)
            {
                adminmanager.AdminUpdate(mevcut);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                ViewBag.roller = RolListesi();
                return View(p);
            }
        }
        public ActionResult ChangeStatus(int id)
        {
            var admin = adminmanager.GetByID(id);
            if (admin != null)
            {
                admin.AdminStatus = !admin.AdminStatus;   // Aktif <-> Pasif
                adminmanager.AdminUpdate(admin);
            }
            return RedirectToAction("Index");
        }

    }
}