using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
namespace Klase
{
    [Serializable]
    public class Automobili
    {        
        public string Naslov { get; set; }
        public string Model { get; set; }
        public string Text { get; set; }
        public DateTime Datum { get; set; }
        public string SlikaAuta { get; set; }
        public int Cena { get; set; }
        public Automobili()
        {

        }

        public Automobili( string naslov, string model, string tekst, int cena, string slika, DateTime datum)
        {
            SlikaAuta = slika;
            Model = model;
            Datum = datum;
            Cena = cena;
            Text = tekst;
            Naslov = naslov;
        }
    }
}
