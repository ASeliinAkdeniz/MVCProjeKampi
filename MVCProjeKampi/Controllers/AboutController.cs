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
    [AdminAuthorize]
    public class AboutController : Controller
    {
        AboutManager abm= new AboutManager(new EfAboutDal());
        AboutValidator aboutValidator = new AboutValidator();
        public ActionResult Index()
        {
            var aboutValue = abm.GetList().FirstOrDefault();  // tek kayıt
            if (aboutValue == null)
            {
                aboutValue = new About();  // hiç yoksa boş form
            }
            return View(aboutValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(About p)
        {
            ValidationResult results = aboutValidator.Validate(p);
            if (results.IsValid)
            {
                if (p.AboutID == 0) abm.AboutAdd(p);
                else abm.AboutUpdate(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                return View(p);
            }
        }
        [HttpGet]
        public ActionResult AddAbout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAbout(About p) 
        {
            abm.AboutAdd(p);
            return RedirectToAction("Index");
        }
        public PartialViewResult AboutPartial()
        {
            return PartialView();

        }
    }
}