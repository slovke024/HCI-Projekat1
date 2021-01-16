using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Klase;
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;
using DataFormats = System.Windows.DataFormats;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;


namespace PR48_2018_Aleksa_Slovic
{
    /// <summary>
    /// Interaction logic for DodajClanak.xaml
    /// </summary>
    public partial class DodajClanak : Window
    {
        public DodajClanak()
        {
            InitializeComponent();
            cmbFont.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            ComboBoxModel.ItemsSource = Konstante.modeli;
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22};
            cmbBoja.ItemsSource = typeof(Colors).GetProperties();
            cmbBoja.SelectedItem = Colors.Black;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void rtbEdit_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEdit.Selection.GetPropertyValue(Inline.FontWeightProperty);
            buttonBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEdit.Selection.GetPropertyValue(Inline.FontStyleProperty);
            buttonItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEdit.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            buttonUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));        
            temp = rtbEdit.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFont.SelectedItem = temp;
            temp = rtbEdit.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void cmbFont_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbFont.SelectedItem != null)
            {
                rtbEdit.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFont.SelectedItem);
            }

        }
        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
            {
                rtbEdit.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                BitmapImage source = new BitmapImage(new Uri(filename));
                browse.Source = source;
                putanjaSlike.Content = filename;
            }
        }

        private void dodaj_click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
                dlg.Title = "Snimanje teksta...";

                if (dlg.ShowDialog() == true)

                    using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create))
                    {
                        TextRange range = new TextRange(rtbEdit.Document.ContentStart, rtbEdit.Document.ContentEnd);
                        range.Save(fileStream, DataFormats.Rtf);

                        MainWindow.Automobili_lista.Add(new Klase.Automobili(nazivbox.Text, ComboBoxModel.SelectedValue.ToString(), dlg.FileName, Int32.Parse(textBoxCena.Text), putanjaSlike.Content.ToString(), DateTime.Now));
                        this.Close();
                    }
            }
            else
            {
                MessageBox.Show("Polja nisu dobro popunjena!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public bool IsRichTextBoxEmpty(RichTextBox rtb)
        {
            if (rtb.Document.Blocks.Count == 0) return true;
            TextPointer startPointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            TextPointer endPointer = rtb.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return startPointer.CompareTo(endPointer) == 0;
        }
        private bool validate()
        {
            bool provera_naslova = true;
            if (nazivbox.Text.Trim().Equals(""))
            {
                provera_naslova = false;
                nazivbox.BorderBrush = Brushes.Red;
                nazivbox.BorderThickness = new Thickness(1);
                labelProveraNaslova.Content = "Ne može biti prazno!";
            }
            else
            {
                nazivbox.BorderBrush = Brushes.Green;
                labelProveraNaslova.Content = string.Empty;
            }

            bool provera_modela = true;
            if (ComboBoxModel.SelectedItem == null)
            {
                provera_modela = false;
                ComboBoxModel.BorderBrush = Brushes.Red;
                ComboBoxModel.BorderThickness = new Thickness(1);
                labelProveraModela.Content = "Izaberite model!";
            }
            else
            {
                ComboBoxModel.BorderBrush = Brushes.Green;
                labelProveraModela.Content = string.Empty;
            }

            bool provera_teksta = true;
            if (IsRichTextBoxEmpty(rtbEdit))
            {
                provera_teksta=false;
                rtbEdit.BorderBrush = Brushes.Red;
                rtbEdit.BorderThickness = new Thickness(1);
                labelProveraTeksta.Content = "Unesite tekst!";
            }
            else
            {
                rtbEdit.BorderBrush = Brushes.Green;
                labelProveraTeksta.Content = string.Empty;
            }

            bool provera_brzine = true;
            int i;
            if (!int.TryParse(textBoxCena.Text, out i))
            {
                labelProveraCena.Content = "Ovo polje je samo za brojeve!";
                textBoxCena.BorderBrush = Brushes.Red;
                textBoxCena.BorderThickness = new Thickness(1);
                provera_brzine = false;
            }
            else
            {
                textBoxCena.BorderBrush = Brushes.Green;
                labelProveraCena.Content = string.Empty;
            }

            bool provera_slike = true;
            if (putanjaSlike.Content == string.Empty)
            {
                labelProveraSlike.Content = "Izaberite sliku!";
                dugmic.BorderBrush = Brushes.Red;
                dugmic.BorderThickness = new Thickness(1);
                provera_slike = false;
            }
            else
            {
                labelProveraSlike.Content = string.Empty;
                dugmic.BorderBrush = Brushes.Green;
            }

            if (provera_brzine == true && provera_slike == true && provera_modela == true && provera_naslova == true && provera_teksta == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void cmbBoja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBoja.SelectedItem != null)
            {
                string color = cmbBoja.SelectedItem.ToString().Substring(cmbBoja.SelectedItem.ToString().LastIndexOf(" ") + 1);
                BrushConverter c = new BrushConverter();
                SolidColorBrush brush = c.ConvertFromString(color) as SolidColorBrush;
                rtbEdit.Selection.ApplyPropertyValue(Inline.ForegroundProperty, brush);
            }
        }

        private void rtbEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rtbText = (new TextRange(rtbEdit.Document.ContentStart, rtbEdit.Document.ContentEnd)).Text;

            MatchCollection wordColl = Regex.Matches(rtbText,@"[\W]+");
            broj.Content = "Reči: " + wordColl.Count.ToString();

            TextRange range = new TextRange(rtbEdit.Document.ContentStart, rtbEdit.Document.ContentEnd);

            if (range.Text.Trim() == "")
            {
                broj.Content = "0";
            }
        }
    }
}
