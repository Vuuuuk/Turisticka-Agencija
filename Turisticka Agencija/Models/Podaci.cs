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
                int.TryParse(podaci[9], out int maxBrojPutnika);
                Enum.TryParse(podaci[14], true, out TipSmestaja tipSmestaja);
                int.TryParse(podaci[16], out int brojZvezdica);
                bool.TryParse(podaci[17], out bool bazen);
                bool.TryParse(podaci[18], out bool spa);
                bool.TryParse(podaci[19], out bool prilagodjen);
                bool.TryParse(podaci[20], out bool wifi);
                MestoNalazenja mestoNalazenja = new MestoNalazenja(podaci[6], geoDuzina, geoSirina);
                Smestaj smestaj = new Smestaj(tipSmestaja, podaci[13], brojZvezdica, bazen, spa, prilagodjen, wifi);
                Aranzman aranzman = new Aranzman(podaci[0], tipAranzmana, tipPrevoza, podaci[3], datumPocetkaPutovanja, datumZavrsetkaPutovanja, mestoNalazenja, 
                                                 vremeNalazenja, maxBrojPutnika, podaci[11], podaci[12], podaci[13], smestaj);
                aranzmani.Add(aranzman.naziv,aranzman);
            }
            sr.Close();
            fs.Close();

            return aranzmani;
        }

    }
}