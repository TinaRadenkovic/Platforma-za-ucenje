using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Entites;
using WebPlatforma.Models;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Net.Mail;

namespace WebPlatforma.Controllers
{
    public class PredavanjeController : Controller
    {
        PredavanjeModel bm = new PredavanjeModel();
        PorukaModel pm = new PorukaModel();
		KomentarModel km = new KomentarModel();
		
        // GET: Predavanje/Index
        public ActionResult Index(String pretraga)
        {
            ViewBag.predavanja = bm.FindAll(pretraga);
            return View();
        }
		
		 public ActionResult LikePredavanje(String id)
        {
            Predavanje b = bm.Find(new ObjectId(id));
            if (b.ocena == null)
                b.ocena = 0.ToString();
            int ocena = Int16.Parse(b.ocena);
            ocena++;
            b.ocena = ocena.ToString();
            bm.Update(b);
            ViewBag.predavanja = bm.FindAll("");
            return View("Index");
        }


        [HttpGet]
        public ActionResult AddPredavanje()
        {
            return View("AddPredavanje", new Predavanje());
        }

        [HttpGet]
        public ActionResult PrijavaPredavanje(String id)
        {
            //pronadji predavanje iz baze na osnovu ida
            return View("PrijavaPredavanje", bm.Find(new ObjectId(id)));
        }

        //[HttpPost]
        //public ActionResult UpdatePredavanje(Predavanje p)
        //{
        //    var trPredavanje = bm.Find(p.id);
        //    trPredavanje.predmet = p.predmet;
        //    trPredavanje.predavac = p.predavac;
        //    trPredavanje.mesto = p.mesto;
        //    trPredavanje.vreme = p.vreme;
        //    trPredavanje.ocena = p.ocena;
        //    trPredavanje.prijavljeniKorisnici += p.prijavljeniKorisnici;
        //    bm.Update(trPredavanje);
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult UpdatePredavanje(String id, String prijavljeniKorisnici)
        {
            var trPredavanje = bm.Find(new ObjectId(id));
            trPredavanje.prijavljeniKorisnici +=  prijavljeniKorisnici + ", ";
            bm.Update(trPredavanje);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeletePredavanje(String id)
        {
            bm.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdminAddPredavanje(Predavanje b)
        {
            bm.Create(b);

            List<Poruka> zahteviNotifikacije = pm.FindZahtevPredavanja();
            foreach (Poruka p in zahteviNotifikacije)
            {
                if (p.naslov == b.predmet)
                {
                    //salje email

                    var sub = "Novo predavanje " + b.predmet;
                    String poruka = "Na platformi je sada dostupno novo predavanje iz oblasti koju ste zahtevali." + Environment.NewLine + "Link do predavanja: https://localhost:44302/Predavanje/Index";
                    var body = poruka;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential("platformazaucenje@gmail.com", "ucenjePlatforma")
                    };
                    using (var mess = new MailMessage("platformazaucenje@gmail.com", p.emailKorisnika)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                }
            }
			
			
            return RedirectToAction(nameof(AdminController.AdminPredavanja), "Admin");
        }

        [HttpPost]
        public ActionResult AdminUpdatePredavanje(Predavanje p, String id)
        {
            var predavanje = bm.Find(new ObjectId(id));

            Predavanje trPredavanje = predavanje;
            trPredavanje.predmet = p.predmet;
            trPredavanje.predavac = p.predavac;
            trPredavanje.mesto = p.mesto;
            trPredavanje.vreme = p.vreme;
            trPredavanje.ocena = p.ocena;
            trPredavanje.prijavljeniKorisnici += p.prijavljeniKorisnici;
            bm.Update(trPredavanje);
            
            return RedirectToAction(nameof(AdminController.AdminPredavanja), "Admin");
        }
		
		public ActionResult AboutPredavanje(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = km.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = km.FindByDocumentSortPopular(id);
            else ViewBag.komentari = km.FindByDocument(id);
            return View();
        }
    }
}