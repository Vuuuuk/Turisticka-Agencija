using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class SmestajController : Controller
    {


        //POCETNO STANJE ZA RESETOVANJE PRETRAGE I SORTIRANJA
        public static Dictionary<string, Rezervacija> testiranje = new Dictionary<string, Rezervacija>();


        //GENERATOR IDENTIFIKACIJA ARANZMANA
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        // GET: Smestaj
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrikazInformacijaSmestaja(string naziv)
        {

            //FULL RESET

            Dictionary<string, Aranzman> aranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            HttpContext.Application["aranzmani"] = aranzmani;

            List<SmestajnaJedinica> initSmestajneJedinice = Podaci.ProcitajJedinice("~/App_Data/SmestajneJedinice.txt");

            foreach (SmestajnaJedinica s in initSmestajneJedinice)
                foreach (Aranzman a in aranzmani.Values)
                    if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                        a.smestaj.smestajneJedinice.Add(s);

            List<SmestajnaJedinica> najnizaCenaZaPrikaz = new List<SmestajnaJedinica>();
            foreach (SmestajnaJedinica s in initSmestajneJedinice.OrderByDescending(vrednost => vrednost.cena))
                najnizaCenaZaPrikaz.Add(s);
            HttpContext.Application["cena"] = najnizaCenaZaPrikaz;

            //FULL RESET

            HttpContext.Application["smestaj"] = aranzmani[naziv].smestaj;
            HttpContext.Application["naziv"] = naziv;
            return View("~/Views/Smestaj/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Sortiranje(string podatakZaSortiranje, string tipSortiranja, string naziv)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            List<SmestajnaJedinica> sortiraneJedinice = new List<SmestajnaJedinica>();

            switch (podatakZaSortiranje)
            {
                case "Dozvoljen broj gostiju":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice.OrderByDescending(vrednost => vrednost.dozvoljenBrojGostiju))
                                sortiraneJedinice.Add(s);
                        }
                        else
                        {
                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice.OrderBy(vrednost => vrednost.dozvoljenBrojGostiju))
                                sortiraneJedinice.Add(s);
                        }
                        break;
                    }
                case "Cena smeštajne jedinice":
                    {
                        if(tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice.OrderByDescending(vrednost => vrednost.cena))
                                sortiraneJedinice.Add(s);
                        }
                        else
                        {
                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice.OrderBy(vrednost => vrednost.cena))
                                sortiraneJedinice.Add(s);
                        }
                        break;
                    }
            }

            aranzmani[naziv].smestaj.smestajneJedinice.Clear();

            foreach (SmestajnaJedinica s in sortiraneJedinice)
                foreach (Aranzman a in aranzmani.Values)
                    if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                        a.smestaj.smestajneJedinice.Add(s);

            HttpContext.Application["aranzmani"] = aranzmani;

            return View("~/Views/Smestaj/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Pretraga(string brojGostijuMin, string brojGostijuMax, string cenaJedinice, string dozvoljeBoravakLjubimcima, string naziv)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            List<SmestajnaJedinica> sortiraneJedinice = new List<SmestajnaJedinica>();

            if (!brojGostijuMin.Equals(string.Empty))
            {
                sortiraneJedinice.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice)
                    if(s.dozvoljenBrojGostiju >= Int32.Parse(brojGostijuMin))
                        sortiraneJedinice.Add(s);

                aranzmani[naziv].smestaj.smestajneJedinice.Clear();

                foreach (SmestajnaJedinica s in sortiraneJedinice)
                    foreach (Aranzman a in aranzmani.Values)
                        if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                            a.smestaj.smestajneJedinice.Add(s);

                HttpContext.Application["aranzmani"] = aranzmani;
            }

            if (!brojGostijuMax.Equals(string.Empty))
            {
                sortiraneJedinice.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice)
                    if (s.dozvoljenBrojGostiju <= Int32.Parse(brojGostijuMax))
                        sortiraneJedinice.Add(s);

                aranzmani[naziv].smestaj.smestajneJedinice.Clear();

                foreach (SmestajnaJedinica s in sortiraneJedinice)
                    foreach (Aranzman a in aranzmani.Values)
                        if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                            a.smestaj.smestajneJedinice.Add(s);

                HttpContext.Application["aranzmani"] = aranzmani;
            }

            if (!cenaJedinice.Equals(string.Empty))
            {
                sortiraneJedinice.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice)
                    if (s.cena <= Int32.Parse(cenaJedinice))
                        sortiraneJedinice.Add(s);

                aranzmani[naziv].smestaj.smestajneJedinice.Clear();

                foreach (SmestajnaJedinica s in sortiraneJedinice)
                    foreach (Aranzman a in aranzmani.Values)
                        if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                            a.smestaj.smestajneJedinice.Add(s);

                HttpContext.Application["aranzmani"] = aranzmani;
            }

            if (!dozvoljeBoravakLjubimcima.Equals(string.Empty))
            {
                switch(dozvoljeBoravakLjubimcima)
                {
                    case "Dozvoljen boravak":
                        {
                            sortiraneJedinice.Clear();
                            aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice)
                                if (s.ljubimci.Equals(true))
                                    sortiraneJedinice.Add(s);

                            aranzmani[naziv].smestaj.smestajneJedinice.Clear();

                            foreach (SmestajnaJedinica s in sortiraneJedinice)
                                foreach (Aranzman a in aranzmani.Values)
                                    if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                                        a.smestaj.smestajneJedinice.Add(s);

                            HttpContext.Application["aranzmani"] = aranzmani;

                            break;
                        }
                    case "Nedozvoljen boravak":
                        {
                            sortiraneJedinice.Clear();
                            aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                            foreach (SmestajnaJedinica s in aranzmani[naziv].smestaj.smestajneJedinice)
                                if (!s.ljubimci.Equals(true))
                                    sortiraneJedinice.Add(s);

                            aranzmani[naziv].smestaj.smestajneJedinice.Clear();

                            foreach (SmestajnaJedinica s in sortiraneJedinice)
                                foreach (Aranzman a in aranzmani.Values)
                                    if (s.nazivSmestaja.Equals(a.smestaj.naziv))
                                        a.smestaj.smestajneJedinice.Add(s);

                            HttpContext.Application["aranzmani"] = aranzmani;

                            break;
                        }
                }
            }

            return View("~/Views/Smestaj/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Rezervacija(string nazivAranzmana, string ID)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["korisnik"];
            SmestajnaJedinica smestaj = new SmestajnaJedinica();

            foreach (SmestajnaJedinica sj in aranzmani[nazivAranzmana].smestaj.smestajneJedinice)
                if (ID.Equals(sj.IDJedinice.ToString()))
                {
                    smestaj = sj;
                    sj.dostupnost = false;
                }

            Rezervacija rezervacija = new Rezervacija(RandomString(15),korisnik,Status.AKTIVNA,aranzmani[nazivAranzmana],smestaj);
            korisnik.rezervacije.Add(rezervacija.oznakaRezervacije,rezervacija);

            testiranje.Add(rezervacija.oznakaRezervacije, rezervacija);
            HttpContext.Application["rezervacije"] = testiranje;

            HttpContext.Application["aranzmani"] = aranzmani;
            HttpContext.Session["korisnik"] = korisnik;

            return View("~/Views/Smestaj/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Otkaz(string nazivAranzmana, string ID)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Korisnik korisnik = (Korisnik)HttpContext.Session["korisnik"];
            SmestajnaJedinica smestaj = new SmestajnaJedinica();

            foreach (SmestajnaJedinica sj in aranzmani[nazivAranzmana].smestaj.smestajneJedinice)
                if (ID.Equals(sj.IDJedinice.ToString()))
                {
                    smestaj = sj;
                    sj.dostupnost = true;
                }

            Rezervacija rezervacija = new Rezervacija(RandomString(15), korisnik, Status.OTKAZANA, aranzmani[nazivAranzmana], smestaj);
            korisnik.rezervacije.Add(rezervacija.oznakaRezervacije, rezervacija);

            testiranje.Add(rezervacija.oznakaRezervacije, rezervacija);
            HttpContext.Application["rezervacije"] = testiranje;

            HttpContext.Application["aranzmani"] = aranzmani;
            HttpContext.Session["korisnik"] = korisnik;

            return View("~/Views/Smestaj/Index.cshtml");
        }

    }
}