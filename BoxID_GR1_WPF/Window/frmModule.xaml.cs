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

namespace BoxID_GR1_WPF
{
    /// <summary>
    /// Interaction logic for frmModule.xaml
    /// </summary>
    public partial class frmModule : Window
    {
        ShSQL tf = new ShSQL();
        public frmModule()
        {
            InitializeComponent();
        }

        private void btnReplaceSerial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegisterBoxId_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteSelection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChangeLimit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ckbDeleteBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (ckbDeleteBox.IsChecked == true) btnDeleteBoxId.Visibility = Visibility.Visible;
            else btnDeleteBoxId.Visibility = Visibility.Hidden;
        }

        private void btnDeleteBoxId_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TxtProductSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

            }
        }
    }
}
