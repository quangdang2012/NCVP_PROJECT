using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BoxIdDb
{
    public partial class frmHistory : Form
    {
        DataTable dtHistory;
        public frmHistory()
        {
            InitializeComponent();
        }

        private void defineDatatable(ref DataTable dt)
        {
            dt.Columns.Add("BoxID", Type.GetType("System.String"));
            dt.Columns.Add("Serial", Type.GetType("System.String"));
            dt.Columns.Add("Model", Type.GetType("System.String"));
            dt.Columns.Add("ShipDate", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            dtHistory = new DataTable();
            defineDatatable(ref dtHistory);
        }

        private void updateDataGridViews(DataTable dt, ref DataGridView dgv1)
        {
            dgvHistory.DataSource = dt;
            // Show row number to the row header
            for (int i = 0; i < dgv1.Rows.Count; i++)
                dgv1.Rows[i].HeaderCell.Value = (i + 1).ToString();

            // Adjust the width of the row header
            dgv1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // Show the bottom of the datagridview
            if (dgv1.Rows.Count >= 1)
                dgv1.FirstDisplayedScrollingRowIndex = dgv1.Rows.Count - 1;
        }

        private void txtSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Disenalbe the textbox to block scanning
                txtSerial.Enabled = false;

                string serno = txtSerial.Text;
                if (serno != String.Empty)
                {
                    string sql = "select boxid, serno, model, ship_date, status from shipment where serno = '" + serno + "'";
                    DataTable dt1 = new DataTable();
                    ShSQL tf = new ShSQL();
                    tf.sqlDataAdapterFillDatatable(sql, ref dt1);

                    System.Diagnostics.Debug.Print(sql);

                    DataView dv = new DataView(dt1);

                    //System.Diagnostics.Debug.Print(System.Environment.NewLine);
                    printDataView(dv);
                    DataTable dt2 = dv.ToTable();

                    // Even when no tester data is found, the module have to appear in the datagridview
                    DataRow newrow = dtHistory.NewRow();
                    newrow["serial"] = serno;

                    // If tester data exists, show it in the datagridview
                    if (dt1.Rows.Count != 0)
                    {
                        string model = dt1.Rows[0][2].ToString();
                        string ship_date = dt1.Rows[0][3].ToString();
                        string status = dt1.Rows[0][4].ToString();
                        string boxid = dt1.Rows[0][0].ToString();

                        newrow["boxid"] = boxid;
                        newrow["model"] = model;
                        newrow["shipdate"] = ship_date;
                        newrow["status"] = status;
                    }

                    // Add the row to the datatable
                    dtHistory.Rows.Add(newrow);

                    // ƒf[ƒ^ƒOƒŠƒbƒgƒrƒ…[‚ÌXV
                    updateDataGridViews(dtHistory, ref dgvHistory);
                }

                txtSerial.Enabled = true;
                txtSerial.Focus();
                txtSerial.SelectAll();
            }
        }

        private void printDataView(DataView dv)
        {
            foreach (DataRowView drv in dv)
            {
                System.Diagnostics.Debug.Print(drv["model"].ToString() + ": " +
                    drv["ship_date"].ToString() + ": " + drv["status"].ToString());
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)dgvHistory.DataSource;
            ExcelClass xl = new ExcelClass();
            xl.ExportToExcel(dt1);
        }
    }
}
