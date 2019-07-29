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
        string testerTableThisMonth;
        string testerTableLastMonth;
        string boxID;
        ShSQL tf = new ShSQL();
        public frmReplace(string box)
        {
            InitializeComponent();
            lblBoxID.Text = "BoxID: " + box;
            boxID = box;
        }
        // Select datatable
        private string decideReferenceTable(string serno)
        {
            string tablekey = string.Empty;
            string filterkey = string.Empty;
            if (VBStrings.Mid(serno, 1, 2) == "1C" || VBStrings.Mid(serno, 1, 2) == "1D")
            { tablekey = "ld4"; filterkey = "LD4"; }
            else if (serno.Length == 8)
            { tablekey = "laa10_003"; filterkey = "LA10"; }
            else
            { tablekey = "ld25"; filterkey = "LD25"; }// ÉGÉâÅ[ëŒçÙ

            testerTableThisMonth = tablekey + DateTime.Today.ToString("yyyyMM");
            testerTableLastMonth = tablekey + ((VBStrings.Right(DateTime.Today.ToString("yyyyMM"), 2) != "01") ?
                (long.Parse(DateTime.Today.ToString("yyyyMM")) - 1).ToString() : (long.Parse(DateTime.Today.ToString("yyyy")) - 1).ToString() + "12");

            return filterkey;
        }
        // Sub procedure: Get module recors from database and set them into this form's datatable
        private void defineAndReadDtOverall(ref DataTable dt)
        {
            string boxId = lblBoxID.Text;

            dt.Columns.Add("id", Type.GetType("System.String"));
            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("config", Type.GetType("System.String"));
            dt.Columns.Add("process", Type.GetType("System.String"));
            dt.Columns.Add("linepass", Type.GetType("System.String"));
            dt.Columns.Add("testtime", Type.GetType("System.DateTime"));
        }
        // Sub procedure: Mark the test results with FAIL or missing test records
        private bool colorViewForFailAndBlank(ref DataGridView dgv)
        {
            int rowCount = dgv.BindingContext[dgv.DataSource, dgv.DataMember].Count;

            if (dgv["linepass", 0].Value.ToString() == "FAIL" || dgv["linepass", 0].Value.ToString() == String.Empty)
            {
                dgv["process", 0].Style.BackColor = Color.Red;
                dgv["linepass", 0].Style.BackColor = Color.Red;
                dgv["testtime", 0].Style.BackColor = Color.Red;
                return false;
            }
            else
            {
                dgv["process", 0].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                dgv["linepass", 0].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                dgv["testtime", 0].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                return true;
            }
        }
        private void txtAfterSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string serLong = txtAfterSerial.Text;
                string filterkey = decideReferenceTable(serLong);
                if (serLong != String.Empty)
                {
                    // Get the tester data from current month's table and store it in datatable
                    string sql = "SELECT serno, process, tjudge, inspectdate" +
                        " FROM " + testerTableThisMonth +
                        " WHERE serno = '" + serLong + "' order by tjudge, inspectdate desc limit 1";

                    DataTable dtReplace = new DataTable();
                    defineAndReadDtOverall(ref dtReplace);
                    DataTable dt1 = new DataTable();
                    tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                    // Get the tester data from last month's table and store it in the same datatable
                    sql = "SELECT serno, process, tjudge, inspectdate" +
                        " FROM " + testerTableLastMonth +
                        " WHERE serno = '" + serLong + "' order by tjudge, inspectdate desc limit 1";
                    tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                    // Even when no tester data is found, the module have to appear in the datagridview
                    DataRow newr = dtReplace.NewRow();

                    string lot = VBStrings.Mid(serLong, 3, 4);
                    string line = string.Empty;
                    string config = VBStrings.Mid(serLong, 1, 2);
                    string model = string.Empty;
                    if (VBStrings.Mid(serLong, 1, 2) == "1C" || VBStrings.Mid(serLong, 1, 2) == "1D")
                    { model = "LD04"; }
                    else if (serLong.Length == 8) model = "LA10";
                    else
                    { model = "LD25"; }

                    if (model == "LA10")
                    {
                        line = "1";
                        lot = VBStrings.Mid(serLong, 1, 4);
                    }
                    else
                    {
                        line = VBStrings.Mid(serLong, 7, 1);
                        lot = VBStrings.Mid(serLong, 3, 4);
                    }

                    // If tester data exists, show it in the datagridview
                    if (dt1.Rows.Count != 0)
                    {
                        string process = dt1.Rows[0][1].ToString();
                        string linepass = String.Empty;
                        string buff = dt1.Rows[0][2].ToString();
                        if (buff == "0") linepass = "PASS";
                        else if (buff == "1") linepass = "FAIL";
                        else linepass = "ERROR";
                        DateTime testtime = (DateTime)dt1.Rows[0][3];
                        newr["process"] = process;
                        newr["linepass"] = linepass;
                        newr["testtime"] = testtime;
                    }

                    newr["serialno"] = serLong;
                    newr["model"] = model;
                    newr["lot"] = lot;
                    newr["line"] = line;
                    newr["config"] = config;

                    //Add the row to the datatable
                    dtReplace.Rows.Add(newr);
                    dgvProductSerial.DataSource = dtReplace;
                    txtRep.Focus();
                    bool col = colorViewForFailAndBlank(ref dgvProductSerial);
                    if (col) { btnReplace.Enabled = true; txtAfterSerial.Enabled = false; txtBeforeSerial.Enabled = false; }
                    else { txtAfterSerial.Focus(); txtAfterSerial.SelectAll(); }
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
            string config = dgvProductSerial["colConfig", 0].Value.ToString();
            string judge = dgvProductSerial["linepass", 0].Value.ToString();
            string testtime = dgvProductSerial["testtime", 0].Value.ToString();

            string sql1 = "UPDATE product_serial SET serialno = '" + serial + "', lot = '" + lot + "', line = '" + line + "', config = '" + config + "', linepass = '" + judge + "', testtime = '" + testtime + "' WHERE boxid = '" + boxID + "' AND serialno = '" + txtBeforeSerial.Text + "'";
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