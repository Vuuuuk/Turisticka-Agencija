using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{
    public class MestoNalazenja
    {
        public string adresa;
        public double geoDuzina;
        public double geoSirina;

        public MestoNalazenja() { }

        public MestoNalazenja(string adresa, double geoDuzina, double geoSirina)
        {
            this.adresa = adresa;
            this.geoDuzina = geoDuzina;
            this.geoSirina = geoSirina;
        }
    }
}