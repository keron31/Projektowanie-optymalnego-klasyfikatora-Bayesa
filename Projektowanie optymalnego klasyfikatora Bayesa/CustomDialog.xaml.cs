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

namespace Projektowanie_optymalnego_klasyfikatora_Bayesa
{
    /// <summary>
    /// Logika interakcji dla klasy CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public CustomDialog()
        {
            InitializeComponent();
        }

        public void SetResultsText(string text)
        {
            rtbResults.Document.Blocks.Clear();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(text);
            rtbResults.Document.Blocks.Add(paragraph);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
