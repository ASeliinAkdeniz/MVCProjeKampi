using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Filters;

namespace MVCProjeKampi.Controllers
{
    // Her admin rolü erişebilir (writer erişemez)
    [AdminAuthorize(Roles = "BasAdmin,Moderator,KategoriYoneticisi,YazarYoneticisi,DestekYoneticisi,IcerikEditoru")]
    public class AdminProfileController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());

        [HttpGet]
        public ActionResult ChangePassword()
        {
            // Kimlik giriş çerezinden geliyor — sadece kendi kaydı
            var kullaniciAdi = User.Identity.Name;
            var admin = am.GetList().FirstOrDefault(x => x.AdminUserName == kullaniciAdi);
            if (admin == null)
                return RedirectToAction("Index", "Login");
            return View(admin);
        }

        [HttpPost]
        public ActionResult ChangePassword(string yeniSifre, string yeniSifreTekrar)
        {
            var kullaniciAdi = User.Identity.Name;
            var admin = am.GetList().FirstOrDefault(x => x.AdminUserName == kullaniciAdi);
            if (admin == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(yeniSifre) || yeniSifre != yeniSifreTekrar)
            {
                ViewBag.Mesaj = "Şifreler boş olamaz ve iki alan birbiriyle aynı olmalı.";
                return View(admin);
            }

            admin.AdminPassword = HashHelper.CreateHash(yeniSifre);   // <-- hash'lendi
            am.AdminUpdate(admin);
            ViewBag.Mesaj = "Şifreniz başarıyla güncellendi.";
            return View(admin);
        }
    }
}