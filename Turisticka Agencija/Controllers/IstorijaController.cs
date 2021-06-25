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
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderByDescending(vrednost => vrednost.Value.datumPocetkaPutovanja))
                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
            HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            return View("~/Views/Istorija/Index.cshtml");
        }
    }
}