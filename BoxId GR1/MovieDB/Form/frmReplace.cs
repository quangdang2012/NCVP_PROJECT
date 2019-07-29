using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Permissions;
using System.Collections;

namespace BoxIdDb
{
    public partial class frmReplace : Form
    {
        string boxID;
        ShSQL tf = new ShSQL();
        public frmReplace(string box)
        {
            InitializeComponent();
            lblBoxID.Text = "BoxID: " + box;
            boxID = box;
        }

        // Sub procedure: Get module recors from database and set them into this form's datatable
        private void defineAndReadDtOverall(ref DataTable dt)
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
        // Sub procedure: Mark the test results with FAIL or missing test records
        private void colorViewForFailAndBlank(ref DataGridView dgv)
        {
            int rowCount = dgv.BindingContext[dgv.DataSource, dgv.DataMember].Count;

            for (int i = 0; i < rowCount; ++i)
            {
                if (dgv["thurst", i].Value.ToString() == "NG" || dgv["thurst", i].Value.ToString() == String.Empty)
                {
                    dgv["thurst", i].Style.BackColor = Color.Red;
                }
                else
                {
                    dgv["thurst", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }

                if (dgv["noise", i].Value.ToString() == String.Empty || dgv["noise", i].Value.ToString().Substring(0, 2) == "NG")
                {
                    dgv["noise", i].Style.BackColor = Color.Red;
                }
                else
                {
                    dgv["noise", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }
        private void txtAfterSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string serLong = txtAfterSerial.Text;
                if (serLong != String.Empty)
                {
                    // Get the tester data from current month's table and store it in datatable
                    string sql = "select a.a90_barcode, a90_line as line, a90_thurst_status as thurst, a90_factory as thurst_mc, b.judgment AS noise, b.eq_id as noise_mc from (select row_number() over(partition by a90_barcode order by oid desc) thurtid, a90_barcode, a90_model, a90_line, a90_thurst_status, a90_factory from t_checkpusha90 where a90_barcode = '" + serLong + "') a full join (select row_number() over(partition by barcode order by noise_id desc) noiseid, barcode, judgment, eq_id from t_noisecheck_a90 where barcode = '" + serLong + "') b on a.a90_barcode = b.barcode where thurtid = 1 or noiseid = 1";

                    DataTable dtReplace = new DataTable();
                    defineAndReadDtOverall(ref dtReplace);
                    DataTable dt1 = new DataTable();
                    tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                    // Even when no tester data is found, the module have to appear in the datagridview
                    string lot = string.Empty;

                    lot = VBStrings.Mid(serLong, 1, 5);

                    // Even when no tester data is found, the module have to appear in the datagridview
                    DataRow newr = dtReplace.NewRow();
                    newr["serialno"] = serLong;
                    newr["model"] = "LPD_5SG";
                    newr["lot"] = lot;

                    // If tester data exists, show it in the datagridview
                    if (dt1.Rows.Count != 0)
                    {
                        string line = dt1.Rows[0][1].ToString();
                        string thurst = dt1.Rows[0][2].ToString();
                        string thurst_mc = dt1.Rows[0][3].ToString();
                        string noise = dt1.Rows[0][4].ToString();
                        string noise_mc = dt1.Rows[0][5].ToString();

                        newr["line"] = line;
                        newr["thurst"] = thurst;
                        newr["thurst_mc"] = thurst_mc;
                        if (noise != "")
                        { newr["noise"] = noise.Substring(0, 2); }
                        newr["noise_mc"] = noise_mc;
                    }

                    //Add the row to the datatable
                    dtReplace.Rows.Add(newr);
                    dgvProductSerial.DataSource = dtReplace;
                    txtRep.Focus();
                    colorViewForFailAndBlank(ref dgvProductSerial);
                }
            }
        }
        private void frmReplace_Load(object sender, EventArgs e)
        {
            txtBeforeSerial.Focus();
            txtBeforeSerial.SelectAll();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (lblBoxID.Text == "BoxID:") return;

            Replace();
        }
        private void Replace()
        {
            string serial = txtAfterSerial.Text;
            string lot = dgvProductSerial["Lot", 0].Value.ToString();
            string line = dgvProductSerial["line", 0].Value.ToString();
            string thurst = dgvProductSerial["thurst", 0].Value.ToString();
            string noise = dgvProductSerial["noise", 0].Value.ToString();
            string thurst_mc = dgvProductSerial["thurst_mc", 0].Value.ToString();
            string noise_mc = dgvProductSerial["noise_mc", 0].Value.ToString();

            string sql1 = "UPDATE t_product_serial SET serialno = '" + serial + "', lot = '" + lot + "', line = '" + line + "', thurst = '" + thurst + "', noise = '" + noise + "', thurst_mc = '" + thurst_mc + "', noise_mc = '" + noise_mc + "' WHERE boxid = '" + boxID + "' AND serialno = '" + txtBeforeSerial.Text + "'";
            tf.sqlExecuteScalarString(sql1);
            dgvProductSerial.Rows.RemoveAt(0);
            txtAfterSerial.ResetText();
            txtBeforeSerial.ResetText();
            txtAfterSerial.Enabled = true;
            txtBeforeSerial.Enabled = true;
            txtBeforeSerial.Focus();
        }

        private void txtBeforeSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) txtAfterSerial.Focus();
        }

        private void txtRep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z) { Replace(); txtRep.ResetText(); }
        }
    }
}