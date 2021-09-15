using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Entites;
using WebPlatforma.Models;

namespace WebPlatforma.Controllers
{
    public class PorukaController : Controller
    {
        PorukaModel bm = new PorukaModel();
        // GET: Poruka
        public ActionResult Index()
        {
            ViewBag.poruke= bm.FindPoruke();
           // ViewBag.procitanePoruke = bm.FindProcitane();
            return View();
        }

        [HttpPost]
        public ActionResult AddPoruka(Poruka b)
        {
            bm.Create(b);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


    }
}