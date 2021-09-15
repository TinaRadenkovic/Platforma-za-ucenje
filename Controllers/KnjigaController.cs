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
    public class KnjigaController : Controller
    {
        KnjigaModel km = new KnjigaModel();
        PorukaModel pm = new PorukaModel();
		KomentarModel kkm = new KomentarModel();
		
        // GET: Knjiga
        public ActionResult Index(String pretraga)
        {
            ViewBag.knjige = km.FindAll(pretraga);
            return View();
        }
		
		public ActionResult LikeKnjiga(String id)
        {
            Knjiga b = km.Find(new ObjectId(id));
            if (b.ocena == null)
                b.ocena = 0.ToString();
            int ocena = Int16.Parse(b.ocena);
            ocena++;
            b.ocena = ocena.ToString();
            km.Update(b);
            ViewBag.knjige = km.FindAll("");
            return View("Index");
        }
		
        [HttpGet]
        public ActionResult AddKnjiga()
        {
            return View("AddKnjiga", new Knjiga());
        }

        [HttpPost]
        public ActionResult AddKnjiga(Knjiga b, HttpPostedFileBase nazivFajla)
        {
            if (nazivFajla != null)
            {
                string path = Server.MapPath("~/Resources/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                nazivFajla.SaveAs(path + Path.GetFileName(nazivFajla.FileName));
             b.nazivFajla = nazivFajla.FileName;
            }
             List<Poruka> zahteviNotifikacije = pm.FindZahtevKnjige();
            foreach (Poruka p in zahteviNotifikacije)
            {
                if (p.naslov == b.predmet)
                {
                    //salje email

                    var sub = "Nova knjiga " + b.predmet;
                    String poruka = "Na platformi je sada dostupan nova knjiga iz oblasti koju ste zahtevali. Link za preuzimanje materijala: https://localhost:44302/Resources/" + b.nazivFajla;
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

            km.Create(b);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AdminAddKnjiga(Knjiga b, HttpPostedFileBase nazivFajla)
        {
            if (nazivFajla != null)
            {
                string path = Server.MapPath("~/Resources/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                nazivFajla.SaveAs(path + Path.GetFileName(nazivFajla.FileName));
				
            b.nazivFajla = nazivFajla.FileName;
            }
          
			
			List<Poruka> zahteviNotifikacije = pm.FindZahtevKnjige();
            foreach (Poruka p in zahteviNotifikacije)
            {
                if (p.naslov == b.predmet)
                {
                    //salje email

                    var sub = "Nova knjiga " + b.predmet;
                    String poruka = "Na platformi je sada dostupan nova knjiga iz oblasti koju ste zahtevali. Link za preuzimanje materijala: https://localhost:44302/Resources/" + b.nazivFajla;
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


            km.Create(b);
            return RedirectToAction(nameof(AdminController.AdminKnjige), "Admin");
        }

        [HttpPost]
        public ActionResult AdminUpdateKnjiga(Knjiga b, HttpPostedFileBase trenutniFajl, String id)
        {
            var knjiga = km.Find(new ObjectId(id));
            Knjiga trKnjiga = knjiga;
            if(trenutniFajl != null)
            {
                string path = Server.MapPath("~/Resources/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                trenutniFajl.SaveAs(path + Path.GetFileName(trenutniFajl.FileName));

                trKnjiga.nazivFajla = trenutniFajl.FileName;

            }
            trKnjiga.predmet = b.predmet;
            trKnjiga.autor= b.autor;
            trKnjiga.opis = b.opis;
            trKnjiga.ocena = b.ocena;
            km.Update(trKnjiga);
            return RedirectToAction(nameof(AdminController.AdminKnjige), "Admin");
        }
		
		 public ActionResult AboutKnjiga(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = kkm.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = kkm.FindByDocumentSortPopular(id);
            else ViewBag.komentari = kkm.FindByDocument(id);
            return View();
        }
    }
}