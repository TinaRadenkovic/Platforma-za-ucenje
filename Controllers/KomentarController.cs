using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Entites;
using WebPlatforma.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Controllers
{
    public class KomentarController : Controller
    {
        KomentarModel km = new KomentarModel();
        // GET: Komentar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddKomentar(Komentar k)
        {
            if(k.idKorisnika == null)
            {

            }

            km.Create(k);

            return RedirectToAction(nameof(BlanketController.AboutBlanket), "Blanket", new { id = k.dokument});
        }

        [HttpPost]
        public ActionResult AddSubKomentar(Komentar k, String glavniKomentar)
        {
            Komentar glavni = km.Find(new ObjectId(glavniKomentar));
            k.dokument = glavni.dokument;
            k.id = ObjectId.GenerateNewId();
            glavni.odgovori.Add(k);
            km.Update(glavni);
			
            return RedirectToAction(nameof(BlanketController.AboutBlanket), "Blanket", new { id = k.dokument});
        }

        public ActionResult LikeKomentar(String id)
        {
            Komentar b = km.Find(new ObjectId(id));
            if (b.ocena == null)
                b.ocena = 0.ToString();
            int ocena = Int16.Parse(b.ocena);
            ocena++;
            b.ocena = ocena.ToString();
            km.Update(b);
            ViewBag.komentari = km.FindByDocument(id);
			
            return RedirectToAction(nameof(BlanketController.AboutBlanket), "Blanket", new { id = b.dokument});
        }
		
		public ActionResult LikeSubKomentar(String id, String idPodKom)
        {
            Komentar b = km.Find(new ObjectId(id));
            Komentar k = b.odgovori.Find(kom => kom.id == new ObjectId(idPodKom));
            if (k.ocena == null)
                k.ocena = 0.ToString();
            int ocena = Int16.Parse(k.ocena);
            ocena++;
            k.ocena = ocena.ToString();
            
            km.Update(b);
            ViewBag.komentari = km.FindByDocument(id);
			
            return RedirectToAction(nameof(BlanketController.AboutBlanket), "Blanket", new { id = b.dokument});
        }

        public ActionResult DislikeSubKomentar(String id, String idPodKom)
        {
            Komentar b = km.Find(new ObjectId(id));
            Komentar k = b.odgovori.Find(kom => kom.id == new ObjectId(idPodKom));
            if (k.negativnaOcena == null)
                k.negativnaOcena = 0.ToString();
            int ocena = Int16.Parse(k.negativnaOcena);
            ocena++;
            k.negativnaOcena = ocena.ToString();

            km.Update(b);
            ViewBag.komentari = km.FindByDocument(id);

            return RedirectToAction(nameof(BlanketController.AboutBlanket), "Blanket", new { id = b.dokument });
        }

        [HttpGet]
        public ActionResult DeleteSubKomentar(String id, String idPodKom)
        {
            Komentar b = km.Find(new ObjectId(id));
            Komentar k = b.odgovori.Find(kom => kom.id == new ObjectId(idPodKom));

            b.odgovori.Remove(k);

            km.Update(b);
            ViewBag.komentari = km.FindByDocument(id);

            return RedirectToAction(nameof(AdminController.AdminAboutBlanket), "Admin", new { id = b.dokument });
        }

        [HttpGet]
        public ActionResult DeleteKomentar(String id)
        {
            Komentar b = km.Find(new ObjectId(id));
            string dok = b.dokument;

            km.Delete(id);
            return RedirectToAction(nameof(AdminController.AdminAboutBlanket), "Admin", new { id = dok });
        }

    }
}