﻿using System;
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
    /// Logika interakcji dla klasy InstructionDialog.xaml
    /// </summary>
    public partial class InstructionDialog : Window
    {
        public InstructionDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
