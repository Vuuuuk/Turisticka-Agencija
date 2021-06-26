using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Turisticka_Agencija.Models
{
    public class Podaci
    {

        public static Dictionary<string, Aranzman> ProcitajAranzmane(string putanja)
        {
            Dictionary<string, Aranzman> aranzmani = new Dictionary<string, Aranzman>();
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split(';');
                Enum.TryParse(podaci[1], true, out TipAranzmana tipAranzmana);
                Enum.TryParse(podaci[2], true, out TipPrevoza tipPrevoza);
                DateTime datumPocetkaPutovanja = DateTime.ParseExact(podaci[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime datumZavrsetkaPutovanja = DateTime.ParseExact(podaci[5], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                double.TryParse(podaci[7], out double geoDuzina);
                double.TryParse(podaci[8], out double geoSirina);
                TimeSpan vremeNalazenja = DateTime.ParseExact(podaci[9], "HH.mm", CultureInfo.InvariantCulture).TimeOfDay;
                int.TryParse(podaci[10], out int maxBrojPutnika);
                Enum.TryParse(podaci[14], true, out TipSmestaja tipSmestaja);
                int.TryParse(podaci[16], out int brojZvezdica);
                bool.TryParse(podaci[17], out bool bazen);
                bool.TryParse(podaci[18], out bool spa);
                bool.TryParse(podaci[19], out bool prilagodjen);
                bool.TryParse(podaci[20], out bool wifi);
                MestoNalazenja mestoNalazenja = new MestoNalazenja(podaci[6], geoDuzina, geoSirina);
                Smestaj smestaj = new Smestaj(tipSmestaja, podaci[15], brojZvezdica, bazen, spa, prilagodjen, wifi);
                Aranzman aranzman = new Aranzman(podaci[0], tipAranzmana, tipPrevoza, podaci[3], datumPocetkaPutovanja, datumZavrsetkaPutovanja, mestoNalazenja, 
                                                 vremeNalazenja, maxBrojPutnika, podaci[11], podaci[12], podaci[13], smestaj);
                aranzmani.Add(aranzman.naziv,aranzman);
            }
            sr.Close();
            fs.Close();

            return aranzmani;
        }

        public static List<SmestajnaJedinica> ProcitajJedinice(string putanja)
        {
            List<SmestajnaJedinica> jedinice = new List<SmestajnaJedinica>();
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split(';');
                int.TryParse(podaci[1], out int dozvoljenBrojGostiju);
                bool.TryParse(podaci[2], out bool ljubimci);
                int.TryParse(podaci[3], out int cena);
                bool.TryParse(podaci[4], out bool dostupnost);
                SmestajnaJedinica smestajnaJedinica = new SmestajnaJedinica(podaci[0], dozvoljenBrojGostiju, ljubimci, cena, dostupnost);
                jedinice.Add(smestajnaJedinica);
            }
            sr.Close();
            fs.Close();

            return jedinice;
        }

        public static Dictionary<string, Korisnik> ProcitajKorisnike(string putanja)
        {
            Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split(';');
                Enum.TryParse(podaci[4], true, out Pol pol);
                DateTime datumRodjenja = DateTime.ParseExact(podaci[6], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Enum.TryParse(podaci[7], true, out Uloga uloga);
                Korisnik korisnik = new Korisnik(podaci[0], podaci[1], podaci[2], podaci[3], pol, podaci[5], datumRodjenja, uloga);
                korisnici.Add(korisnik.korisnicko_ime, korisnik);
            }
            sr.Close();
            fs.Close();

            return korisnici;
        }

        public static void UcitajKorisnika(Korisnik korisnik)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            using (StreamWriter file = File.AppendText(path))
                file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
                                                  korisnik.korisnicko_ime, korisnik.lozinka, korisnik.ime, korisnik.prezime, korisnik.pol,
                                                  korisnik.email, korisnik.datumRodjenja.ToString("dd/MM/yyyy"),
                                                  korisnik.uloga);
        }

        public static void ObrisiKorisnike()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Korisnici.txt");
            var file = File.Create(path);
            file.Close();
        }

    }
}