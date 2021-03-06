using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{
    public enum Status
    {
        AKTIVNA,
        OTKAZANA
    }

    public class Rezervacija
    {
        public string oznakaRezervacije { get; set; } //15 karaktera
        public Korisnik korisnik { get; set; }
        public Status status { get; set; }
        public Aranzman aranzman { get; set; }
        public SmestajnaJedinica smestajnaJedinica { get; set; }

        public Rezervacija() { }

        public Rezervacija(string oznakaRezervacije, Korisnik korisnik, Status status, Aranzman aranzman, SmestajnaJedinica smestajnaJedinica)
        {
            this.oznakaRezervacije = oznakaRezervacije;
            this.korisnik = korisnik;
            this.status = status;
            this.aranzman = aranzman;
            this.smestajnaJedinica = smestajnaJedinica;
        }
    }
}