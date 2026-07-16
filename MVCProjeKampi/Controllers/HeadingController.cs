using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using PagedList;
using MVCProjeKampi.Filters;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize(Roles = "BasAdmin,Moderator")]
    public class HeadingController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager catm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingValidator headingValidator = new HeadingValidator();

        // Dropdown'ları dolduran yardımcı metot (tekrar yazmamak için)
        private void DropdownlariDoldur()
        {
            ViewBag.vlc = (from x in catm.GetActiveList()
                           select new SelectListItem
                           {
                               Text = x.CategoryName,
                               Value = x.CategoryID.ToString()
                           }).ToList();

            ViewBag.vlw = (from y in wm.GetList()
                           select new SelectListItem
                           {
                               Text = y.WriterName + " " + y.WriterSurname,
                               Value = y.WriterID.ToString()
                           }).ToList();
        }

        public ActionResult Index(int page = 1)
        {
            var headingValues = hm.GetList().ToPagedList(page, 10);
            return View(headingValues);
        }

        public ActionResult HeadingReport()
        {
            var headingValues = hm.GetList();
            return View(headingValues);
        }

        [HttpGet]
        public ActionResult AddHeading()
        {
            DropdownlariDoldur();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHeading(Heading p)
        {
            ValidationResult results = headingValidator.Validate(p);
            if (results.IsValid)
            {
                p.HeadingDate = DateTime.Now.ToShortDateString();
                p.HeadingStatus = true;
                hm.HeadingAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                DropdownlariDoldur();   // ŞART: yoksa sayfa patlar
                return View(p);
            }
        }

        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            DropdownlariDoldur();
            var HeadingValue = hm.GetByID(id);
            return View(HeadingValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHeading(Heading p)
        {
            ValidationResult results = headingValidator.Validate(p);
            if (results.IsValid)
            {
                hm.HeadingUpdate(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                DropdownlariDoldur();   // ŞART
                return View(p);
            }
        }

        public ActionResult DeleteHeading(int id)
        {
            var HeadingValue = hm.GetByID(id);
            HeadingValue.HeadingStatus = false;
            hm.HeadingDelete(HeadingValue);
            return RedirectToAction("Index");
        }
    }
}