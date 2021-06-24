using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{
    public class SmestajnaJedinica
    {
        public int oznakaJedinice { get; set; }
        public int dozvoljenBrojGostiju { get; set; }
        public bool ljubimci { get; set; }
        public int cena { get; set; }

        public SmestajnaJedinica() { }

        public SmestajnaJedinica(int oznakaJedinice, int dozvoljenBrojGostiju, bool ljubimci, int cena)
        {
            this.oznakaJedinice = oznakaJedinice;
            this.dozvoljenBrojGostiju = dozvoljenBrojGostiju;
            this.ljubimci = ljubimci;
            this.cena = cena;
        }
    }
}