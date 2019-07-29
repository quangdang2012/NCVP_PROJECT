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
    public partial class frmRTV : Form
    {
        DataTable dtHistory;
        public frmRTV()
        {
            InitializeComponent();
        }

        private void defineDatatable(ref DataTable dt)
        {
            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("thurst", Type.GetType("System.String"));
            dt.Columns.Add("thurst_mc", Type.GetType("System.String"));
            dt.Columns.Add("noise", Type.GetType("System.String"));
            dt.Columns.Add("noise_mc", Type.GetType("System.String"));
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
                    string sql = "select * from t_product_serial where serialno = '" + serno + "'";
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
                    newrow["serialno"] = serno;
                    

                    // If tester data exists, show it in the datagridview
                    if (dt1.Rows.Count != 0)
                    {
                        string model = dt1.Rows[0][7].ToString();
                        string lot = dt1.Rows[0][5].ToString();
                        string line = dt1.Rows[0][2].ToString();
                        string thurst = dt1.Rows[0][3].ToString();
                        string thurst_mc = dt1.Rows[0][8].ToString();
                        string noise = dt1.Rows[0][4].ToString();
                        string noise_mc = dt1.Rows[0][9].ToString();

                        newrow["model"] = model;
                        newrow["lot"] = lot;
                        newrow["line"] = line;
                        newrow["thurst"] = thurst;
                        newrow["thurst_mc"] = thurst_mc;
                        newrow["noise"] = noise;
                        newrow["noise_mc"] = noise_mc;
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
                System.Diagnostics.Debug.Print(drv["serialno"].ToString() + ": " +
                    drv["model"].ToString() + ": " + drv["lot"].ToString());
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ShSQL sh = new ShSQL();
            DataTable data = dtHistory.Copy();
            DialogResult result = MessageBox.Show("Please export this data to excel for safety! Do you really want to delete these serials ?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                DialogResult result1 = MessageBox.Show("You can not restore the data after this action!!! Are you sure ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result1 == DialogResult.Yes) sh.sqlMultipleDelete(data);
            }
        }
    }
}