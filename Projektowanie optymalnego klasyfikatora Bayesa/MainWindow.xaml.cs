using ExcelDataReader;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Projektowanie_optymalnego_klasyfikatora_Bayesa
{
    public partial class MainWindow : System.Windows.Window
    {
        public ObservableCollection<Przypadek> Przypadki { get; set; } = new ObservableCollection<Przypadek>();
        public MainWindow()
        {
            InitializeComponent();
            dgResults.ItemsSource = Przypadki;
        }

        public class Przypadek : INotifyPropertyChanged
        {
            private int _liczbaDrzwi;
            private int _mocSilnika;
            private string _kolor;
            private string _marka;
            private string _kupSamochod;

            public int LiczbaDrzwi
            {
                get => _liczbaDrzwi;
                set
                {
                    _liczbaDrzwi = value;
                    OnPropertyChanged();
                }
            }
            public int MocSilnika
            {
                get => _mocSilnika;
                set
                {
                    _mocSilnika = value;
                    OnPropertyChanged();
                }
            }

            public string Kolor
            {
                get => _kolor;
                set
                {
                    _kolor = value;
                    OnPropertyChanged();
                }
            }

            public string Marka
            {
                get => _marka;
                set
                {
                    _marka = value;
                    OnPropertyChanged();
                }
            }

            public string KupSamochod
            {
                get => _kupSamochod;
                set
                {
                    _kupSamochod = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void btnAddCases_Click(object sender, RoutedEventArgs e)
        {
            // Rozdzielanie wprowadzonych przypadków na wiersze
            string[] wiersze = txtCases.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string wiersz in wiersze)
            {
                // Rozdzielanie wartości w wierszu na podstawie separatora (w tym przypadku tabulacji)
                string[] wartosci = wiersz.Split(' ');

                try
                {
                    // Dodawanie nowego przypadku
                    Przypadek nowyPrzypadek = new Przypadek
                    {
                        LiczbaDrzwi = int.Parse(wartosci[0]),
                        MocSilnika = int.Parse(wartosci[1]),
                        Kolor = wartosci[2].Trim(),
                        Marka = wartosci[3].Trim(),
                        KupSamochod = wartosci[4].Trim()
                    };
                    Przypadki.Add(nowyPrzypadek);
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Błąd: Nieprawidłowy format danych w wierszu: '{wiersz}'. Upewnij się, że liczba drzwi i moc silnika są liczbami całkowitymi, a kolor i marka są ciągami tekstowymi. Wartości powinny być oddzielone tabulacją.");
                }
            }
        }

        private void btnPredict_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj powinien być zaimplementowany algorytm klasyfikatora Bayesa
            // Następnie zaktualizuj wartość właściwości KupSamochod dla każdego przypadku w kolekcji Przypadki

            // Na przykład:
            // Przypadki[0].KupSamochod = "Tak";
            // Przypadki[1].KupSamochod = "Nie";
            // ...
        }
    }
}