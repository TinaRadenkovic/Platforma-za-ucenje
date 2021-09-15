using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Models;
using WebPlatforma.Entites;

namespace WebPlatforma.Controllers
{
    public class KorisnikController : Controller
    {
        // GET: Korisnik
        private KorisnikModel bm = new KorisnikModel();

        public ActionResult Random()
        {
            //var korisnik = new Korisnik{ email = "e", sifra = "e"};
            ////ViewData["Korisnik"] = korisnik;
            //return View(korisnik);
            //// return Content("hello");

            ViewBag.blanketi = bm.FindAll();
            return View();
        }
        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
        }
    }
}