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

namespace MVCProjeKampi.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager cm = new ContentManager(new EfContentDal());
        CategoryManager catm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        AboutManager abm = new AboutManager(new EfAboutDal());
        ContactManager conm = new ContactManager(new EfContactDal());
        ImageFileManager imgm = new ImageFileManager(new EfImageFileDal());
        ContentValidator contentValidator = new ContentValidator();
        ContactValidator contactValidator = new ContactValidator();

        public ActionResult Headings()
        {
            var headinglist = hm.GetActiveList();          // sadece aktif başlıklar
            return View(headinglist);
        }

        public PartialViewResult Index(int id = 0)
        {
            var contentlist = cm.GetListByHeadingIDApproved(id);   // sadece onaylı entry'ler
            return PartialView(contentlist);
        }
        public ActionResult VitrinTest()
        {
            return View();
        }


        public ActionResult HomePage(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var headings = hm.GetActiveList();
            var contents = cm.GetList("");
            var categories = catm.GetActiveList();
            var writers = wm.GetList();
            var allCategories = catm.GetList();

            var counts = contents.GroupBy(x => x.HeadingID).ToDictionary(g => g.Key, g => g.Count());
            var latest = contents.GroupBy(x => x.HeadingID)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.ContentDate).First().ContentValue);
            var categoryNames = allCategories.ToDictionary(c => c.CategoryID, c => c.CategoryName);

            var trending = headings
                .OrderByDescending(h => counts.ContainsKey(h.HeadingID) ? counts[h.HeadingID] : 0)
                .Take(6).ToList();

            var model = new MVCProjeKampi.Models.VitrinHomeViewModel
            {
                Headings = headings.OrderByDescending(h => h.HeadingID).ToPagedList(pageNumber, pageSize),
                Trending = trending,
                Categories = categories,
                EntryCounts = counts,
                LatestEntry = latest,
                CategoryNames = categoryNames,
                TotalHeadings = headings.Count,
                TotalEntries = contents.Count,
                TotalWriters = writers.Count
            };
            return View(model);
        }
        // Başlık detay — entry akışı
        public ActionResult Baslik(int id, int? page)
        {
            var heading = hm.GetByID(id);
            if (heading == null || !heading.HeadingStatus)
                return RedirectToAction("HomePage");

            int pageSize = 10;
            int pageNumber = page ?? 1;

            var contents = cm.GetListByHeadingIDApproved(id).ToPagedList(pageNumber, pageSize);
            var writers = wm.GetList();

            ViewBag.Heading = heading;
            ViewBag.HeadingId = id;
            ViewBag.WriterNames = writers.ToDictionary(w => w.WriterID, w => (w.WriterName + " " + w.WriterSurname));
            ViewBag.Categories = catm.GetActiveList();
            ViewBag.TotalHeadings = hm.GetActiveList().Count;
            ViewBag.TotalEntries = cm.GetList("").Count;
            ViewBag.TotalWriters = writers.Count;

            var allHeadings = hm.GetActiveList();
            var allContents = cm.GetList("");
            var counts = allContents.GroupBy(x => x.HeadingID).ToDictionary(g => g.Key, g => g.Count());
            ViewBag.Trending = allHeadings
                .OrderByDescending(h => counts.ContainsKey(h.HeadingID) ? counts[h.HeadingID] : 0)
                .Take(6).ToList();
            ViewBag.TrendingCounts = counts;
            ViewBag.Writers = writers;

            return View(contents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EntryEkle(int headingId, string contentValue)
        {
            string mail = (string)Session["WriterMail"];
            if (mail == null) return RedirectToAction("WriterLogin", "Login");

            var writer = wm.GetByMail(mail);
            if (writer == null) return RedirectToAction("WriterLogin", "Login");

            Content p = new Content();
            p.ContentValue = contentValue;
            p.HeadingID = headingId;
            p.WriterID = writer.WriterID;
            p.ContentDate = DateTime.Now;
            p.ContentStatus = true;

            ValidationResult results = contentValidator.Validate(p);
            if (results.IsValid)
            {
                cm.ContentAdd(p);
            }
            else
            {
                // Hata mesajını TempData ile başlık sayfasına taşı
                TempData["EntryHata"] = results.Errors.First().ErrorMessage;
            }
            return RedirectToAction("Baslik", new { id = headingId });
        }
        public ActionResult Like(int id, int headingId)
        {
            cm.Like(id);
            return RedirectToAction("Baslik", new { id = headingId });
        }

        public ActionResult Dislike(int id, int headingId)
        {
            cm.Dislike(id);
            return RedirectToAction("Baslik", new { id = headingId });
        }
        [AllowAnonymous]
        public ActionResult Hakkimizda()
        {
            ViewBag.About = abm.GetList().FirstOrDefault();
            ViewBag.Gallery = imgm.GetList();
            ViewBag.TotalHeadings = hm.GetActiveList().Count;
            ViewBag.TotalEntries = cm.GetList("").Count;
            ViewBag.TotalWriters = wm.GetList().Count;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult IletisimGonder(Contact p)
        {
            p.ContactDate = DateTime.Now;

            ValidationResult results = contactValidator.Validate(p);
            if (results.IsValid)
            {
                conm.ContactAdd(p);
                TempData["IletisimMesaj"] = "Mesajınız başarıyla gönderildi, teşekkürler!";
            }
            else
            {
                TempData["IletisimMesaj"] = results.Errors.First().ErrorMessage;
            }
            return RedirectToAction("Hakkimizda");
        }
        [AllowAnonymous]
        public ActionResult Search(string p)
        {
            if (string.IsNullOrWhiteSpace(p))
                return RedirectToAction("HomePage");

            var allActive = hm.GetActiveList();

            // 1) Başlık adında geçenlerin ID'leri
            var idsByName = allActive
                .Where(h => h.HeadingName != null && h.HeadingName.ToLower().Contains(p.ToLower()))
                .Select(h => h.HeadingID);

            // 2) Entry içeriğinde geçenlerin başlık ID'leri
            var contentMatches = cm.GetList("")
                .Where(c => c.ContentValue != null && c.ContentValue.ToLower().Contains(p.ToLower()));
            var idsByContent = contentMatches.Select(c => c.HeadingID);

            // Birleşik ID kümesi
            var matchedIds = idsByName.Concat(idsByContent).Distinct().ToList();

            // Bu ID'lere sahip aktif başlıklar
            var combined = allActive
                .Where(h => matchedIds.Contains(h.HeadingID))
                .OrderByDescending(h => h.HeadingID)
                .ToList();

            // ... (ViewBag kısmı aynı kalıyor, altındaki kodu değiştirme)
            var contents = cm.GetList("");
            var counts = contents.GroupBy(x => x.HeadingID).ToDictionary(g => g.Key, g => g.Count());
            var latest = contents.GroupBy(x => x.HeadingID)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.ContentDate).First().ContentValue);
            var categoryNames = catm.GetList().ToDictionary(c => c.CategoryID, c => c.CategoryName);
            var trending = allActive
                .OrderByDescending(h => counts.ContainsKey(h.HeadingID) ? counts[h.HeadingID] : 0)
                .Take(6).ToList();

            ViewBag.Query = p;
            ViewBag.Trending = trending;
            ViewBag.TrendingCounts = counts;
            ViewBag.Categories = catm.GetActiveList();
            ViewBag.EntryCounts = counts;
            ViewBag.LatestEntry = latest;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.TotalHeadings = allActive.Count;
            ViewBag.TotalEntries = contents.Count;
            ViewBag.TotalWriters = wm.GetList().Count;

            return View(combined);
        }
        [AllowAnonymous]
        public ActionResult Kategori(int id)
        {
            var category = catm.GetByID(id);
            if (category == null) return RedirectToAction("HomePage");

            var headings = hm.GetListByCategory(id);
            var allActive = hm.GetActiveList();
            var contents = cm.GetList("");
            var counts = contents.GroupBy(x => x.HeadingID).ToDictionary(g => g.Key, g => g.Count());
            var latest = contents.GroupBy(x => x.HeadingID)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.ContentDate).First().ContentValue);
            var categoryNames = catm.GetList().ToDictionary(c => c.CategoryID, c => c.CategoryName);
            var trending = allActive
                .OrderByDescending(h => counts.ContainsKey(h.HeadingID) ? counts[h.HeadingID] : 0)
                .Take(6).ToList();

            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryId = id;
            ViewBag.Trending = trending;
            ViewBag.TrendingCounts = counts;
            ViewBag.Categories = catm.GetActiveList();
            ViewBag.EntryCounts = counts;
            ViewBag.LatestEntry = latest;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.TotalHeadings = allActive.Count;
            ViewBag.TotalEntries = contents.Count;
            ViewBag.TotalWriters = wm.GetList().Count;

            return View(headings.OrderByDescending(h => h.HeadingID).ToList());
        }
    }
}