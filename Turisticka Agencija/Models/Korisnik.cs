using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turisticka_Agencija.Models
{

    public enum Pol
    {
        MUSKI,
        ZENSKI,
        DRUGO
    }

    public enum Uloga
    {
        ADMINISTRATOR,
        MENADZER,
        TURISTA
    }

    public class Korisnik
    {
        public string korisnicko_ime { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Pol pol { get; set; }
        public string email { get; set; }
        public DateTime datumRodjenja { get; set; }
        public Uloga uloga { get; set; }
        public Dictionary<string, Rezervacija> rezervacije { get; set; }
        public Dictionary<string, Aranzman> aranzmani { get; set; }

        public Korisnik() { rezervacije = new Dictionary<string, Rezervacija>(); aranzmani = new Dictionary<string, Aranzman>(); }

        public Korisnik(string korisnicko_ime, string lozinka, string ime, string prezime, Pol pol, string email, DateTime datumRodjenja, Uloga uloga)
        {
            this.korisnicko_ime = korisnicko_ime;
            this.lozinka = lozinka;
            this.ime = ime;
            this.prezime = prezime;
            this.pol = pol;
            this.email = email;
            this.datumRodjenja = datumRodjenja;
            this.uloga = uloga;

            rezervacije = new Dictionary<string, Rezervacija>();
            aranzmani = new Dictionary<string, Aranzman>();
        }
    }
}