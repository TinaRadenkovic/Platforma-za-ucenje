using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPlatforma.Models;
using WebPlatforma.Entites;

namespace WebPlatforma.Controllers
{
    public class HomeController : Controller
    {
        KnjigaModel km = new KnjigaModel();
        BlanketModel bm = new BlanketModel();
        public ActionResult Index()
        {
            ViewBag.knjige = km.FindAll("");
            ViewBag.blanketi = bm.FindAll("");
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult Proba()
        {
            return View();
        }
    }
}