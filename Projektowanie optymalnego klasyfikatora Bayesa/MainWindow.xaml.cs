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