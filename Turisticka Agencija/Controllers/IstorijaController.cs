using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class IstorijaController : Controller
    {
        // GET: Istorija
        public ActionResult Index()
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


            //PRETHODNI ARANZMANI

            Dictionary<string, Aranzman> aranzmaniIstorija = new Dictionary<string, Aranzman>();

            foreach (Aranzman a in aranzmani.Values)
                if (a.datumPocetkaPutovanja.CompareTo(DateTime.Now) <= 0)
                    aranzmaniIstorija.Add(a.naziv, a);

            HttpContext.Application["istorija"] = aranzmaniIstorija;

            //PRETHODNI ARANZMANI


            Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
            HttpContext.Application["aranzmani"] = sortiraniAranzmani;

            return View("~/Views/Istorija/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Pretraga(string datumPocetkaMin, string datumPocetkaMax, string datumZavrsetkaMin, string datumZavrsetkaMax, string tipPrevoza, string tipAranzmana, string nazivAranzmana)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

            string dpmin = (string)HttpContext.Application["dpmin"];
            string dpmax = (string)HttpContext.Application["dpmax"];
            string dkmin = (string)HttpContext.Application["dkmin"];
            string dkmax = (string)HttpContext.Application["dkmax"];

            Dictionary<string, Aranzman> pretrazeniAranzmani = new Dictionary<string, Aranzman>();

            if (!datumPocetkaMin.Equals(dpmin))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumPocetkaPutovanja.CompareTo(DateTime.Parse(datumPocetkaMin)) >= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                HttpContext.Application["aranzmani"] = pretrazeniAranzmani;

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumPocetkaMax.Equals(dpmax))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumPocetkaPutovanja.CompareTo(DateTime.Parse(datumPocetkaMax)) <= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumZavrsetkaMin.Equals(dkmin))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumZavrsetkaPutovanja.CompareTo(DateTime.Parse(datumZavrsetkaMin)) >= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!datumZavrsetkaMax.Equals(dkmax))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.datumZavrsetkaPutovanja.CompareTo(DateTime.Parse(datumZavrsetkaMax)) <= 0)
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!tipPrevoza.Equals(string.Empty))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.tipPrevoza.ToString().Equals(tipPrevoza))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!tipAranzmana.Equals(string.Empty))
            {

                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.tipAranzmana.ToString().Equals(tipAranzmana))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            if (!nazivAranzmana.Equals(string.Empty))
            {
                pretrazeniAranzmani.Clear();
                aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];

                foreach (Aranzman a in aranzmani.Values)
                    if (a.naziv.Contains(nazivAranzmana))
                        if (!pretrazeniAranzmani.ContainsKey(a.naziv))
                            pretrazeniAranzmani.Add(a.naziv, a);

                //DEFAULT SORTIRANJE

                Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
                foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                    sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);

                //DEFAULT SORTIRANJE

                HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            }

            return View("~/Views/Istorija/Index.cshtml");
        }

        [HttpPost]
        public ActionResult Sortiranje(string podatakZaSortiranje, string tipSortiranja)
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["istorija"];
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
            return View("~/Views/Istorija/Index.cshtml");
        }
    }
}