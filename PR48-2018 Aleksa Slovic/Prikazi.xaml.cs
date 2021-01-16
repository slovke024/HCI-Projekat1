using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace PR48_2018_Aleksa_Slovic
{
    /// <summary>
    /// Interaction logic for Prikazi.xaml
    /// </summary>
    public partial class Prikazi : Window
    {
        public Prikazi( int idx)
        {
            InitializeComponent();
            string naslov = MainWindow.Automobili_lista[idx].Naslov;
            string path = MainWindow.Automobili_lista[idx].Text;
            string model = MainWindow.Automobili_lista[idx].Model;
            string cena = MainWindow.Automobili_lista[idx].Cena.ToString();
            string slika = MainWindow.Automobili_lista[idx].SlikaAuta;

            NaslovDodavanje.Content = naslov;
            ModelDodavanje.Content = model;
            CenaDodavanje.Content = cena;
            Uri fileUri = new Uri(slika);
            slikaDodavanje.Source = new BitmapImage(fileUri);

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                TextRange range = new TextRange(rtbEdit.Document.ContentStart, rtbEdit.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Button_Izadji(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
