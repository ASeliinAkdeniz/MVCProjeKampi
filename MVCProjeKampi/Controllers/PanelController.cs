using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Filters;

namespace MVCProjeKampi.Controllers
{
    [AdminAuthorize]
    public class PanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager catm = new CategoryManager(new EfCategoryDal());
        ContentManager cm = new ContentManager(new EfContentDal()); 
         WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult Index()
        {
            ViewBag.TotalHeadings = hm.GetActiveList().Count;
            ViewBag.TotalEntries = cm.GetList("").Count;
            ViewBag.TotalWriters = wm.GetList().Count;
            ViewBag.TotalCategories = catm.GetActiveList().Count;
            ViewBag.SonBasliklar = hm.GetActiveList().OrderByDescending(x => x.HeadingID).Take(5).ToList();
            return View();
        }
    }
}