using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{
    public class Komentar
    {
        public Korisnik korisnik { get; set; }
        public Aranzman aranzman { get; set; }
        public string tekstKomentara { get; set; }
        public int ocena { get; set; }

        public Komentar() { }

        public Komentar(Korisnik korisnik, Aranzman aranzman, string tekstKomentara, int ocena)
        {
            this.korisnik = korisnik;
            this.aranzman = aranzman;
            this.tekstKomentara = tekstKomentara;
            this.ocena = ocena;
        }
    }
}