using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nalozi()
        {

            //FULL RESET

            Dictionary<string, Aranzman> aranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            Dictionary<string, Korisnik> korisnici = Podaci.ProcitajKorisnike("~/App_Data/Korisnici.txt");
            HttpContext.Application["aranzmani"] = aranzmani;
            HttpContext.Application["korisnici"] = korisnici;

            List<SmestajnaJedinica> initSmestajneJedinice = Podaci.ProcitajJedinice("~/App_Data/SmestajneJedinice.txt");

            foreach (SmestajnaJedinica s in initSmestajneJedinice)
                foreach (Aranzman a in aranzmani.Values)
                    if (s.nazivSmestaja.Equals(a.smestaj.nazivSmestaja))
                        a.smestaj.smestajneJedinice.Add(s);

            List<SmestajnaJedinica> najnizaCenaZaPrikaz = new List<SmestajnaJedinica>();
            foreach (SmestajnaJedinica s in initSmestajneJedinice.OrderByDescending(vrednost => vrednost.cena))
                najnizaCenaZaPrikaz.Add(s);
            HttpContext.Application["cena"] = najnizaCenaZaPrikaz;

            //FULL RESET

            return View();
        }

        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            korisnik.uloga = Uloga.MENADZER;
            if (!korisnici.ContainsKey(korisnik.korisnicko_ime))
            {
                korisnici.Add(korisnik.korisnicko_ime, korisnik);
                Podaci.UcitajKorisnika(korisnik);

                ViewBag.Error = "Menadžer uspešno registrovan\n";
            }
            else
            {
                ViewBag.Error = "Menadžer ime već postoji u sistemu, molimo pokušajte ponovo\n";
            }
            return View("~/Views/Administrator/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Sortiranje(string podatakZaSortiranje, string tipSortiranja)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];
            Dictionary<string, Korisnik> sortiraniKorisnici = new Dictionary<string, Korisnik>();
            switch (podatakZaSortiranje)
            {
                case "Ime korisnika":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderByDescending(vrednost => vrednost.Value.ime))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderBy(vrednost => vrednost.Value.ime))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        break;
                    }
                case "Prezime korisnika":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderByDescending(vrednost => vrednost.Value.prezime))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderBy(vrednost => vrednost.Value.prezime))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        break;
                    }
                case "Uloga korisnika":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderByDescending(vrednost => vrednost.Value.uloga))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Korisnik> korisnik in korisnici.OrderBy(vrednost => vrednost.Value.uloga))
                                sortiraniKorisnici.Add(korisnik.Value.korisnicko_ime, korisnik.Value);
                        }
                        break;
                    }
            }

            HttpContext.Application["korisnici"] = sortiraniKorisnici;
            return View("~/Views/Administrator/Nalozi.cshtml");
        }

        [HttpPost]
        public ActionResult Pretraga(string ime, string prezime, string uloga)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];

            Dictionary<string, Korisnik> pretrazeniKorisnici = new Dictionary<string, Korisnik>();

            if (!ime.Equals(string.Empty))
            {
                pretrazeniKorisnici.Clear();
                korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];

                foreach (Korisnik k in korisnici.Values)
                    if (k.ime.Contains(ime))
                        if (!pretrazeniKorisnici.ContainsKey(k.korisnicko_ime))
                            pretrazeniKorisnici.Add(k.korisnicko_ime, k);

                HttpContext.Application["korisnici"] = pretrazeniKorisnici;
            }

            else if (!prezime.Equals(string.Empty))
            {
                pretrazeniKorisnici.Clear();
                korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];

                foreach (Korisnik k in korisnici.Values)
                    if (k.prezime.Contains(prezime))
                        if (!pretrazeniKorisnici.ContainsKey(k.korisnicko_ime))
                            pretrazeniKorisnici.Add(k.korisnicko_ime, k);

                HttpContext.Application["korisnici"] = pretrazeniKorisnici;
            }

            else if (!uloga.Equals(string.Empty))
            {
                pretrazeniKorisnici.Clear();
                korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["korisnici"];

                foreach (Korisnik k in korisnici.Values)
                    if (k.uloga.ToString().Equals(uloga))
                        if (!pretrazeniKorisnici.ContainsKey(k.korisnicko_ime))
                            pretrazeniKorisnici.Add(k.korisnicko_ime, k);

                HttpContext.Application["korisnici"] = pretrazeniKorisnici;
            }

            return View("~/Views/Administrator/Nalozi.cshtml");
        }
    }
}