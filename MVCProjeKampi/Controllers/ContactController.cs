using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Filters;
using PagedList;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize(Roles = "BasAdmin,DestekYoneticisi")]
    public class ContactController : Controller
    {
        // GET: Contact
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator cv = new ContactValidator();
        
        public ActionResult Index(int page = 1)
        {
            var contactvalues = cm.GetList().ToPagedList(page, 10);
            return View(contactvalues);
        }
        public ActionResult GetContactDetails(int id)
        {
            var contactvalues = cm.GetByID(id);
            return View(contactvalues);
        }
        public PartialViewResult MessageListMenu()
        {
            return PartialView();
        }
    }
}