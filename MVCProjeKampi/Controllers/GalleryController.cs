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
    public class GalleryController : Controller
    {
        ImageFileManager ifm = new ImageFileManager(new EfImageFileDal());
        ImageFileValidator imageValidator = new ImageFileValidator();
        // GET: Gallery
        public ActionResult Index()
        {
            var files = ifm.GetList();
            return View(files);
        }
        [HttpGet]
        public ActionResult AddImage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage(ImageFile p)
        {
            ValidationResult results = imageValidator.Validate(p);
            if (results.IsValid)
            {
                ifm.TAdd(p);                    // kendi metot adın
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                return View(p);
            }
        }

        public ActionResult DeleteImage(int id)
        {
            var value = ifm.GetByID(id);
            ifm.TDelete(value);   // silme metodu (adı farklıysa uyarla)
            return RedirectToAction("Index");
        }
    }
}