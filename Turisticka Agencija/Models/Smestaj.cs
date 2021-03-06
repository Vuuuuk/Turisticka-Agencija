using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{

    public enum TipSmestaja
    {
        HOTEL,
        MOTEL,
        VILA
    }

    public class Smestaj
    {
        public TipSmestaja tipSmestaja { get; set; }
        public string nazivSmestaja { get; set; }
        public int brojZvezdica { get; set; }
        public bool bazen { get; set; }
        public bool spa { get; set; }
        public bool prilagodjen { get; set; }
        public bool wifi { get; set; }
        public List<SmestajnaJedinica> smestajneJedinice { get; set; }

        public bool obrisan;


        public Smestaj() { smestajneJedinice = new List<SmestajnaJedinica>(); }

        public Smestaj(TipSmestaja tipSmestaja, string nazivSmestaja, int brojZvezdica, bool bazen, bool spa, bool prilagodjen, bool wifi, bool obrisan)
        {
            this.tipSmestaja = tipSmestaja;
            this.nazivSmestaja = nazivSmestaja;
            this.brojZvezdica = brojZvezdica;
            this.bazen = bazen;
            this.spa = spa;
            this.prilagodjen = prilagodjen;
            this.wifi = wifi;

            this.obrisan = obrisan;

            smestajneJedinice = new List<SmestajnaJedinica>();
        }
    }
}