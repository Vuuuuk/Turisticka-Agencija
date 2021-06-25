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
            Dictionary<string, Aranzman> initAranzmani = Podaci.ProcitajAranzmane("~/App_Data/Aranzmani.txt");
            HttpContext.Current.Application["aranzmani"] = initAranzmani;
        }
    }
}
