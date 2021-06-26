using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{
    public class SmestajnaJedinica
    {
        public string nazivSmestaja { get; set; }
        public int dozvoljenBrojGostiju { get; set; }
        public bool ljubimci { get; set; }
        public int cena { get; set; }
        public bool dostupnost { get; set; }

        public SmestajnaJedinica() { }

        public SmestajnaJedinica(string nazivSmestaja, int dozvoljenBrojGostiju, bool ljubimci, int cena, bool dostupnost)
        {
            this.nazivSmestaja = nazivSmestaja;
            this.dozvoljenBrojGostiju = dozvoljenBrojGostiju;
            this.ljubimci = ljubimci;
            this.cena = cena;
            this.dostupnost = dostupnost;
        }
    }
}