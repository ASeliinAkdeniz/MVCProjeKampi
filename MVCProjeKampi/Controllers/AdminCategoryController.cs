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
    [AdminAuthorize(Roles = "BasAdmin,KategoriYoneticisi")]
    public class AdminCategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        CategoryValidator categoryValidator = new CategoryValidator();

        public ActionResult Index()
        {
            var categoryvalues=cm.GetList();
            return View(categoryvalues);
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category p)
        {
            ValidationResult results = categoryValidator.Validate(p);
            if (results.IsValid)
            {
                p.CategoryStatus = true;        // yeni kategori aktif başlasın
                cm.CategoryAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                return View(p);
            }
        }

        public ActionResult DeleteCategory(int id)
        {
            var categoryvalue = cm.GetByID(id);
            categoryvalue.CategoryStatus = false;      // silme yerine pasifleştir
            cm.CategoryUpdate(categoryvalue);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var categoryvalue=cm.GetByID(id);
            return View(categoryvalue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category p)
        {
            ValidationResult results = categoryValidator.Validate(p);
            if (results.IsValid)
            {
                cm.CategoryUpdate(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                return View(p);
            }
        }
        public ActionResult ChangeCategoryStatus(int id)
        {
            var categoryvalue = cm.GetByID(id);
            categoryvalue.CategoryStatus = !categoryvalue.CategoryStatus;   // Aktif <-> Pasif
            cm.CategoryUpdate(categoryvalue);
            return RedirectToAction("Index");
        }
    }
}