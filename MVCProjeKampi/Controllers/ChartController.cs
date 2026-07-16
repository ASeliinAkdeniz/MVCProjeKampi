using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Filters;
using MVCProjeKampi.Models;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize]
    public class ChartController : Controller
    {
        CategoryManager catm = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager cm = new ContentManager(new EfContentDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult Index()
        {
            return View();
        }

        // 1) Kategorilere göre başlık sayısı
        public ActionResult CategoryChart()
        {
            var kategoriler = catm.GetActiveList();
            var basliklar = hm.GetActiveList();
            var liste = kategoriler.Select(c => new CategoryClass
            {
                CategoryName = c.CategoryName,
                CategoryCount = basliklar.Count(h => h.CategoryID == c.CategoryID)
            }).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        // 2) Kategorilere göre entry sayısı
        public ActionResult CategoryEntryChart()
        {
            var kategoriler = catm.GetActiveList();
            var basliklar = hm.GetActiveList();
            var entryler = cm.GetList("");
            var liste = kategoriler.Select(c => new CategoryClass
            {
                CategoryName = c.CategoryName,
                CategoryCount = entryler.Count(e => basliklar.Any(h => h.HeadingID == e.HeadingID && h.CategoryID == c.CategoryID))
            }).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        // 3) En çok entry yazan yazarlar (ilk 6)
        public ActionResult WriterChart()
        {
            var yazarlar = wm.GetList();
            var entryler = cm.GetList("");
            var liste = yazarlar.Select(y => new CategoryClass
            {
                CategoryName = y.WriterName + " " + y.WriterSurname,
                CategoryCount = entryler.Count(e => e.WriterID == y.WriterID)
            })
            .OrderByDescending(x => x.CategoryCount)
            .Take(6)
            .ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
    }
}