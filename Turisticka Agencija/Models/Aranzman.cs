using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{

    public enum TipAranzmana
    {
        NOCENJE_SA_DORUCKOM,
        POLUPANSION,
        PUN_PANSION,
        ALL_INCLUSIVE,
        NAJAM_APARTMANA
    }

    public enum TipPrevoza
    {
        AUTOBUS,
        AVION,
        AUTOBUS_I_AVION,
        INDIVIDUALAN,
        OSTALO
    }

    public class Aranzman
    {
        public string naziv { get; set; }
        public TipAranzmana tipAranzmana { get; set; }
        public TipPrevoza tipPrevoza { get; set; }
        public string lokacija;
        public DateTime datumPocetkaPutovanja { get; set; }
        public DateTime datumZavrsetkaPutovanja { get; set; }
        public MestoNalazenja mestoNalazenja { get; set; }
        public TimeSpan vremeNalazenja { get; set; }
        public int maksimalanBrojPutnika { get; set; }
        public string opisAranzmana { get; set; }
        public string programPutovanja { get; set; }
        public string posterAranzmana { get; set; }
        public Smestaj smestaj { get; set; }


        public Aranzman() { }

        public Aranzman(string naziv, TipAranzmana tipAranzmana, TipPrevoza tipPrevoza, string lokacija, DateTime datumPocetkaPutovanja, 
                        DateTime datumZavrsetkaPutovanja, MestoNalazenja mestoNalazenja, TimeSpan vremeNalazenja, int maksimalanBrojPutnika, 
                        string opisAranzmana, string programPutovanja, string posterAranzmana, Smestaj smestaj)
        {
            this.naziv = naziv;
            this.tipAranzmana = tipAranzmana;
            this.tipPrevoza = tipPrevoza;
            this.lokacija = lokacija;
            this.datumPocetkaPutovanja = datumPocetkaPutovanja;
            this.datumZavrsetkaPutovanja = datumZavrsetkaPutovanja;
            this.mestoNalazenja = mestoNalazenja;
            this.vremeNalazenja = vremeNalazenja;
            this.maksimalanBrojPutnika = maksimalanBrojPutnika;
            this.opisAranzmana = opisAranzmana;
            this.programPutovanja = programPutovanja;
            this.posterAranzmana = posterAranzmana;
            this.smestaj = smestaj;
        }
    }
}