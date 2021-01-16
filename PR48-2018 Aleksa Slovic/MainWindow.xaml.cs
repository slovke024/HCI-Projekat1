using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PR48_2018_Aleksa_Slovic
{

    public partial class MainWindow : Window
    {
        private Klase.DataIO serializer = new Klase.DataIO();
        public static BindingList<Klase.Automobili> Automobili_lista { get; set; }

        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            Automobili_lista = serializer.DeSerializeObject<BindingList<Klase.Automobili>>("automobili.xml");
            if (Automobili_lista == null) 
            {
                Automobili_lista = new BindingList<Klase.Automobili>(); 
            }
            DataContext = this;
            InitializeComponent();
            uvod.Source = new Uri(Environment.CurrentDirectory + @"/uvod.mp4");
            loading();
        }

        private void time_tick(object sender, EventArgs e)
        {
            timer.Stop();
            uvod.Visibility = Visibility.Hidden;
            dataGridClanci.Visibility = Visibility.Visible;
            slika1.Visibility = Visibility.Visible;
            izlaz.Visibility = Visibility.Visible;
            dodaj.Visibility = Visibility.Visible;
        }

        void loading()
        {
            timer.Tick += time_tick;
            timer.Interval = new TimeSpan(0, 0, 7);
            timer.Start();
        }
        private void save(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<Klase.Automobili>>(Automobili_lista, "automobili.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajClanak newWindow = new DodajClanak();
            newWindow.ShowDialog();
        }

        private void PrikaziAuto(object sender, RoutedEventArgs e)
        {
            int index = dataGridClanci.SelectedIndex;

            Prikazi prikazi = new Prikazi(index);
            prikazi.ShowDialog();
            dataGridClanci.SelectedIndex = -1;
        }

        private void IzmeniAuto(object sender, RoutedEventArgs e)
        {
            int index = dataGridClanci.SelectedIndex;
            Izmeni izmeni = new Izmeni(index);
            izmeni.ShowDialog();
            dataGridClanci.SelectedIndex = -1;
        }

        private void ObrisiAuto(object sender, RoutedEventArgs e)
        {
            object item = dataGridClanci.SelectedItem;
            string Naziv = (dataGridClanci.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obristete clanak sa nazovom :  " + Naziv + "?", "Greska!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Automobili_lista.Remove((Klase.Automobili)item);
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {

        }
    }
}
