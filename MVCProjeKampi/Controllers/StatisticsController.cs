using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers
{
    public class StatisticsController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager com = new ContentManager(new EfContentDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        [AllowAnonymous]
        public ActionResult Index()
        {
            var writers = wm.GetList();
            var contents = com.GetList("");   // boş arama = tüm entry'ler

            ViewBag.KategoriSayisi = cm.GetList().Count;
            ViewBag.BaslikSayisi = hm.GetList().Count;
            ViewBag.EntrySayisi = contents.Count;
            ViewBag.YazarSayisi = writers.Count;

            // En aktif yazarlar: en çok entry yazan ilk 5
            var enAktif = contents
                .Where(x => x.WriterID != null)
                .GroupBy(x => x.WriterID)
                .Select(g => new { WriterID = g.Key, Adet = g.Count() })
                .OrderByDescending(x => x.Adet)
                .Take(5)
                .ToList();

            var liste = new List<KeyValuePair<string, int>>();
            foreach (var item in enAktif)
            {
                var w = writers.FirstOrDefault(x => x.WriterID == item.WriterID);
                string ad = w != null ? (w.WriterName + " " + w.WriterSurname) : "Bilinmeyen";
                liste.Add(new KeyValuePair<string, int>(ad, item.Adet));
            }
            ViewBag.EnAktifYazarlar = liste;

            return View();
        }
    }
}