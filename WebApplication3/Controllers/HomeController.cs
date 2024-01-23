using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        Context dbcontext = new Context();
        Uyeler YeniUye = new Uyeler();
        Randevu YeniRandevu = new Randevu();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Randevular()
        {
            List<Randevu> randevular = dbcontext.Randevu.ToList();

            // Veritabanından diğer tabloyu çekme
            List<Uyeler> uyelerListesi = dbcontext.Uyeler.ToList();

            // ViewModel oluşturup, içindeki listelere verileri atama
            ViewModel viewModel = new ViewModel
            {
                RandevuListesi = randevular,
                UyelerListesi = uyelerListesi
            };

            return View(viewModel);
        }

        public ActionResult RandevuVer()
        {
            // Veritabanından SysAktifi 1 olan Uyeler tablosundaki verileri çekme
            List<Uyeler> uyeler = dbcontext.Uyeler.Where(u => u.SysAktif == 1).ToList();

            // SelectListItem listesi oluşturarak, SelectList'e uygun veri kaynağını sağlama
            List<SelectListItem> uyeListesi = uyeler.Select(u => new SelectListItem
            {
                Text = u.UyeAdi + " " + u.UyeSoyadi,
                Value = u.UyeID.ToString()
            }).ToList();

            // ViewBag aracılığıyla ViewBag.Uyeler adında bir SelectList oluşturup, bu SelectList'i View'e gönderme
            ViewBag.Uyeler = new SelectList(uyeListesi, "Value", "Text");

            return View();
        }


        [HttpPost]
        public ActionResult RandevuVer(Randevu model)
        {
            if (ModelState.IsValid)
            {
                // Formdan gelen bilgileri kullanarak randevu kaydı oluştur
                Randevu randevu = new Randevu
                {
                    UyeID = model.UyeID,
                    RandevuBaslangicTarihi = model.RandevuBaslangicTarihi,
                    RandevuHizmetSuresi = model.RandevuHizmetSuresi,
                    ChckBxSecimleri = model.ChckBxSecimleri,
                    NotAciklama = model.NotAciklama,
                    SysAktif = 1,
                    RandevuKayıtTarihi = DateTime.Now,
                    RandevuKaydedenPersonel = "Admin",
                    RandevuGuncellenmeTarihi = DateTime.Now,
                    RandevuGuncelleyenPersonel = "Admin",

                };

                // Oluşturulan randevu kaydını veritabanına ekleyin
                dbcontext.Randevu.Add(randevu);
                dbcontext.SaveChanges();

                // Başka bir işlem yapabilir veya bir başka sayfaya yönlendirebilirsiniz
                return RedirectToAction("Index");
            }

            // ModelState.IsValid false olduğunda, yani formda doğrulama hataları varsa, aynı View'i tekrar göster
            // UyeID'leri yeniden yüklemek için ViewBag'i kullanın
            ViewBag.Uyeler = new SelectList(dbcontext.Uyeler.ToList(), "UyeID", "UyeAdiSoyadi", model.UyeID);
            return View(model);
        }


        public ActionResult UyeKaydıYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeKaydıYap(FormCollection myform)
        {
            YeniUye.UyeAdi = myform["UyeAdi"].ToString().Trim();
            YeniUye.UyeSoyadi = myform["UyeSoyadi"].ToString().Trim();
            YeniUye.UyeTelNo = myform["UyeTelNo"].Trim();
            YeniUye.UyeTcNo = myform["UyeTcNo"]?.Trim();
            YeniUye.UyeMaili = myform["UyeMaili"]?.ToString().Trim();
            YeniUye.UyeOzelNotu = myform["UyeOzelNotu"]?.ToString().Trim();
            YeniUye.SysAktif = 1;
            YeniUye.UyeEklenmeTarihi = DateTime.Now;
            YeniUye.UyeEkleyenPersonel = "Admin";
            YeniUye.UyeGuncellenmeTarihi = DateTime.Now;
            YeniUye.UyeGuncelleyenPersonel = "Admin";

            dbcontext.Uyeler.Add(YeniUye);
            dbcontext.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult Uyeler()
        {
            var uyelerListesi = dbcontext.Uyeler.ToList();
            return View(uyelerListesi);
        }

        // Üye güncelleme işlemini gerçekleştiren aksiyon
        [HttpPost]
        public ActionResult Uyeler(Uyeler model)
        {
            if (ModelState.IsValid)
            {
                var existingUye = dbcontext.Uyeler.FirstOrDefault(u => u.UyeID == model.UyeID);

                if (existingUye != null)
                {
                    existingUye.UyeAdi = model.UyeAdi;
                    existingUye.UyeSoyadi = model.UyeSoyadi;
                    existingUye.UyeTelNo = model.UyeTelNo;
                    existingUye.UyeTcNo = model.UyeTcNo;
                    existingUye.UyeMaili = model.UyeMaili;
                    existingUye.UyeOzelNotu = model.UyeOzelNotu;
                    existingUye.UyeGuncellenmeTarihi = DateTime.Now;
                    existingUye.UyeGuncelleyenPersonel = "Admin";

                    dbcontext.SaveChanges(); // Değişiklikleri veritabanına kaydet

                    return RedirectToAction("Uyeler"); // Üye listesini gösteren sayfaya yönlendirme
                }
                else
                {
                    ModelState.AddModelError("", "Üye bulunamadı.");
                }
            }

            // ModelState.IsValid false olduğunda, yani formda doğrulama hataları varsa, uyeleri ve seçilen UyeID'yi ViewBag aracılığıyla yükleyerek kullanıcının hatasını düzeltmesine yardımcı olunuyor
            ViewBag.Uyeler = new SelectList(dbcontext.Uyeler.ToList(), "UyeID", "UyeAdiSoyadi", model.UyeID);
            return View("Uyeler", model); // Aynı sayfaya geri dön ve hataları göster
        }

        // Üyeyi pasif hale getirme işlemini gerçekleştiren aksiyon
        [HttpPost]
        public ActionResult UyelerPasif(Uyeler model)
        {
            if (ModelState.IsValid)
            {
                var existingUye = dbcontext.Uyeler.FirstOrDefault(u => u.UyeID == model.UyeID);

                if (existingUye != null)
                {
                    existingUye.SysAktif = 0;
                    existingUye.UyeGuncellenmeTarihi = DateTime.Now;
                    existingUye.UyeGuncelleyenPersonel = "Admin";

                    dbcontext.SaveChanges(); // Değişiklikleri veritabanına kaydet

                    return RedirectToAction("Uyeler"); // Üye listesini gösteren sayfaya yönlendirme
                }
                else
                {
                    ModelState.AddModelError("", "Üye bulunamadı.");
                }
            }

            // ModelState.IsValid false olduğunda, yani formda doğrulama hataları varsa, uyeleri ve seçilen UyeID'yi ViewBag aracılığıyla yükleyerek kullanıcının hatasını düzeltmesine yardımcı olunuyor
            ViewBag.Uyeler = new SelectList(dbcontext.Uyeler.ToList(), "UyeID", "UyeAdiSoyadi", model.UyeID);
            return View("Uyeler", model); // Aynı sayfaya geri dön ve hataları göster
        }

        // Üyeyi aktif hale getirme işlemini gerçekleştiren aksiyon
        [HttpPost]
        public ActionResult UyelerAktif(Uyeler model)
        {
            if (ModelState.IsValid)
            {
                var existingUye = dbcontext.Uyeler.FirstOrDefault(u => u.UyeID == model.UyeID);

                if (existingUye != null)
                {
                    existingUye.SysAktif = 1;
                    existingUye.UyeGuncellenmeTarihi = DateTime.Now;
                    existingUye.UyeGuncelleyenPersonel = "Admin";

                    dbcontext.SaveChanges(); // Değişiklikleri veritabanına kaydet

                    return RedirectToAction("Uyeler"); // Üye listesini gösteren sayfaya yönlendirme
                }
                else
                {
                    ModelState.AddModelError("", "Üye bulunamadı.");
                }
            }

            // ModelState.IsValid false olduğunda, yani formda doğrulama hataları varsa, uyeleri ve seçilen UyeID'yi ViewBag aracılığıyla yükleyerek kullanıcının hatasını düzeltmesine yardımcı olunuyor
            ViewBag.Uyeler = new SelectList(dbcontext.Uyeler.ToList(), "UyeID", "UyeAdiSoyadi", model.UyeID);
            return View("Uyeler", model); // Aynı sayfaya geri dön ve hataları göster
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
