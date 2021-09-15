using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Models;
using WebPlatforma.Entites;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebPlatforma.Controllers
{
    public class AdminController : Controller
    {
        AdminModel am = new AdminModel();
        PorukaModel bm = new PorukaModel();
        BlanketModel blm = new BlanketModel();
        KnjigaModel km = new KnjigaModel();
        PredavanjeModel pm = new PredavanjeModel();
        KomentarModel kkm = new KomentarModel();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.svePoruke = bm.FindPoruke();
            ViewBag.brojPoruka = bm.FindPoruke().Count();
            //ViewBag.procitanePoruke = bm.FindProcitane();
            return View();
        }

        [HttpPost]
        public ActionResult AdminHome(String email, String sifra)
        {
            Admin a = am.Find(email, sifra);
            if (a != null)
			{
				 ViewBag.svePoruke = bm.FindPoruke();
                 ViewBag.brojPoruka = bm.FindPoruke().Count();
				 return View("Index", a);
			}
            else
            {
                TempData["message"] = "<script>alert('Neispravno uneti podaci');</script>";
                ViewBag.neispravnoLogovanje = "Pogresno uneti podaci";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

		#region poruka
		
		[HttpGet]
        public ActionResult DeletePoruka(String id)
        {
            bm.Delete(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region predavanje
        [HttpGet]
        public ActionResult AdminPredavanja(String pretraga)
        {
            ViewBag.predavanja = pm.FindAll(pretraga);
            return View();
        }
		
        public ActionResult AdminPredavanja()
        {
            ViewBag.predavanja = pm.FindAll("");
            return View();
        }

        [HttpGet]
        public ActionResult AdminAddPredavanje()
        {
            return View("AdminAddPredavanje", new Predavanje());
        }
        

        [HttpGet]
        public ActionResult DeletePredavanje(String id)
        {
            pm.Delete(id);
            return RedirectToAction("AdminPredavanja");
        }
		
		
        [HttpGet]
        public ActionResult AdminUpdatePredavanje(String id)
        {
            ViewBag.predavanje = pm.Find(new ObjectId(id));
            return View("AdminUpdatePredavanje");
        }


        #endregion

        #region blanket

        [HttpGet]
        public ActionResult AdminBlanketi(String pretraga)
        {
            ViewBag.blanketi = blm.FindAll(pretraga);
            return View();
        }

        public ActionResult AdminBlanketi()
        {
            ViewBag.blanketi = blm.FindAll("");
            return View();
        }

        [HttpGet]
        public ActionResult AdminAddBlanket()
        {
            return View("AdminAddBlanket", new Blanket());
        }

        [HttpGet]
        public ActionResult DeleteBlanket(String id)
        {
            blm.Delete(id);
            return RedirectToAction("AdminBlanketi");
        }

        [HttpGet]
        public ActionResult AdminUpdateBlanket(String id)
        {
            ViewBag.blanket = blm.Find(new ObjectId(id));
            return View("AdminUpdateBlanket");
        }

        public ActionResult AdminAboutBlanket(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = kkm.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = kkm.FindByDocumentSortPopular(id);
            else ViewBag.komentari = kkm.FindByDocument(id);
            return View();
        } 
        public ActionResult AdminAboutKnjiga(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = kkm.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = kkm.FindByDocumentSortPopular(id);
            else ViewBag.komentari = kkm.FindByDocument(id);
            return View();
        } 
        public ActionResult AdminAboutPredavanje(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = kkm.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = kkm.FindByDocumentSortPopular(id);
            else ViewBag.komentari = kkm.FindByDocument(id);
            return View();
        }



        #endregion

        #region knjiga

        [HttpGet]
        public ActionResult AdminKnjige(String pretraga)
        {
            ViewBag.knjige = km.FindAll(pretraga);
            return View();
        }
        
        public ActionResult AdminKnjige()
        {
            ViewBag.knjige = km.FindAll("");
            return View();
        }

        [HttpGet]
        public ActionResult AdminAddKnjiga()
        {
            return View("AdminAddKnjiga", new Knjiga());
        }
		
		[HttpGet]
        public ActionResult DeleteKnjiga(String id)
        {
            km.Delete(id);
            return RedirectToAction("AdminKnjige");
        }

        [HttpGet]
        public ActionResult AdminUpdateKnjiga(String id)
        {
            ViewBag.knjiga = km.Find(new ObjectId(id));
            return View("AdminUpdateKnjiga");
        }



        #endregion

        #region korisnik
        public ActionResult AdminKorisnici(String pretraga)
        {
            ViewBag.korisniciB = bm.FindZahtevBlanketi();
            ViewBag.korisniciK = bm.FindZahtevKnjige();
            ViewBag.korisniciP = bm.FindZahtevPredavanja();

            return View();
        }
        #endregion
    }
}