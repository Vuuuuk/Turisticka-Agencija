using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class MenadzerController : Controller
    {
        // GET: Menadzer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pregled()
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

            return View();
        }

        public ActionResult Smestaj()
        {
            return View();
        }

        public ActionResult SmestajPregled()
        {

            //FULL RESET

            Dictionary<string, Aranzman> aranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            HttpContext.Application["aranzmani"] = aranzmani;

            List<SmestajnaJedinica> initSmestajneJedinice = Podaci.ProcitajJedinice("~/App_Data/SmestajneJedinice.txt");

            Dictionary<string, Smestaj> smestaji = Podaci.ProcitajSmestaje("~/App_Data/Smestaji.txt");
            HttpContext.Application["smestaji"] = smestaji;

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
        public ActionResult DodajAranzman(string naziv, TipAranzmana tipAranzmana, TipPrevoza tipPrevoza, string lokacija, DateTime datumPocetkaPutovanja, DateTime datumZavrsetkaPutovanja,
                                          string adresa, string geoDuzina, string geoSirina, string vremeNalazenja, int maksimalanBrojPutnika, string opisAranzmana, 
                                          string programPutovanja, string posterAranzmana, Smestaj smestaj, string menadzerID)
        {


            string pattern = @"[0-9]{2,2}[:][0-9]{2,2}";
            Regex rg = new Regex(pattern);

            if(rg.IsMatch(vremeNalazenja) && smestaj.brojZvezdica <= 5)
            {

                Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                if (!smestaji.ContainsKey(smestaj.nazivSmestaja))
                {
                    smestaji.Add(smestaj.nazivSmestaja, smestaj);
                    Podaci.UcitajSmestaj(smestaj);
                }

                MestoNalazenja mestoNalazenja = new MestoNalazenja(adresa, Double.Parse(geoDuzina), Double.Parse(geoSirina));
                TimeSpan vreme = TimeSpan.Parse(vremeNalazenja);
                Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
                Aranzman aranzman = new Aranzman(naziv, tipAranzmana, tipPrevoza, lokacija, datumPocetkaPutovanja, datumZavrsetkaPutovanja, mestoNalazenja, vreme,
                                                 maksimalanBrojPutnika, opisAranzmana, programPutovanja, posterAranzmana, smestaj, menadzerID, false);

                if (!aranzmani.ContainsKey(aranzman.naziv))
                {
                    aranzmani.Add(aranzman.naziv, aranzman);
                    Podaci.UcitajAranzman(aranzman);
                }
                else
                    ViewBag.Error = "Aranžman već postoji u sistemu, molimo proverite podatke\n";

                HttpContext.Application["aranzmani"] = aranzmani;

                ViewBag.Error = "Aranžman uspešno dodat";

            }

            else
            {
                ViewBag.Error = "Molimo pokušajte ponovo, proverite da li ste popunili sve podatke u odgovarajućem formatu";
            }

            return View("~/Views/Menadzer/Index.cshtml");
        }

        [HttpPost]
        public ActionResult DodajSmestaj(Smestaj smestaj)
        {

            if (smestaj.brojZvezdica <= 5)
            {
                Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                if (!smestaji.ContainsKey(smestaj.nazivSmestaja))
                {
                    smestaji.Add(smestaj.nazivSmestaja, smestaj);
                    Podaci.UcitajSmestaj(smestaj);
                }
                else
                    ViewBag.Error = "Aranžman već postoji u sistemu, molimo proverite podatke\n";

                HttpContext.Application["smestaji"] = smestaji;

                ViewBag.Error = "Uspešno dodat smeštaj";
            }

            else
            {
                ViewBag.Error = "Molimo pokušajte ponovo, proverite da li ste popunili sve podatke u odgovarajućem formatu";
            }

            return View("~/Views/Menadzer/Smestaj.cshtml");
        }

        [HttpPost]
        public ActionResult DeaktivirajSmestaj(string nazivSmestaja)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];
            smestaji[nazivSmestaja].obrisan = true;
            HttpContext.Application["smestaji"] = smestaji;

            Podaci.ObrisiSmestaje();
            foreach (Smestaj s in smestaji.Values)
                Podaci.UcitajSmestaj(s);

            foreach (Aranzman a in aranzmani.Values)
                if (a.smestaj.nazivSmestaja.Equals(nazivSmestaja))
                    a.smestaj.obrisan = true;

            HttpContext.Application["aranzmani"] = aranzmani;

            Podaci.ObrisiAranzmane();
            foreach (Aranzman a in aranzmani.Values)
                Podaci.UcitajAranzman(a);

            return View("~/Views/Menadzer/SmestajPregled.cshtml");
        }

        [HttpPost]
        public ActionResult AktivirajSmestaj(string nazivSmestaja)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];
            smestaji[nazivSmestaja].obrisan = false;
            HttpContext.Application["smestaji"] = smestaji;

            Podaci.ObrisiSmestaje();
            foreach (Smestaj s in smestaji.Values)
                Podaci.UcitajSmestaj(s);

            foreach (Aranzman a in aranzmani.Values)
                if (a.smestaj.nazivSmestaja.Equals(nazivSmestaja))
                    a.smestaj.obrisan = false;

            HttpContext.Application["aranzmani"] = aranzmani;

            Podaci.ObrisiAranzmane();
            foreach (Aranzman a in aranzmani.Values)
                Podaci.UcitajAranzman(a);

            return View("~/Views/Menadzer/SmestajPregled.cshtml");
        }

        [HttpPost]
        public ActionResult IzmeniSmestaj(Smestaj smestaj)
        {

            Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

            if (smestaj.brojZvezdica <= 5)
            {
                smestaji[smestaj.nazivSmestaja] = smestaj;

                Podaci.ObrisiSmestaje();

                foreach (Smestaj s in smestaji.Values)
                    Podaci.UcitajSmestaj(s);

                HttpContext.Application["smestaji"] = smestaji;

                foreach (Aranzman a in aranzmani.Values)
                    if (a.smestaj.nazivSmestaja.Equals(smestaj.nazivSmestaja))
                        a.smestaj = smestaj;

                Podaci.ObrisiAranzmane();

                foreach (Aranzman a in aranzmani.Values)
                    Podaci.UcitajAranzman(a);

                HttpContext.Application["aranzmani"] = aranzmani;

                ViewBag.Error = "Smeštaj uspešno izmenjen";
            }

            else
            {
                ViewBag.Error = "Molimo pokušajte ponovo, proverite da li ste popunili sve podatke u odgovarajućem formatu";
            }

            return View("~/Views/Menadzer/SmestajPregled.cshtml");
        }

        [HttpPost]
        public ActionResult SortiranjeSmestaja(string podatakZaSortiranje, string tipSortiranja)
        {
            Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];
            Dictionary<string, Smestaj> sortiraniSmestaji = new Dictionary<string, Smestaj>();
            switch (podatakZaSortiranje)
            {
                case "Naziv smeštaja":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Smestaj> smestaj in smestaji.OrderByDescending(vrednost => vrednost.Value.nazivSmestaja))
                                sortiraniSmestaji.Add(smestaj.Value.nazivSmestaja, smestaj.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Smestaj> smestaj in smestaji.OrderBy(vrednost => vrednost.Value.nazivSmestaja))
                                sortiraniSmestaji.Add(smestaj.Value.nazivSmestaja, smestaj.Value);
                        }
                        break;
                    }
            }

            HttpContext.Application["smestaji"] = sortiraniSmestaji;
            return View("~/Views/Menadzer/SmestajPregled.cshtml");
        }

        [HttpPost]
        public ActionResult PretragaSmestaja(string tipSmestaja, string nazivSmestaja, string spa, string prilagodjen, string wifi, string bazen)
        {
            Dictionary<string, Smestaj> smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

            Dictionary<string, Smestaj> pretrazeniSmestaji = new Dictionary<string, Smestaj>();

            if (!tipSmestaja.Equals(string.Empty))
            {
                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                foreach (Smestaj s in smestaji.Values)
                    if (s.tipSmestaja.ToString().Equals(tipSmestaja))
                        if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                            pretrazeniSmestaji.Add(s.nazivSmestaja, s);


                HttpContext.Application["smestaji"] = pretrazeniSmestaji;
            }

            else if (!nazivSmestaja.Equals(string.Empty))
            {
                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                foreach (Smestaj s in smestaji.Values)
                    if (s.nazivSmestaja.Contains(nazivSmestaja))
                        if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                            pretrazeniSmestaji.Add(s.nazivSmestaja, s);


                HttpContext.Application["smestaji"] = pretrazeniSmestaji;
            }

            else if (!spa.Equals(string.Empty))
            {
                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                switch (spa)
                {
                    case "true":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (s.spa)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);
                            break;
                        }
                    case "false":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (!s.spa)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);

                            break;
                        }
                }

                HttpContext.Application["smestaji"] = pretrazeniSmestaji;

            }

            else if (!prilagodjen.Equals(string.Empty))
            {

                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                switch (prilagodjen)
                {
                    case "true":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (s.prilagodjen)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);
                            break;
                        }
                    case "false":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (!s.prilagodjen)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);
                            break;
                        }
                }


                HttpContext.Application["smestaji"] = pretrazeniSmestaji;
            }

            else if (!wifi.Equals(string.Empty))
            {

                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                switch (wifi)
                {
                    case "true":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (s.wifi)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);
                            break;
                        }
                    case "false":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (!s.wifi)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);

                            break;
                        }
                }

                HttpContext.Application["smestaji"] = pretrazeniSmestaji;
            }

            else if (!bazen.Equals(string.Empty))
            {

                pretrazeniSmestaji.Clear();
                smestaji = (Dictionary<string, Smestaj>)HttpContext.Application["smestaji"];

                switch (bazen)
                {
                    case "true":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (s.bazen)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);

                            break;
                        }
                    case "false":
                        {

                            foreach (Smestaj s in smestaji.Values)
                                if (!s.bazen)
                                    if (!pretrazeniSmestaji.ContainsKey(s.nazivSmestaja))
                                        pretrazeniSmestaji.Add(s.nazivSmestaja, s);
                            break;
                        }
                }

                HttpContext.Application["smestaji"] = pretrazeniSmestaji;

            }

            return View("~/Views/Menadzer/SmestajPregled.cshtml");
        }

        [HttpPost]
        public ActionResult Deaktiviraj(string naziv)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            aranzmani[naziv].obrisan = true;
            HttpContext.Application["aranzmani"] = aranzmani;

            Podaci.ObrisiAranzmane();
            foreach (Aranzman a in aranzmani.Values)
                Podaci.UcitajAranzman(a);

            return View("~/Views/Menadzer/Pregled.cshtml");
        }

        [HttpPost]
        public ActionResult Aktiviraj(string naziv)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            aranzmani[naziv].obrisan = false;
            HttpContext.Application["aranzmani"] = aranzmani;

            Podaci.ObrisiAranzmane();
            foreach (Aranzman a in aranzmani.Values)
                Podaci.UcitajAranzman(a);

            return View("~/Views/Menadzer/Pregled.cshtml");
        }

        [HttpPost]
        public ActionResult Sortiranje(string podatakZaSortiranje, string tipSortiranja)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
            switch (podatakZaSortiranje)
            {
                case "Naziv aranžmana":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.naziv))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderBy(vrednost => vrednost.Value.naziv))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        break;
                    }
                case "Datum početka putovanja":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        break;
                    }
                case "Datum kraja putovanja":
                    {
                        if (tipSortiranja.Equals("Opadajuće"))
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumZavrsetkaPutovanja))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderBy(vrednost => vrednost.Value.datumZavrsetkaPutovanja))
                                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        }
                        break;
                    }
                default:
                    {
                        foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                            sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
                        break;
                    }
            }

            HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            return View("~/Views/Menadzer/Pregled.cshtml");
        }


        [HttpPost]
        public ActionResult Pretraga(string datumPocetkaMin, string datumPocetkaMax, string datumZavrsetkaMin, string datumZavrsetkaMax, string tipPrevoza, string tipAranzmana, string nazivAranzmana)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

            string dpmin = (string)HttpContext.Application["dpmin"];
            string dpmax = (string)HttpContext.Application["dpmax"];
            string dkmin = (string)HttpContext.Application["dkmin"];
            string dkmax = (string)HttpContext.Application["dkmax"];

            Dictionary<string, Aranzman> pretrazeniAranzmani = new Dictionary<string, Aranzman>();

            if (!datumPocetkaMin.Equals(dpmin))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumPocetkaPutovanja.CompareTo(DateTime.Parse(datumPocetkaMin)) >= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                HttpContext.Application["aranzmani"] = pretrazeniAranzmani;

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumPocetkaMax.Equals(dpmax))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumPocetkaPutovanja.CompareTo(DateTime.Parse(datumPocetkaMax)) <= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumZavrsetkaMin.Equals(dkmin))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumZavrsetkaPutovanja.CompareTo(DateTime.Parse(datumZavrsetkaMin)) >= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumZavrsetkaMax.Equals(dkmax))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumZavrsetkaPutovanja.CompareTo(DateTime.Parse(datumZavrsetkaMax)) <= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!tipPrevoza.Equals(string.Empty))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.tipPrevoza.ToString().Equals(tipPrevoza))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!tipAranzmana.Equals(string.Empty))
            {

                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.tipAranzmana.ToString().Equals(tipAranzmana))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!nazivAranzmana.Equals(string.Empty))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.naziv.Contains(nazivAranzmana))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in pretrazeniAranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            return View("~/Views/Menadzer/Pregled.cshtml");
        }

    }
}