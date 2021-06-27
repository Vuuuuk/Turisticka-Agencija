using System.Collections.Generic;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class LoginController : Controller
    {

        public bool ProveraKorisnika(string korisnickoIme, string lozinka)
        {
            bool izlaz = false;
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            if (korisnici.ContainsKey(korisnickoIme))
                if (korisnici[korisnickoIme].lozinka.Equals(lozinka))
                        izlaz = true;
            return izlaz;
        }


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Prijava(string korisnickoIme, string lozinka)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            if(ProveraKorisnika(korisnickoIme, lozinka))
            {
                Session["korisnik"] = korisnici[korisnickoIme];
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Pogrešno korisničko ime ili lozinka, molimo pokušajte ponovo ili se obratite administratoru";
                return View("~/Views/Login/Index.cshtml");
            }
        }

        public ActionResult Odjava()
        {
            Session["korisnik"] = null;
            return View("~/Views/Home/Index.cshtml");
        }

    }
}