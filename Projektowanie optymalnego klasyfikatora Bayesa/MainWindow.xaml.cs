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
        public ObservableCollection<Przypadek> PrzypadkiDoSklasyfikowania { get; set; } = new ObservableCollection<Przypadek>();
        public MainWindow()
        {
            InitializeComponent();
            dgResults.ItemsSource = Przypadki;
            dgCasesToClassify.ItemsSource = PrzypadkiDoSklasyfikowania;
        }

        public class Przypadek : INotifyPropertyChanged
        {
            private List<string> _values = new List<string>();

            public List<string> Values
            {
                get => _values;
                set
                {
                    _values = value;
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

            int columnCount = 0;

            foreach (string wiersz in wiersze)
            {
                // Rozdzielanie wartości w wierszu na podstawie separatora (w tym przypadku spacji)
                string[] wartosci = wiersz.Split(txtColumnSeparator.Text);

                columnCount = Math.Max(columnCount, wartosci.Length);

                // Dodawanie nowego przypadku
                Przypadek nowyPrzypadek = new Przypadek
                {
                    Values = wartosci.ToList()
                };
                Przypadki.Add(nowyPrzypadek);

            }

            // Generowanie kolumn na podstawie liczby wprowadzonych kolumn
            GenerateColumns(columnCount);

            txtCases.Clear();
        }

        private void GenerateColumns(int columnCount)
        {

            //dgResults.Columns.Clear();
            //var columnId = new DataGridTextColumn
            //{
            //    Header = "Obiekt (ID)",
            //    Binding = new Binding("Ids")
            //};
            //dgResults.Columns.Add(columnId);

            for (int i = 0; i < columnCount; i++)
            {
                var column = new DataGridTextColumn
                {
                    Header = $"q{i + 1}",
                    Binding = new Binding($"Values[{i}]")
                };
                dgResults.Columns.Add(column);
            }

            var actionsColumn = new DataGridTemplateColumn
            {
                Header = "Akcje",
                CellTemplate = CreateEdit_DeleteButtonTemplate()
            };
            dgResults.Columns.Add(actionsColumn);
        }

        private DataTemplate CreateEdit_DeleteButtonTemplate()
        {
            var buttonDelete = new FrameworkElementFactory(typeof(Button));
            buttonDelete.SetValue(Button.ContentProperty, "Usuń");
            buttonDelete.AddHandler(Button.ClickEvent, new RoutedEventHandler(DeleteButton_Click));

            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.AppendChild(buttonDelete);

            var template = new DataTemplate();
            template.VisualTree = stackPanel;
            return template;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = (Przypadek)button!.DataContext;
            Przypadki.Remove(row);
        }

        private void btnAddCasesToClassify_Click(object sender, RoutedEventArgs e)
        {
            string[] wiersze = txtCasesToClassify.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            int columnCount = 0;

            foreach (string wiersz in wiersze)
            {
                string[] wartosci = wiersz.Split(txtColumnSeparator.Text);

                columnCount = Math.Max(columnCount, wartosci.Length);

                Przypadek nowyPrzypadek = new Przypadek
                {
                    Values = wartosci.ToList()
                };
                PrzypadkiDoSklasyfikowania.Add(nowyPrzypadek);
            }

            GenerateColumnsForCasesToClassify(columnCount);

            txtCasesToClassify.Clear();
        }

        private void GenerateColumnsForCasesToClassify(int columnCount)
        {
            dgCasesToClassify.Columns.Clear();

            for (int i = 0; i < columnCount; i++)
            {
                var column = new DataGridTextColumn
                {
                    Header = $"q{i + 1}",
                    Binding = new Binding($"Values[{i}]")
                };
                dgCasesToClassify.Columns.Add(column);
            }
        }


        private void btnPredict_Click(object sender, RoutedEventArgs e)
        {
            var przypadki = Przypadki.ToList();
            var przypadkiDoSklasyfikowania = PrzypadkiDoSklasyfikowania.ToList();

            var liczbaAtrybutów = przypadkiDoSklasyfikowania.First().Values.Count;

            double P_Tak = przypadki.Count(x => x.Values.Last() == "Tak");
            double P_Nie = przypadki.Count(x => x.Values.Last() == "Nie");

            double PC1_Tak = P_Tak / (double)przypadki.Count;
            double PC2_Nie = P_Nie / (double)przypadki.Count;

            var Y_tak = new List<double>();
            var Y_nie = new List<double>();

            var Decyzje = new List<string>();
            var Wyniki = "";
            for (int i = 0; i < przypadkiDoSklasyfikowania.Count; i++)
            {
                var przypadekDoSklasyfikowania = przypadkiDoSklasyfikowania[i];

                for (int k = 0; k < liczbaAtrybutów; k++)
                {
                    var q = przypadekDoSklasyfikowania.Values[k];
                    double Pq_Tak = przypadki.Count(x => x.Values[k] == q && x.Values.Last() == "Tak") / P_Tak;
                    double Pq_Nie = przypadki.Count(x => x.Values[k] == q && x.Values.Last() == "Nie") / P_Nie;

                    if (Y_tak.Count > i)
                    {
                        Y_tak[i] *= Pq_Tak;
                        Y_nie[i] *= Pq_Nie;
                    }
                    else
                    {
                        Y_tak.Add(Pq_Tak);
                        Y_nie.Add(Pq_Nie);
                    }
                }

                double IleNaTak = Y_tak[i] * PC1_Tak;
                double IleNaNie = Y_nie[i] * PC2_Nie;

                if (IleNaTak > IleNaNie)
                {
                    Decyzje.Add("Tak");
                    Wyniki += "Przypadek Y" + (i + 1) + ": " +
                        string.Join(", ", przypadekDoSklasyfikowania.Values) +
                        " \nDecyzja: Tak \nNa tak: " + IleNaTak + ", \nNa nie: "
                        + IleNaNie + " \n\n";
                }
                else if (IleNaTak < IleNaNie)
                {
                    Decyzje.Add("Nie");
                    Wyniki += "Przypadek Y" + (i + 1) + ": " +
                        string.Join(", ", przypadekDoSklasyfikowania.Values) +
                        " \nDecyzja: Nie \nNa tak: " + IleNaTak + ", \nNa nie: "
                        + IleNaNie + " \n\n";
                }
                else
                {
                    Decyzje.Add("Wynik równy");
                    Wyniki += "Przypadek Y" + (i + 1) + ": " +
                        string.Join(", ", przypadekDoSklasyfikowania.Values) +
                        " \nDecyzja: Wynik równy \nNa tak: " + IleNaTak + ", \nNa nie: "
                        + IleNaNie + " \n\n";
                }
            }

            var customDialog = new CustomDialog();
            customDialog.SetResultsText(Wyniki);
            customDialog.ShowDialog();
        }

        private void btnInstruction_Click(object sender, RoutedEventArgs e)
        {
            var instructionDialog = new InstructionDialog();
            instructionDialog.ShowDialog();
        }
    }
}