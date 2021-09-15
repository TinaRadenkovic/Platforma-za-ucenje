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
    public class BlanketController : Controller
    {
        BlanketModel bm = new BlanketModel();
        PorukaModel pm = new PorukaModel();
        KomentarModel km = new KomentarModel();

        // GET: Blanket/Index
        public ActionResult Index(String pretraga)
        {
            ViewBag.blanketi = bm.FindAll(pretraga);
            return View();
        }
    
        public ActionResult LikeBlanket(String id)
        {
            Blanket b = bm.Find(new ObjectId(id));
            if (b.ocena == null)
                b.ocena = 0.ToString();
            int ocena = Int16.Parse(b.ocena);
            ocena++;
            b.ocena = ocena.ToString();
            bm.Update(b);
            ViewBag.blanketi = bm.FindAll("");
            return View("Index");
        }

        [HttpGet]
        public ActionResult AddBlanket()
        {
            return View("AddBlanket", new Blanket());
        }

        [HttpPost]
        public ActionResult AddBlanket(Blanket b, HttpPostedFileBase nazivFajla)
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
            
            bm.Create(b);

            List<Poruka> zahteviNotifikacije = pm.FindZahtevBlanketi();
            foreach (Poruka p in zahteviNotifikacije)
            {
                if (p.naslov == b.predmet)
                {
                    var sub = "Novi blanket " + b.predmet;
                    String poruka = "Na platformi je sada dostupan novi blanket iz oblasti koju ste zahtevali." + Environment.NewLine + " Link za preuzimanje materijala: https://localhost:44302/Resources/" + b.nazivFajla;
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


            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult AdminAddBlanket(Blanket b, HttpPostedFileBase nazivFajla)
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
            bm.Create(b);
			
			 List<Poruka> zahteviNotifikacije = pm.FindZahtevBlanketi();
            foreach (Poruka p in zahteviNotifikacije)
            {
                if (p.naslov == b.predmet)
                {
                    //salje email

                    var sub = "Novi blanket " + b.predmet;
                    String poruka = "Na platformi je sada dostupan novi blanket iz oblasti koju ste zahtevali." + Environment.NewLine + "Link za preuzimanje materijala: https://localhost:44302/Resources/" + b.nazivFajla;
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

			
            return RedirectToAction(nameof(AdminController.AdminBlanketi), "Admin");
        }

        [HttpPost]
        public ActionResult AdminUpdateBlanket(Blanket b, HttpPostedFileBase trenutniFajl)
        {
            var blanket = bm.Find(b.id);
            Blanket trBlanket = blanket;
            if (trenutniFajl != null)
            {
                string path = Server.MapPath("~/Resources/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                trenutniFajl.SaveAs(path + Path.GetFileName(trenutniFajl.FileName));

                trBlanket.nazivFajla = trenutniFajl.FileName;

            }
            trBlanket.predmet = b.predmet;
            trBlanket.godina = b.godina;
            trBlanket.opis = b.opis;
            trBlanket.ocena = b.ocena;
            trBlanket.rok = b.rok;
            trBlanket.tipBlanketa = b.tipBlanketa;
            bm.Update(trBlanket);
            return RedirectToAction(nameof(AdminController.AdminBlanketi), "Admin");
        }

        

        public ActionResult AboutBlanket(String id, String sort)
        {
            ViewBag.dokument = id;
            if (sort == "najnoviji")
                ViewBag.komentari = km.FindByDocumentSortNew(id);
            else if (sort == "najpopularniji")
                ViewBag.komentari = km.FindByDocumentSortPopular(id);
            else ViewBag.komentari = km.FindByDocumentSortNew(id);
            return View();
        }
    }
}