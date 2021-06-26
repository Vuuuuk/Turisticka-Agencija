using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            korisnik.uloga = Uloga.TURISTA;
            if(!korisnici.ContainsKey(korisnik.korisnicko_ime))
            {
                korisnici.Add(korisnik.korisnicko_ime, korisnik);
                Podaci.UcitajKorisnika(korisnik);
            }
            else
            {
                ViewBag.Error = "Korisničko ime već postoji u sistemu, molimo pokušajte ponovo\n";
            }
            return View("~/Views/Login/Index.cshtml");
        }

    }
}