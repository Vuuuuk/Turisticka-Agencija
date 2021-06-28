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
                bool.TryParse(podaci[21], out bool obrisanSmestaj);
                bool.TryParse(podaci[23], out bool obrisan);
                MestoNalazenja mestoNalazenja = new MestoNalazenja(podaci[6], geoDuzina, geoSirina);
                Smestaj smestaj = new Smestaj(tipSmestaja, podaci[15], brojZvezdica, bazen, spa, prilagodjen, wifi, obrisanSmestaj);
                Aranzman aranzman = new Aranzman(podaci[0], tipAranzmana, tipPrevoza, podaci[3], datumPocetkaPutovanja, datumZavrsetkaPutovanja, mestoNalazenja, 
                                                 vremeNalazenja, maxBrojPutnika, podaci[11], podaci[12], podaci[13], smestaj, podaci[22], obrisan);
                aranzmani.Add(aranzman.naziv,aranzman);
            }
            sr.Close();
            fs.Close();

            return aranzmani;
        }

        public static void UcitajAranzman(Aranzman aranzman)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Aranzmani.txt");
            using (StreamWriter file = File.AppendText(path))
                file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23}",
                                                  aranzman.naziv, aranzman.tipAranzmana, aranzman.tipPrevoza, aranzman.lokacija, 
                                                  aranzman.datumPocetkaPutovanja.ToString("dd-MM-yyyy"), aranzman.datumZavrsetkaPutovanja.ToString("dd-MM-yyyy"), 
                                                  aranzman.mestoNalazenja.adresa, aranzman.mestoNalazenja.geoDuzina, aranzman.mestoNalazenja.geoSirina,
                                                  aranzman.vremeNalazenja.ToString("hh'.'mm"), aranzman.maksimalanBrojPutnika, aranzman.opisAranzmana, aranzman.programPutovanja,
                                                  aranzman.posterAranzmana, aranzman.smestaj.tipSmestaja, aranzman.smestaj.nazivSmestaja, aranzman.smestaj.brojZvezdica, aranzman.smestaj.bazen,
                                                  aranzman.smestaj.spa, aranzman.smestaj.prilagodjen, aranzman.smestaj.wifi, aranzman.smestaj.obrisan, aranzman.menadzerID, aranzman.obrisan);
        }

        public static void ObrisiAranzmane()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Aranzmani.txt");
            var file = File.Create(path);
            file.Close();
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
                int.TryParse(podaci[5], out int IDJedinice);
                SmestajnaJedinica smestajnaJedinica = new SmestajnaJedinica(podaci[0], dozvoljenBrojGostiju, ljubimci, cena, dostupnost, IDJedinice);
                jedinice.Add(smestajnaJedinica);
            }
            sr.Close();
            fs.Close();

            return jedinice;
        }

        public static Dictionary<string, Smestaj> ProcitajSmestaje(string putanja)
        {
            Dictionary<string, Smestaj> smestaji = new Dictionary<string, Smestaj>();
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split(';');
                Enum.TryParse(podaci[0], true, out TipSmestaja tipSmestaja);
                int.TryParse(podaci[2], out int brojZvezdica);
                bool.TryParse(podaci[3], out bool bazen);
                bool.TryParse(podaci[4], out bool spa);
                bool.TryParse(podaci[5], out bool prilagodjen);
                bool.TryParse(podaci[6], out bool wifi);
                bool.TryParse(podaci[7], out bool obrisan);
                Smestaj smestaj = new Smestaj(tipSmestaja, podaci[1], brojZvezdica, bazen, spa, prilagodjen, wifi, obrisan);
                smestaji.Add(smestaj.nazivSmestaja, smestaj);
            }
            sr.Close();
            fs.Close();

            return smestaji;
        }

        public static void UcitajSmestaj(Smestaj smestaj)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Smestaji.txt");
            using (StreamWriter file = File.AppendText(path))
                file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",smestaj.tipSmestaja, smestaj.nazivSmestaja, smestaj.brojZvezdica, smestaj.bazen, 
                                                             smestaj.spa, smestaj.prilagodjen, smestaj.wifi, smestaj.obrisan);
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

        public static void ObrisiSmestaje()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Smestaji.txt");
            var file = File.Create(path);
            file.Close();
        }

    }
}