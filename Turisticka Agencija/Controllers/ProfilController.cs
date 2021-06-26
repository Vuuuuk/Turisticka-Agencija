using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Azuriranje(Korisnik korisnik)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            korisnici[korisnik.korisnicko_ime] = korisnik;
            HttpContext.Application["korisnici"] = korisnici;
            HttpContext.Session["korisnik"] = korisnik;
            ViewBag.Poruka = "Uspešno ažuriran profil";

            Podaci.ObrisiKorisnike();

            foreach (Korisnik k in korisnici.Values)
                Podaci.UcitajKorisnika(k);

            return View("~/Views/Profil/Index.cshtml");
        }
    }
}