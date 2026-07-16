using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers
{
    public class WriterPanelContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentValidator contentValidator = new ContentValidator();

        public ActionResult MyContent()
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);
            if (writer == null) return RedirectToAction("WriterLogin", "Login");

            var values = cm.GetListByWriter(writer.WriterID);
            // Başlık adları için
            var headingNames = hm.GetList().ToDictionary(h => h.HeadingID, h => h.HeadingName);
            ViewBag.HeadingNames = headingNames;
            return View(values);
        }

        // Entry sil (sadece kendi entry'sini)
        public ActionResult DeleteContent(int id)
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);
            var content = cm.GetByID(id);

            // Güvenlik: sadece kendi entry'sini silebilsin
            if (content != null && writer != null && content.WriterID == writer.WriterID)
            {
                cm.ContentDelete(content);
            }
            return RedirectToAction("MyContent");
        }

        [HttpGet]
        public ActionResult AddContent(int id)
        {
            ViewBag.d = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContent(Content p)
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);              // Context yerine manager
            if (writer == null) return RedirectToAction("WriterLogin", "Login");

            p.ContentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.WriterID = writer.WriterID;
            p.ContentStatus = true;

            ValidationResult results = contentValidator.Validate(p);
            if (results.IsValid)
            {
                cm.ContentAdd(p);
                return RedirectToAction("MyContent");
            }
            else
            {
                foreach (var item in results.Errors)
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                ViewBag.d = p.HeadingID;
                return View(p);
            }
        }
        public ActionResult ToDoList()
        {
            return View();
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContent(int id, string contentValue)
        {
            string mail = (string)Session["WriterMail"];
            var writer = wm.GetByMail(mail);
            var content = cm.GetByID(id);

            if (content != null && writer != null && content.WriterID == writer.WriterID)
            {
                content.ContentValue = contentValue;

                ValidationResult results = contentValidator.Validate(content);
                if (results.IsValid)
                {
                    cm.ContentUpdate(content);
                }
                else
                {
                    TempData["EntryHata"] = results.Errors.First().ErrorMessage;
                }
            }
            return RedirectToAction("MyContent");
        }
    }
}