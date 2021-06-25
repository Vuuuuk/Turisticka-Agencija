﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Dictionary<string, Aranzman> aranzmani = (Dictionary<string, Aranzman>)HttpContext.Application["aranzmani"];
            Dictionary<string, Aranzman> sortiraniAranzmani = new Dictionary<string, Aranzman>();
            foreach (KeyValuePair<string, Aranzman> aranzman in aranzmani.OrderBy(vrednost => vrednost.Value.datumPocetkaPutovanja))
                sortiraniAranzmani.Add(aranzman.Value.naziv, aranzman.Value);
            HttpContext.Application["aranzmani"] = sortiraniAranzmani;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}