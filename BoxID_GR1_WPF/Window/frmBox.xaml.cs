using System;
using System.Collections.Generic;
using System.Data;
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

namespace BoxID_GR1_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dgvBoxId.AutoGenerateColumns = false;
        }

        private void BtnAddBoxID_Click(object sender, RoutedEventArgs e)
        {
            new frmModule().ShowDialog();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnShipmentHistory_Click(object sender, RoutedEventArgs e)
        {
            new frmHistory().ShowDialog();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string shipdate = dtpShipDate.Text;
            string printdate = dtpPrintDate.Text;
            ShSQL tf = new ShSQL();
            DataTable dt2 = new DataTable();

            if (rdPrintDate.IsChecked == true)
            {
                tf.sqlDataAdapterFillDatatable("select boxid, user_cd, child_model, printdate, shipdate from t_box_id WHERE printdate >= '" + printdate + " 00:00:00' and printdate < '" + printdate + " 23:59:59'", ref dt2);
            }
            if (rdShipDate.IsChecked == true)
            {
                tf.sqlDataAdapterFillDatatable("select boxid as BoxID, user_cd as User, child_model as ChildModel, printdate as PrintDate, shipdate as ShipDate from t_box_id WHERE shipdate >= '" + shipdate + " 00:00:00' and shipdate < '" + shipdate + " 23:59:59'", ref dt2);
            }
            dgvBoxId.ItemsSource = dt2.DefaultView;
        }
        public string GetData(DataGrid grid)
        {
            StringBuilder val = new StringBuilder();
            foreach (DataGridCellInfo info in grid.SelectedCells)
            {
                object value = grid.GetCellValue(info);
                val.Append(value);
            }
            return val.ToString();
        }
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            //int currentRow = int.Parse(e.RowIndex.ToString());
            string boxId = GetData(dgvBoxId);
            MessageBox.Show(boxId);
            //// OPEN button generate frmModule by view mode without delegate event
            //if (dgvBoxId.Columns[0].GetValue == openBoxId && currentRow >= 0)
            //{
            //    // In case frmModule is already opened, close it first
            //    ShGeneral.closeOpenForm("frmModule");

            //    //string boxId = dgvBoxId["boxid", currentRow].Value.ToString();
            //    DateTime printDate = DateTime.Parse(dgvBoxId["printdate", currentRow].Value.ToString());
            //    string user = dgvBoxId["user", currentRow].Value.ToString();

            //    frmModule fM = new frmModule();
            //    fM.updateControls(boxId, printDate, user, false);
            //    fM.Show();
            //}
        }

        private void DgvBoxId_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            StringBuilder val = new StringBuilder();
            foreach (DataGridCellInfo info in dgvBoxId.SelectedCells)
            {
                object value = dgvBoxId.GetCellValue(info);
                val.Append(value);
            }
            txtInvoice.Text = val.ToString();
        }
    }
}
