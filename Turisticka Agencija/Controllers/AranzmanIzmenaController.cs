using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class AranzmanIzmenaController : Controller
    {
        // GET: AranzmanIzmena
        public ActionResult Index(string naziv)
        {
            HttpContext.Application["nazivAranzmana"] = naziv;
            return View();
        }

        [HttpPost]
        public ActionResult Izmena()
        {
            return View("~/Views/AranzmanIzmena/Index.cshtml");
        }


        [HttpPost]
        public ActionResult IzmeniAranzman(string naziv, TipAranzmana tipAranzmana, TipPrevoza tipPrevoza, string lokacija, DateTime datumPocetkaPutovanja, DateTime datumZavrsetkaPutovanja,
                                  string adresa, string geoDuzina, string geoSirina, string vremeNalazenja, int maksimalanBrojPutnika, string opisAranzmana,
                                  string programPutovanja, string posterAranzmana, Smestaj smestaj, string menadzerID, bool obrisan)
        {


            string pattern = @"[0-9]{2,2}[:][0-9]{2,2}";
            Regex rg = new Regex(pattern);

            string nazivAranzmana = (string)HttpContext.Application["nazivAranzmana"];

            if (rg.IsMatch(vremeNalazenja) && smestaj.brojZvezdica <= 5)
            {
                MestoNalazenja mestoNalazenja = new MestoNalazenja(adresa, Double.Parse(geoDuzina), Double.Parse(geoSirina));
                TimeSpan vreme = TimeSpan.Parse(vremeNalazenja);
                Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
                Aranzman aranzman = new Aranzman(naziv, tipAranzmana, tipPrevoza, lokacija, datumPocetkaPutovanja, datumZavrsetkaPutovanja, mestoNalazenja, vreme,
                                                 maksimalanBrojPutnika, opisAranzmana, programPutovanja, posterAranzmana, smestaj, menadzerID, obrisan);


                aranzmani[nazivAranzmana] = aranzman;


                Podaci.ObrisiAranzmane();

                foreach (Aranzman a in aranzmani.Values)
                    Podaci.UcitajAranzman(a);

                HttpContext.Application["aranzmani"] = aranzmani;


                ViewBag.Error = "Aranžman uspešno izmenjen";

            }

            else
            {
                ViewBag.Error = "Molimo pokušajte ponovo, proverite da li ste popunili sve podatke u odgovarajućem formatu";
            }

            return View("~/Views/AranzmanIzmena/Index.cshtml");
        }

    }
}