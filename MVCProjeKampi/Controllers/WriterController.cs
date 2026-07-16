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
    [AdminAuthorize(Roles = "BasAdmin,YazarYoneticisi")]
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writervalidator = new WriterValidator();
        

        public ActionResult Index()
        {
            var WriterValues = wm.GetList();
            return View(WriterValues);
        }
        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWriter(Writer p)
        {
            
            ValidationResult results = writervalidator.Validate(p);
            if (results.IsValid)
            {
                p.WriterPassword = HashHelper.CreateHash(p.WriterPassword);
                wm.WriterAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditWriter(int id)
        {
            var writervalue = wm.GetByID(id);
            return View(writervalue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWriter(Writer p, string yeniSifre)
        {
            var mevcut = wm.GetByID(p.WriterID);

            // Düzenlenebilir (düz metin) alanlar
            mevcut.WriterName = p.WriterName;
            mevcut.WriterSurname = p.WriterSurname;
            mevcut.WriterMail = p.WriterMail;
            mevcut.WriterImage = p.WriterImage;
            mevcut.WriterTitle = p.WriterTitle;
            mevcut.WriterAbout = p.WriterAbout;
            // WriterPassword ve WriterStatus dokunulmadan korunur

            // Sadece yeni şifre girildiyse hash'le ve değiştir
            if (!string.IsNullOrWhiteSpace(yeniSifre))
                mevcut.WriterPassword = HashHelper.CreateHash(yeniSifre);

            ValidationResult results = writervalidator.Validate(mevcut);
            if (results.IsValid)
            {
                wm.WriterUpdate(mevcut);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                return View(p);
            }
        }
    }
}