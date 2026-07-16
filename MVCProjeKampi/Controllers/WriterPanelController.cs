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
using PagedList;
using PagedList.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class WriterPanelController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingValidator headingValidator = new HeadingValidator();
        [Authorize(Roles = "Writer")]
        public ActionResult WriterProfile()
        {
            string mail = (string)Session["WriterMail"];
            var writervalue = wm.GetByMail(mail);
            if (writervalue == null) return RedirectToAction("WriterLogin", "Login");
            ViewBag.a = writervalue.WriterID;
            return View(writervalue);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WriterProfile(Writer p, string yeniSifre)
        {
            // Yeni şifre girildiyse hash'le; girilmediyse gizli alandan gelen eski hash korunur
            if (!string.IsNullOrWhiteSpace(yeniSifre))
            {
                p.WriterPassword = HashHelper.CreateHash(yeniSifre);
            }
            // yeniSifre boşsa: p.WriterPassword zaten formdaki gizli alandan (eski hash) geldi

            wm.WriterUpdate(p);
            return RedirectToAction("WriterProfile");
        }
        public ActionResult MyHeading()
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);
            if (writer == null) return RedirectToAction("WriterLogin", "Login");

            var values = hm.GetListByWriterActive(writer.WriterID);
            return View(values);
        }
        [HttpGet]
        public ActionResult NewHeading()
        {
            KategorileriDoldur();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewHeading(Heading p)
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);
            if (writer == null) return RedirectToAction("WriterLogin", "Login");

            p.WriterID = writer.WriterID;
            p.HeadingDate = DateTime.Now.ToShortDateString();
            p.HeadingStatus = true;

            ValidationResult results = headingValidator.Validate(p);
            if (results.IsValid)
            {
                hm.HeadingAdd(p);
                return RedirectToAction("MyHeading");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                KategorileriDoldur();       // ŞART
                return View(p);
            }
        }
        public ActionResult EditHeading(int id)
        {
            KategorileriDoldur();
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
                return RedirectToAction("MyHeading");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                KategorileriDoldur();
                return View(p);
            }
        }
        public ActionResult DeleteHeading(int id)
        {
            var HeadingValue = hm.GetByID(id);
            HeadingValue.HeadingStatus = false;
            hm.HeadingDelete(HeadingValue);
            return RedirectToAction("MyHeading");
        }
        public ActionResult AllHeading(int p=1)
        {
            var headings = hm.GetList().ToPagedList(p, 4);
            return View(headings);
        }
        [AllowAnonymous]
        public ActionResult WriterRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult WriterRegister(Writer p, string WriterPasswordAgain)
        {
            // Basit kontrol
            if (string.IsNullOrWhiteSpace(p.WriterMail) || string.IsNullOrWhiteSpace(p.WriterPassword))
            {
                ViewBag.Hata = "Mail ve şifre zorunludur.";
                return View();
            }
            if (p.WriterPassword != WriterPasswordAgain)
            {
                ViewBag.Hata = "Şifreler eşleşmiyor.";
                return View();
            }
            // Mail zaten kayıtlı mı?
            var mevcut = wm.GetList().FirstOrDefault(x => x.WriterMail == p.WriterMail);
            if (mevcut != null)
            {
                ViewBag.Hata = "Bu mail adresi zaten kayıtlı.";
                return View();
            }

            p.WriterPassword = HashHelper.CreateHash(p.WriterPassword); // şifre hash
            p.WriterStatus = true;   // hemen aktif
            if (string.IsNullOrEmpty(p.WriterImage))
                p.WriterImage = "/images/default-avatar.png"; // varsayılan (opsiyonel)
            if (string.IsNullOrEmpty(p.WriterTitle)) p.WriterTitle = "Yazar";
            if (string.IsNullOrEmpty(p.WriterAbout)) p.WriterAbout = "";

            wm.WriterAdd(p);
            return RedirectToAction("WriterLogin", "Login");
        }
        private void KategorileriDoldur()
        {
            ViewBag.vlc = (from x in cm.GetActiveList()      // cm = CategoryManager
                           select new SelectListItem
                           {
                               Text = x.CategoryName,
                               Value = x.CategoryID.ToString()
                           }).ToList();
        }

    }
}