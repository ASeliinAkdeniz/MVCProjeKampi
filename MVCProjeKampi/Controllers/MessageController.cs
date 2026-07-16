using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using FluentValidation.Results;
using MVCProjeKampi.Filters;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize]
    public class MessageController : Controller
    {
        MessageManager mm = new MessageManager(new EFMessageDal());
        MessageValidator writervalidator = new MessageValidator();
     
        public ActionResult Inbox()
        {
            string adminMail = User.Identity.Name;          // giriş yapmış admin
            var messagelist = mm.GetListInbox(adminMail);
            return View(messagelist);
        }

        public ActionResult Sendbox()
        {
            string adminMail = User.Identity.Name;
            var values = mm.GetListSendbox(adminMail);
            return View(values);
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            return View(values);
        }
        public ActionResult GetSendboxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            return View(values);
        }
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMessage(EntityLayer.Concrete.Message p) // Tür çakışmasını engellemek için tam yoluyla yazdık
        {
            // Kendi yazdığınız validator sınıfını örneklendiriyorsunuz
            MessageValidator messageValidator = new MessageValidator();

            // Doğru ValidationResult (FluentValidation'a ait olan) tetikleniyor
            FluentValidation.Results.ValidationResult results = messageValidator.Validate(p);

            if (results.IsValid)
            {
                p.MessageDate=DateTime.Parse( DateTime.Now.ToShortDateString());
                mm.MessageAdd(p);
                return RedirectToAction("Sendbox");
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
        // Çöpe at (soft delete)
        public ActionResult MoveToTrash(int id, string from = "Inbox")
        {
            var message = mm.GetByID(id);
            message.MessageDeleted = true;
            mm.MessageUpdate(message);
            return RedirectToAction(from);
        }

        // Çöp kutusu
        public ActionResult Trash()
        {
            string mail = (string)Session["AdminUserName"];
            var values = mm.GetListTrash(mail);
            return View(values);
        }

        // Geri yükle
        public ActionResult RestoreMessage(int id)
        {
            var message = mm.GetByID(id);
            message.MessageDeleted = false;
            mm.MessageUpdate(message);
            return RedirectToAction("Trash");
        }

        // Kalıcı sil
        public ActionResult DeletePermanent(int id)
        {
            var message = mm.GetByID(id);
            mm.MessageDelete(message);
            return RedirectToAction("Trash");
        }
    }
}