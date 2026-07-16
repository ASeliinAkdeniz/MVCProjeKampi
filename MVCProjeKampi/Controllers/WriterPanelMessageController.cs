using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers
{
    public class WriterPanelMessageController : Controller
    {
        MessageManager mm = new MessageManager(new EFMessageDal());
        MessageValidator writervalidator = new MessageValidator();
        WriterManager wm = new WriterManager(new EfWriterDal());
        [Authorize(Roles = "Writer")]
        public ActionResult Inbox()
        {
            string mail = (string)Session["WriterMail"];
            var messagelist = mm.GetListInbox(mail);

            // Mail -> profil foto eşlemesi
            ViewBag.WriterImages = wm.GetList()
                .Where(w => w.WriterImage != null)
                .ToDictionary(w => w.WriterMail, w => w.WriterImage);

            return View(messagelist);
        }
        public PartialViewResult MessageListMenu()
        {
            string mail = (string)Session["WriterMail"];
            ViewBag.OkunmamisSayisi = mm.GetInboxUnreadCount(mail);
            return PartialView();
        }
        public ActionResult Sendbox()
        {
            string mail = (string)Session["WriterMail"];
            var values = mm.GetListSendbox(mail);
            ViewBag.WriterImages = wm.GetList().Where(w => w.WriterImage != null)
                                     .ToDictionary(w => w.WriterMail, w => w.WriterImage);
            return View(values);
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            if (values != null && values.MessageStatus == false)
            {
                values.MessageStatus = true;
                mm.MessageUpdate(values);
            }
            ViewBag.WriterImages = wm.GetList().Where(w => w.WriterImage != null)
                                     .ToDictionary(w => w.WriterMail, w => w.WriterImage);
            return View(values);
        }

        public ActionResult GetSendboxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            ViewBag.WriterImages = wm.GetList().Where(w => w.WriterImage != null)
                                     .ToDictionary(w => w.WriterMail, w => w.WriterImage);
            return View(values);
        }
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }
        [HttpPost] // Sayfadan form gönderildiğinde çalışması için Post özniteliğini ekledik
        [ValidateAntiForgeryToken]
        public ActionResult NewMessage(EntityLayer.Concrete.Message p)
        {

            MessageValidator messageValidator = new MessageValidator();
            FluentValidation.Results.ValidationResult results = messageValidator.Validate(p);

            if (results.IsValid)
            {
                // 1. Statik "gizem@gmail.com" yerine Session'dan dinamik mail adresini alıyoruz
                string senderMailInfo = (string)Session["WriterMail"];

                // 2. Aldığımız bu mail bilgisini gönderen olarak mesaja atıyoruz
                p.SenderMail = senderMailInfo;

                p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
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
        // Çöpe at (soft delete) — from: hangi kutudan geldiyse oraya dön
        public ActionResult MoveToTrash(int id, string from = "Inbox")
        {
            var message = mm.GetByID(id);
            message.MessageDeleted = true;
            mm.MessageUpdate(message);
            return RedirectToAction(from);
        }

        // Çöp kutusu — çöpe atılmış mesajlar
        public ActionResult Trash()
        {
            string mail = (string)Session["WriterMail"];
            var values = mm.GetListTrash(mail);
            return View(values);
        }

        // Çöpten geri yükle
        public ActionResult RestoreMessage(int id)
        {
            var message = mm.GetByID(id);
            message.MessageDeleted = false;
            mm.MessageUpdate(message);
            return RedirectToAction("Trash");
        }

        // Kalıcı sil (gerçek delete) — onay view tarafında JS ile alınacak
        public ActionResult DeletePermanent(int id)
        {
            var message = mm.GetByID(id);
            mm.MessageDelete(message);
            return RedirectToAction("Trash");
        }
    }
}