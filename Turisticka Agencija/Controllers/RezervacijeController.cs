using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class RezervacijeController : Controller
    {

        // GET: Rezervacije
        public ActionResult Index()
        {

            //FULL RESET

            Dictionary<string, Aranzman> aranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            HttpContext.Application["aranzmani"] = aranzmani;

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

            Dictionary<string, Rezervacija> rezervacije = (Dictionary<string, Rezervacija>)HttpContext.Application["rezervacije"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["korisnik"];
            korisnik.rezervacije = rezervacije;
            HttpContext.Session["korisnik"] = korisnik;
            return View();
        }

        [HttpPost]
        public ActionResult Sortiranje(string podatakZaSortiranje, string tipSortiranja)
        {

            Korisnik korisnik = (Korisnik)HttpContext.Session["korisnik"];
            Korisnik soritraniKorisnik = new Korisnik();

            switch (podatakZaSortiranje)
            {
                case "Broj gostiju":
                {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (Rezervacija rezervacija in korisnik.rezervacije.Values.OrderByDescending(vrednost => vrednost.smestajnaJedinica.dozvoljenBrojGostiju))
                            soritraniKorisnik.rezervacije.Add(rezervacija.oznakaRezervacije, rezervacija);
                            break;
                        }

                        else
                        {
                            foreach (Rezervacija rezervacija in korisnik.rezervacije.Values.OrderBy(vrednost => vrednost.smestajnaJedinica.dozvoljenBrojGostiju))
                                soritraniKorisnik.rezervacije.Add(rezervacija.oznakaRezervacije, rezervacija);
                            break;
                        }
                }
                case "Status":
                {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (Rezervacija rezervacija in korisnik.rezervacije.Values.OrderByDescending(vrednost => vrednost.status))
                                soritraniKorisnik.rezervacije.Add(rezervacija.oznakaRezervacije, rezervacija);
                            break;
                        }

                        else
                        {
                            foreach (Rezervacija rezervacija in korisnik.rezervacije.Values.OrderBy(vrednost => vrednost.status))
                                soritraniKorisnik.rezervacije.Add(rezervacija.oznakaRezervacije, rezervacija);
                            break;
                        }
                    }
            }

            korisnik.rezervacije = soritraniKorisnik.rezervacije;
            HttpContext.Session["korisnik"] = korisnik;
            return View("~/Views/Rezervacije/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Pretraga(string brojGostijuMin, string brojGostijuMax, string nazivAranzmana, string nazivSmestaja)
        {
            Korisnik korisnik = (Korisnik)HttpContext.Session["korisnik"];
            Korisnik soritraniKorisnik = new Korisnik();

            if (!brojGostijuMin.Equals(string.Empty))
            {
                soritraniKorisnik = new Korisnik();
                korisnik = (Korisnik)HttpContext.Session["korisnik"];

                foreach (Rezervacija r in korisnik.rezervacije.Values)
                    if (r.smestajnaJedinica.dozvoljenBrojGostiju >= Int32.Parse(brojGostijuMin))
                        if (!soritraniKorisnik.rezervacije.ContainsKey(r.oznakaRezervacije))
                            soritraniKorisnik.rezervacije.Add(r.oznakaRezervacije, r);

                korisnik.rezervacije = soritraniKorisnik.rezervacije;
                HttpContext.Session["korisnik"] = korisnik;

            }

            if (!brojGostijuMax.Equals(string.Empty))
            {
                soritraniKorisnik = new Korisnik();
                korisnik = (Korisnik)HttpContext.Session["korisnik"];

                foreach (Rezervacija r in korisnik.rezervacije.Values)
                    if (r.smestajnaJedinica.dozvoljenBrojGostiju <= Int32.Parse(brojGostijuMax))
                        if (!soritraniKorisnik.rezervacije.ContainsKey(r.oznakaRezervacije))
                            soritraniKorisnik.rezervacije.Add(r.oznakaRezervacije, r);

                korisnik.rezervacije = soritraniKorisnik.rezervacije;
                HttpContext.Session["korisnik"] = korisnik;

            }

            if (!nazivAranzmana.Equals(string.Empty))
            {
                soritraniKorisnik = new Korisnik();
                korisnik = (Korisnik)HttpContext.Session["korisnik"];

                foreach (Rezervacija r in korisnik.rezervacije.Values)
                    if (r.aranzman.naziv.Contains(nazivAranzmana))
                        if (!soritraniKorisnik.rezervacije.ContainsKey(r.oznakaRezervacije))
                            soritraniKorisnik.rezervacije.Add(r.oznakaRezervacije, r);

                korisnik.rezervacije = soritraniKorisnik.rezervacije;
                HttpContext.Session["korisnik"] = korisnik;

            }

            if (!nazivSmestaja.Equals(string.Empty))
            {
                soritraniKorisnik = new Korisnik();
                korisnik = (Korisnik)HttpContext.Session["korisnik"];

                foreach (Rezervacija r in korisnik.rezervacije.Values)
                    if (r.smestajnaJedinica.nazivSmestaja.Contains(nazivSmestaja))
                        if (!soritraniKorisnik.rezervacije.ContainsKey(r.oznakaRezervacije))
                            soritraniKorisnik.rezervacije.Add(r.oznakaRezervacije, r);

                korisnik.rezervacije = soritraniKorisnik.rezervacije;
                HttpContext.Session["korisnik"] = korisnik;

            }

            if(nazivSmestaja.Equals(string.Empty) && nazivAranzmana.Equals(string.Empty) && brojGostijuMax.Equals(string.Empty) && brojGostijuMin.Equals(string.Empty))
            {
                Dictionary<string, Rezervacija> rezervacije = (Dictionary<string, Rezervacija>)HttpContext.Application["rezervacije"];
                korisnik.rezervacije = rezervacije;
                HttpContext.Session["korisnik"] = korisnik;
            }

            return View("~/Views/Rezervacije/Index.cshtml");
        }

    }
}