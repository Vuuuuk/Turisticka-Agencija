using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Turisticka_Agencija.Models;

namespace Turisticka_Agencija
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //INIT 
            Dictionary<string, Korisnik> initKorisnici = Podaci.ProcitajKorisnike("~/App_Data/Korisnici.txt");
            HttpContext.Current.Application["korisnici"] = initKorisnici;

            Dictionary<string, Smestaj> initSmestaji = Podaci.ProcitajSmestaje("~/App_Data/Smestaji.txt");
            HttpContext.Current.Application["smestaji"] = initSmestaji;

            Dictionary<string, Aranzman> initAranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            HttpContext.Current.Application["aranzmani"] = initAranzmani;

            //UCITVANJE JEDINICA U ODGOVARAJUCE SMESTAJNE OBJEKTE
            List<SmestajnaJedinica> initSmestajneJedinice = Podaci.ProcitajJedinice("~/App_Data/SmestajneJedinice.txt");
       
            foreach (SmestajnaJedinica s in initSmestajneJedinice)
                foreach (Aranzman a in initAranzmani.Values)
                    if (s.nazivSmestaja.Equals(a.smestaj.nazivSmestaja))
                        a.smestaj.smestajneJedinice.Add(s);

            //NAJNIZA CENA ZA PRIKAZ
            List<SmestajnaJedinica> najnizaCenaZaPrikaz = new List<SmestajnaJedinica>();
            foreach (SmestajnaJedinica s in initSmestajneJedinice.OrderByDescending(vrednost => vrednost.cena))
                najnizaCenaZaPrikaz.Add(s);
            HttpContext.Current.Application["cena"] = najnizaCenaZaPrikaz;

        }
    }
}
