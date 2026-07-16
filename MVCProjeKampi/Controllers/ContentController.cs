using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Filters;
using PagedList;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize(Roles = "BasAdmin,Moderator,IcerikEditoru")]
    public class ContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult Index()
        {
            return RedirectToAction("GetAllContent");
        }

        public ActionResult GetAllContent(string p, int page = 1)
        {
            var values = cm.GetList(p ?? "");

            // Başlık ve yazar adları
            ViewBag.HeadingNames = hm.GetList().ToDictionary(h => h.HeadingID, h => h.HeadingName);
            ViewBag.WriterNames = wm.GetList().ToDictionary(w => w.WriterID, w => w.WriterName + " " + w.WriterSurname);
            ViewBag.Arama = p;

            var sirali = values.OrderByDescending(x => x.ContentDate).ToPagedList(page, 10);
            return View(sirali);
        }

        // Entry sil (soft-delete)
        public ActionResult DeleteContent(int id)
        {
            var value = cm.GetByID(id);
            if (value != null)
            {
                value.ContentStatus = false;   // pasif yap, veriyi kaybetme
                cm.ContentUpdate(value);
            }
            return RedirectToAction("GetAllContent");
        }

        public ActionResult ContentByHeading(int id)
        {
            var contentvalues = cm.GetListByHeadingID(id);
            return View(contentvalues);
        }
    }
}