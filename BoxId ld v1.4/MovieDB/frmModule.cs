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
using Npgsql;
using System.Runtime.InteropServices;

namespace BoxIdDb
{
    public partial class frmModule : Form
    {
        // The delegate variable to signal the occurrance of delegate event to the parent form "formBoxid"
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // The variable for degignate the shared floder to save text files for printing, 
        // which is to be printed by separate printing application
        string appconfig = System.Environment.CurrentDirectory + "\\info.ini";
        string directory = @"C:\Users\mt-qc20\Desktop\Auto Print\\";

        // Other global variables
        bool formEditMode;
        string user;
        string config;
        string m_model;
        int okCount;
        bool inputBoxModeOriginal;
        string testerTableThisMonth;
        string testerTableLastMonth;
        DataTable dtOverall;
        DataTable dtOQC;
        int limit = 3150;
        int updateRowIndex;
        bool sound;

        // Constructor
        public frmModule()
        {
            InitializeComponent();
        }

        // Load event
        private void frmModule_Load(object sender, EventArgs e)
        {
            // Store user name to the variable
            user = txtUser.Text;

            // Show box capacity in the text box
            txtLimit.Text = limit.ToString();

            // Get the printing folder directory from the application setting file and store it to the variable
            directory = readIni("TARGET DIRECTORY", "DIR", appconfig);

            // Set this form's position on the screen
            this.Left = 350;
            this.Top = 30;

            // Generate datatbles to hold modules records
            dtOverall = new DataTable();
            defineAndReadDtOverall(ref dtOverall);
            dtOQC = new DataTable();
            defineDtNoise(ref dtOQC);


            updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);


            tabForGridview.SelectedTab = tpOQC;
            tabForGridview.SelectedTab = tpOverall;
        }

        // Sub procedure: Read ini file content
        private string readIni(string s, string k, string cfs)
        {
            StringBuilder retVal = new StringBuilder(255);
            string section = s;
            string key = k;
            string def = String.Empty;
            int size = 255;
            //get the value from the key in section
            int strref = GetPrivateProfileString(section, key, def, retVal, size, cfs);
            return retVal.ToString();
        }
        // Import Windows API
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        // Sub procedure: Transfer the parent from's information to this child form's objects
        public void updateControls(string boxId, DateTime printDate, string user, string serialNo, bool editMode)
        {
            txtBoxId.Text = boxId;
            dtpPrintDate.Value = printDate;
            txtUser.Text = user;
            txtProductSerial.Text = serialNo;

            txtProductSerial.Enabled = editMode;
            btnRegisterBoxId.Enabled = !editMode;
            btnDeleteAll.Visible = editMode;
            btnDeleteSelection.Visible = editMode;
            formEditMode = editMode;
            this.Text = editMode ? "Product Serial - Edit Mode" : "Product Serial - Browse Mode";
            btnRegisterBoxId.Text = editMode ? "Register Box ID" : "Re-print";
            if (user == "User_9")
            {
                btnChangeLimit.Visible = true;
                txtLimit.Visible = true;
            }
            if (!editMode && user == "User_9")
            {
                btnReplace.Visible = true;
                btnDeleteBoxId.Visible = true;
            }
        }

        // Sub procedure: Get module recors from database and set them into this form's datatable
        private void defineAndReadDtOverall(ref DataTable dt)
        {
            string boxId = txtBoxId.Text;

            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("config", Type.GetType("System.String"));
            dt.Columns.Add("process", Type.GetType("System.String"));
            dt.Columns.Add("linepass", Type.GetType("System.String"));
            dt.Columns.Add("testtime", Type.GetType("System.DateTime"));

            if (!formEditMode)
            {
                string sql = "select serialno, lot, line, config, stationid, linepass, testtime, model " +
                    "FROM product_serial WHERE boxid='" + boxId + "'";
                ShSQL tf = new ShSQL();
                tf.sqlDataAdapterFillDatatable(sql, ref dt);
            }
        }


        private void defineDtNoise(ref DataTable dt)
        {
            string boxId = txtBoxId.Text;

            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("config", Type.GetType("System.String"));
            dt.Columns.Add("process", Type.GetType("System.String"));
            dt.Columns.Add("linepass", Type.GetType("System.String"));
            dt.Columns.Add("testtime", Type.GetType("System.DateTime"));
        }

        // Sub procedure: Update datagridviews
        private void updateDataGripViews(DataTable dt1, DataTable dt2, ref DataGridView dgv1, ref DataGridView dgv2)
        {
            // Store the ENABLED status to the variable, then make the text boxs disenabled
            inputBoxModeOriginal = txtProductSerial.Enabled;
            txtProductSerial.Enabled = false;

            // Bind datatable to the datagridview
            updateDataGripViewsSub(dt1, dt2, ref dgv1, ref dgv2);
            
            // Mark the records with the test result FAIL or missing
            colorViewForFailAndBlank(ref dgv1);
            colorViewForOQC(ref dgv2);

            // Mark config with duplicate or character length error
            colorMixedConfig(dt1, ref dgv1);

            // Mark the records with duplicate product serial or the serial with not enough character length
            colorViewForDuplicateSerial(ref dgv1);

            // Show row number to the row header
            for (int i = 0; i < dgv1.Rows.Count; i++)
                dgv1.Rows[i].HeaderCell.Value = (i + 1).ToString();

            // Show row number to the row header
            for (int i = 0; i < dgv2.Rows.Count; i++)
                dgv2.Rows[i].HeaderCell.Value = (i + 1).ToString();

            // Adjust the width of the row header
            dgv1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // Adjust the width of the row header
            dgv2.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // Show the bottom of the datagridview
            if (dgv1.Rows.Count >= 1)
                dgv1.FirstDisplayedScrollingRowIndex = dgv1.Rows.Count - 1;

            // Show the bottom of the datagridview
            if (dgv2.Rows.Count >= 1)
                dgv2.FirstDisplayedScrollingRowIndex = dgv2.Rows.Count - 1;

            // Set the text box back to the original state
            txtProductSerial.Enabled = inputBoxModeOriginal;

            // Store the OK record count to the variable and show in the text box
            okCount = getOkCount(dt1);
            txtOkCount.Text = okCount.ToString();

            // If the OK record count has already reached to the capacity, disenable the scan text box
            if (okCount == limit)
            {
                txtProductSerial.Enabled = false;
            }
            else
            {
                txtProductSerial.Enabled = true;
            }

            // If the OK record coutn has already reached to the capacity, enable the register button
            if (okCount == limit && dgv1.Rows.Count == limit)
            {
                btnRegisterBoxId.Enabled = true;
            }
            else
            {
                btnRegisterBoxId.Enabled = false;
            }
        }

        // Sub procedure: Count the without-duplicate OK records
        private int getOkCount(DataTable dt)
        {
            if (dt.Rows.Count <= 0) return 0;
            DataTable distinct = dt.DefaultView.ToTable(true, new string[] { "serialno", "linepass" });
            //DataRow[] dr = distinct.Select("linepass = 'PASS' and noise = 'SPEC IN'");
            DataRow[] dr = distinct.Select("linepass = 'PASS'");
            int dist = dr.Length;
            return dist;
        }

        // Sub procedure: Bind main datatable to the datagridview and make summary datatables
        private void updateDataGripViewsSub(DataTable dt1, DataTable dt2, ref DataGridView dgv1, ref DataGridView dgv2)
        {
            dgv1.DataSource = dt1;
            dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgv2.DataSource = dt2;
            dgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] criteriaLine = { "1", "2", "3", "4", "Total" };
            makeDatatableSummary(dt1, ref dgvLine, criteriaLine, "line");

            string[] criteriaConfig = { "01", "02", "1C", "1D", "Total" };
            makeDatatableSummary(dt1, ref dgvConfig, criteriaConfig, "config");

            string[] criteriaPassFail = { "PASS", "FAIL", "No Data", "Total" };
            makeDatatableSummary(dt1, ref dgvPassFail, criteriaPassFail, "linepass");

            string[] criteriaDateCode = getLotArray(dt1);
            makeDatatableSummary(dt1, ref dgvDateCode, criteriaDateCode, "lot");
        }

        // Sub procedure: Make the summary datatables and bind them to the summary datagridviews
        public void makeDatatableSummary(DataTable dt0, ref DataGridView dgv, string[] criteria, string header)
        {
            DataTable dt1 = new DataTable();
            DataRow dr = dt1.NewRow();
            Int32 count;
            Int32 total = 0;
            string condition;

            for (int i = 0; i < criteria.Length; i++)
            {
                dt1.Columns.Add(criteria[i], typeof(Int32));
                condition = header + " = '" + criteria[i] + "'";
                count = dt0.Select(condition).Length;
                total += count;
                dr[criteria[i]] = (i != criteria.Length - 1 ? count : total);
                if (criteria[i] == "Total" && header == "linepass")
                {
                    dr[criteria[i]] = dgvProductSerial.Rows.Count;
                    dr[criteria[i - 1]] = dgvProductSerial.Rows.Count - total;
                }
            }
            dt1.Rows.Add(dr);

            dgv.Columns.Clear();
            dgv.DataSource = dt1;
            dgv.AllowUserToAddRows = false; // remove the null line
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // Sub procedure: Make lot summary
        private string[] getLotArray(DataTable dt0)
        {
            DataTable dt1 = dt0.Copy();
            DataView dv = dt1.DefaultView;
            dv.Sort = "lot";
            DataTable dt2 = dv.ToTable(true, "lot");
            string[] array = new string[dt2.Rows.Count + 1];
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                array[i] = dt2.Rows[i]["lot"].ToString();
            }
            array[dt2.Rows.Count] = "Total";
            return array;
        }

        // Sub procedure: Mark the test results with FAIL or missing test records
        private void colorViewForFailAndBlank(ref DataGridView dgv)
        {
            int rowCount = dgv.BindingContext[dgv.DataSource, dgv.DataMember].Count;
            for (int i = 0; i < rowCount; ++i)
            {
                if (dgv["linepass", i].Value.ToString() == "FAIL" || dgv["linepass", i].Value.ToString() == String.Empty)
                {
                    dgv["process", i].Style.BackColor = Color.Red;
                    dgv["linepass", i].Style.BackColor = Color.Red;
                    dgv["testtime", i].Style.BackColor = Color.Red;
                    soundAlarm();
                }
                else
                {
                    dgv["process", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    dgv["linepass", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    dgv["testtime", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        //
        private void colorViewForOQC(ref DataGridView dgv)
        {
            int rowCount = dgv.BindingContext[dgv.DataSource, dgv.DataMember].Count;
            for (int i = 0; i < rowCount; ++i)
            {
                // テスト結果のマーキング
                if (dgv["linepass", i].Value.ToString() == "FAIL" || dgv["linepass", i].Value.ToString() == string.Empty)
                {
                    dgv["process", i].Style.BackColor = Color.Red;
                    dgv["linepass", i].Style.BackColor = Color.Red;
                    dgv["testtime", i].Style.BackColor = Color.Red;
                    soundAlarm();
                    tabForGridview.SelectedTab = tpOQC;
                }
                else
                {
                    dgv["process", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    dgv["linepass", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    dgv["testtime", i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // Sub procesure: Mark product serials with duplicate or character length error
        private void colorViewForDuplicateSerial(ref DataGridView dgv)
        {
            DataTable dt = ((DataTable)dgv.DataSource).Copy();
            if (dt.Rows.Count <= 0) return;

            for (int i = 0; i < dtOverall.Rows.Count; i++)
            {
                string serial = dgv[0, i].Value.ToString();
                DataRow[] dr = dt.Select("serialno = '" + serial + "'");
                if (dr.Length >= 2 || dgv[0, i].Value.ToString().Length != 10)
                {
                    dgv[0, i].Style.BackColor = Color.Red;
                    soundAlarm();
                }
                else
                {
                    dgv[0, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // Sub procesure: Mark config with duplicate or character length error
        private void colorMixedConfig(DataTable dt, ref DataGridView dgv)
        {
            if (dt.Rows.Count <= 0) return;

            DataTable distinct = dt.DefaultView.ToTable(true, new string[] { "model" });
            if (distinct.Rows.Count == 1)
                m_model = distinct.Rows[0]["model"].ToString();

            if (distinct.Rows.Count >= 2)
            {
                string A = distinct.Rows[0]["model"].ToString();
                string B = distinct.Rows[1]["model"].ToString();
                int a = distinct.Select("model = '" + A + "'").Length;
                int b = distinct.Select("model = '" + B + "'").Length;

                // 件数の多いコンフィグを、この箱のメインモデルとする
                m_model = a > b ? A : B;

                // 件数の少ないほうのメインモデル文字を取得し、セル番地を特定してマークする
                string C = a < b ? A : B;
                int c = -1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["model"].ToString() == C) { c = i; }
                }

                if (c != -1)
                {
                    dgv["model", c].Style.BackColor = Color.Red;
                    soundAlarm();
                }
                else
                {
                    dgv.Columns["model"].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // Event when a module is scanned 
        private void txtProductSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Disenalbe the extbox to block scanning
                txtProductSerial.Enabled = false;

                string serLong = txtProductSerial.Text;
                string serShort = serLong;
                string filterkey = decideReferenceTable(serShort);
                if (serLong != String.Empty)
                {
                    // Get the tester data from current month's table and store it in datatable
                    string sql = "SELECT serno, process, tjudge, inspectdate" +
                        " FROM " + testerTableThisMonth +
                        " WHERE serno = '" + serShort + "'";
                    DataTable dt1 = new DataTable();
                    ShSQL tf = new ShSQL();
                    tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                    System.Diagnostics.Debug.Print(sql);

                    // Get the tester data from last month's table and store it in the same datatable
                    sql = "SELECT serno, process, tjudge, inspectdate" +
                        " FROM " + testerTableLastMonth +
                        " WHERE serno = '" + serShort + "'";
                    tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                    System.Diagnostics.Debug.Print(sql);

                    DataView dv = new DataView(dt1);
                    dv.RowFilter = "process like '%LD4-LVT'";
                    dv.Sort = "tjudge, inspectdate";
                    //printDataView(dv);
                    DataTable dt2 = dv.ToTable();

                    DataView dv2 = new DataView(dt1);
                    dv2.RowFilter = "process like '%LVT_OQC'";
                    dv2.Sort = "tjudge, inspectdate";
                    //printDataView(dv);
                    DataTable dt3 = dv2.ToTable();

                    //Take the values of serial no
                    string lot = VBStrings.Mid(serShort, 4, 4);
                    string line = VBStrings.Mid(serShort, 7, 1);
                    string config = VBStrings.Mid(serShort, 1, 2);
                    string model = string.Empty;
                    if (VBStrings.Mid(serLong, 1, 2) == "1C" || VBStrings.Mid(serLong, 1, 2) == "1D")
                    { model = "LD04"; }
                    else
                    { model = "LD25"; }

                    // Even when no tester data is found, the module have to appear in the datagridview
                    DataRow newrow = dtOverall.NewRow();
                    newrow["serialno"] = serLong;
                    newrow["model"] = model;
                    newrow["lot"] = lot;
                    newrow["line"] = line;
                    newrow["config"] = config;

                    // If tester data exists, show it in the datagridview
                    if (dt2.Rows.Count != 0)
                    {
                        string process = dt2.Rows[0][1].ToString();
                        string linepass = String.Empty;
                        string buff = dt2.Rows[0][2].ToString();
                        if (buff == "0") linepass = "PASS";
                        else if (buff == "1") linepass = "FAIL";
                        else linepass = "ERROR";
                        DateTime testtime = (DateTime)dt2.Rows[0][3];
                        newrow["process"] = process;
                        newrow["linepass"] = linepass;
                        newrow["testtime"] = testtime;
                    }

                    // Add the row to the datatable
                    dtOverall.Rows.Add(newrow);

                    //Take the values of serial no
                    string lot_oqc = VBStrings.Mid(serShort, 4, 4);
                    string line_oqc = VBStrings.Mid(serShort, 7, 1);
                    string config_oqc = VBStrings.Mid(serShort, 1, 2);
                    string model_oqc = string.Empty;
                    if (VBStrings.Mid(serLong, 1, 2) == "1C" || VBStrings.Mid(serLong, 1, 2) == "1D")
                    { model_oqc = "LD04"; }
                    else
                    { model_oqc = "LD25"; }

                    // Even when no tester data is found, the module have to appear in the datagridview
                    DataRow newrow_oqc = dtOQC.NewRow();
                    newrow_oqc["serialno"] = serLong;
                    newrow_oqc["model"] = model_oqc;
                    newrow_oqc["lot"] = lot_oqc;
                    newrow_oqc["line"] = line_oqc;
                    newrow_oqc["config"] = config_oqc;

                    // If tester data exists, show it in the datagridview
                    if (dt3.Rows.Count != 0)
                    {
                        string process_oqc = dt3.Rows[0][1].ToString();
                        string linepass_oqc = String.Empty;
                        string buff_oqc = dt3.Rows[0][2].ToString();
                        if (buff_oqc == "0") linepass_oqc = "PASS";
                        else if (buff_oqc == "1") linepass_oqc = "FAIL";
                        else linepass_oqc = "ERROR";
                        DateTime testtime_oqc = (DateTime)dt3.Rows[0][3];
                        newrow_oqc["process"] = process_oqc;
                        newrow_oqc["linepass"] = linepass_oqc;
                        newrow_oqc["testtime"] = testtime_oqc;
                    }

                    // Add the row to the datatable
                    dtOQC.Rows.Add(newrow_oqc);

                    // update
                    updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                }

                // For the operator to continue scanning, enable the scan text box and select the text in the box
                if (okCount >= limit)
                {
                    txtProductSerial.Enabled = false;
                }
                else
                {
                    txtProductSerial.Enabled = true;
                    txtProductSerial.Focus();
                    txtProductSerial.SelectAll();
                }
            }
        }

        // Select datatable
        private string decideReferenceTable(string serno)
        {
            string tablekey = string.Empty;
            string filterkey = string.Empty;
            if (VBStrings.Mid(serno, 1, 2) == "1C" || VBStrings.Mid(serno, 1, 2) == "1D")
            { tablekey = "ld4"; filterkey = "LD4"; }
            else
            { tablekey = "ld25"; filterkey = "LD25"; } // Multi model

            testerTableThisMonth = tablekey + DateTime.Today.ToString("yyyyMM");
            testerTableLastMonth = tablekey + ((VBStrings.Right(DateTime.Today.ToString("yyyyMM"), 2) != "01") ?
                (long.Parse(DateTime.Today.ToString("yyyyMM")) - 1).ToString() : (long.Parse(DateTime.Today.ToString("yyyy")) - 1).ToString() + "12");

            return filterkey;
        }

        // Issue new box id, register product serials, and save text file for barcode printing
        private void btnRegisterBoxId_Click(object sender, EventArgs e)
        {
            btnRegisterBoxId.Enabled = false;
            btnDeleteSelection.Enabled = false;
            btnDeleteAll.Enabled = false;
            btnCancel.Enabled = false;

            string boxId = txtBoxId.Text;

            // If this form mode is not for EDIT, this botton works for RE-PRINTING barcode lable
            if (!formEditMode)
            {
                // Take the 'config' to print label
                config = dtOverall.Rows[0]["config"].ToString();
                printBarcode(directory, boxId, config, m_model, dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);
                btnRegisterBoxId.Enabled = true;
                btnCancel.Enabled = true;
                return;
            }

            // Check if the product serials had already registered in the database table
            string checkResult = checkDataTableWithRealTable(dtOverall);

            if (checkResult != String.Empty)
            {
                MessageBox.Show("The following serials are already registered with box id:" + Environment.NewLine +
                    checkResult + Environment.NewLine + "Please check and delete.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                btnRegisterBoxId.Enabled = true;
                btnDeleteSelection.Enabled = true;
                btnDeleteAll.Enabled = true;
                btnCancel.Enabled = true;
                return;
            }

            // Issue new box id
            string boxIdNew = getNewBoxId();

            // As the first step, add new box id information to the product serial datatable
            DataTable dt = dtOverall.Copy();
            dt.Columns.Add("boxid", Type.GetType("System.String"));
            for (int i = 0; i < dt.Rows.Count; i++)
                dt.Rows[i]["boxid"] = boxIdNew;

            // As the second step, register datatables' each record into database table
            ShSQL tf = new ShSQL();
            bool res1 = tf.sqlMultipleInsertOverall(dt);
            bool res2 = true; //tf.sqlMultipleInsertNoise(dtSI);

            if (res1 & res2)
            {
                // Print barcode
                printBarcode(directory, boxIdNew, config, m_model, dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);
                // Clear the datatable
                dtOverall.Clear();
                dtOQC.Clear();
                dt = null;

                txtBoxId.Text = boxIdNew;
                dtpPrintDate.Value = DateTime.ParseExact(VBStrings.Mid(boxIdNew, 6, 6), "yyMMdd", CultureInfo.InvariantCulture);

                // Generate delegate event to update parant form frmBoxid's datagridview (box id list)
                this.RefreshEvent(this, new EventArgs());

                this.Focus();
                MessageBox.Show("The box id " + boxIdNew + " and " + Environment.NewLine +
                    "its product serials were registered.", "Process Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxId.Text = String.Empty;
                txtProductSerial.Text = String.Empty;
                updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                btnRegisterBoxId.Enabled = false;
                btnDeleteSelection.Enabled = false;
                btnDeleteAll.Enabled = false;
                btnCancel.Enabled = true;
            }
            else
            {
                MessageBox.Show("Box id and product serials were not registered." + System.Environment.NewLine +
                @"Please try again by clicking ""Register Box ID"".", "Process Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnRegisterBoxId.Enabled = true;
                btnDeleteSelection.Enabled = true;
                btnDeleteAll.Enabled = true;
                btnCancel.Enabled = true;
            }

        }

        // Sub procedure: Check if datatable's product serial is included in the database table
        // (actually, database itself blocks the duplicate, so this process is not needed)
        private string checkDataTableWithRealTable(DataTable dt1)
        {
            string result = String.Empty;

            string sql = "select serial_short, boxid " +
                 "FROM product_serial_printdate WHERE testtime BETWEEN '" + System.DateTime.Today.AddDays(-7) + "' AND '" + System.DateTime.Today.AddDays(1) + "'";

            DataTable dt2 = new DataTable();
            ShSQL tf = new ShSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt2);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string serial = VBStrings.Left(dt1.Rows[i]["serialno"].ToString(), 17);
                DataRow[] dr = dt2.Select("serial_short = '" + serial + "'");
                if (dr.Length >= 1)
                {
                    string boxid = dr[0]["boxId"].ToString();
                    result += (i + 1 + ": " + serial + " / " + boxid + Environment.NewLine);
                }
            }

            if (result == String.Empty)
            {
                return String.Empty;
            }
            else
            {
                return result;
            }
        }

        // Sub procedure: Issue new box id
        private string getNewBoxId()
        {
            m_model = dtOverall.Rows[0]["model"].ToString();
            string sql = "select MAX(boxid) FROM box_id where boxid like '" + m_model + "%'";
            System.Diagnostics.Debug.Print(sql);
            ShSQL yn = new ShSQL();
            string boxIdOld = yn.sqlExecuteScalarString(sql);

            DateTime dateOld = new DateTime(0);
            long numberOld = 0;
            string boxIdNew;

            if (boxIdOld != string.Empty)
            {
                dateOld = DateTime.ParseExact(VBStrings.Mid(boxIdOld, 6, 6), "yyMMdd", CultureInfo.InvariantCulture);
                numberOld = long.Parse(VBStrings.Right(boxIdOld, 3));
            }

            if (dateOld != DateTime.Today)
            {
                boxIdNew = m_model + "-" + DateTime.Today.ToString("yyMMdd") + "001";
            }
            else
            {
                boxIdNew = m_model + "-" + DateTime.Today.ToString("yyMMdd") + (numberOld + 1).ToString("000");
            }

            sql = "INSERT INTO box_id(" +
                "boxid," +
                "suser," +
                "printdate) " +
                "VALUES(" +
                "'" + boxIdNew + "'," +
                "'" + user + "'," +
                "'" + DateTime.Now.ToString() + "')";
            System.Diagnostics.Debug.Print(sql);
            yn.sqlExecuteNonQuery(sql, false);
            return boxIdNew;
        }

        // Sub procedure: Print barcode, by generating a text file in shared folder and let another application print it
        private void printBarcode(string dir, string id, string config, string m_model, DataGridView dgv1, ref DataGridView dgv2, ref TextBox txt)
        {
            ShPrint tf = new ShPrint();
            tf.createBoxidFiles(dir, id, config, m_model, dgv1, ref dgv2, ref txt);
        }

        // Delete records on datagridview selected by the user
        private void btnDeleteSelection_Click(object sender, EventArgs e)
        {
            // Alarm when it not check sample module to print
            if (dgvProductSerial.Columns.GetColumnCount(DataGridViewElementStates.Selected) >= 2)
            {
                MessageBox.Show("Please select range with only one columns.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                return;
            }

            DialogResult result = MessageBox.Show("Do you really want to delete the selected rows?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewCell cell in dgvProductSerial.SelectedCells)
                {
                    int i = cell.RowIndex;
                    dtOverall.Rows[i].Delete();
                    dtOQC.Rows[i].Delete();
                }
                dtOverall.AcceptChanges();
                updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                txtProductSerial.Focus();
            }
        }

        // Delete all records on datagridview, by the user's click on the delete all button
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            int rowCount = dgvProductSerial.Rows.Count;
            if (rowCount != 0)
            {
                DialogResult result = MessageBox.Show("Do you really want to delete all the record?",
                    "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    dtOverall.Clear();
                    dtOverall.AcceptChanges();
                    dtOQC.Clear();
                    dtOQC.AcceptChanges();
                    updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                    txtProductSerial.Focus();
                }
            }
        }

        // Change the capacity of the box (only for the super user)
        private void btnChangeLimit_Click(object sender, EventArgs e)
        {
            // Open frmCapacity with delegate event
            bool bl = ShGeneral.checkOpenFormExists("frmCapacity");
            if (bl)
            {
                MessageBox.Show("Please close or complete another form.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                frmCapacity f4 = new frmCapacity();
                // When the delegate event is triggered by child, update this form's datagridview
                f4.RefreshEvent += delegate (object sndr, EventArgs excp)
                {
                    limit = f4.getLimit();
                    txtLimit.Text = limit.ToString();
                    updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                    this.Focus();
                };

                f4.updateControls(limit.ToString());
                f4.Show();
            }
        }

        // Replace a product serial in already registered box id (only for super user)
        private void btnReplace_Click(object sender, EventArgs e)
        {
            // If more than 2 columns or more than 2 records are selected, leave the procedure
            if (dgvProductSerial.Columns.GetColumnCount(DataGridViewElementStates.Selected) >= 2 ||
                    dgvProductSerial.Rows.GetRowCount(DataGridViewElementStates.Selected) >= 2 ||
                    dgvProductSerial.CurrentCell.ColumnIndex != 0)
            {
                MessageBox.Show("Please select only one serial number.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                return;
            }

            // Open form frmModuleReplace with delegate function
            bool bl = ShGeneral.checkOpenFormExists("frmModuleReplace");
            if (bl)
            {
                MessageBox.Show("Please close or complete another form.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                return;
            }

            updateRowIndex = dgvProductSerial.CurrentCell.RowIndex;
            frmModuleReplace f2 = new frmModuleReplace();
            // Update this form's datagridview when child's delegate event is triggered
            f2.RefreshEvent += delegate (object sndr, EventArgs excp)
            {
                dtOverall.Clear();
                readDatatable(ref dtOverall);
                updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                dgvProductSerial.CurrentCell = dgvProductSerial[0, updateRowIndex];
                dgvProductSerial.FirstDisplayedScrollingRowIndex = updateRowIndex;
                this.Focus();
            };

            string curSerial = dgvProductSerial.CurrentCell.Value.ToString();
            int curRowIndex = dgvProductSerial.CurrentRow.Index;
            f2.updateControls(curSerial, curRowIndex + 1);
            f2.Show();
        }

        // Delete box is and its product module data(done by only the user user)
        private void btnDeleteBoxId_Click(object sender, EventArgs e)
        {
            // Ask 2 times to the user for check
            DialogResult result1 = MessageBox.Show("Do you really delete this box id's all the serial data?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result1 == DialogResult.Yes)
            {
                DialogResult result2 = MessageBox.Show("Are you really sure? Please select NO if you are not sure.",
                    "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result2 == DialogResult.Yes)
                {
                    string boxid = txtBoxId.Text;
                    string sql = "delete from product_serial where boxid = '" + boxid + "'";
                    ShSQL tf = new ShSQL();
                    tf.sqlExecuteNonQuery(sql, true);

                    dtOverall.Clear();
                    // Update datagridviw
                    updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                }
            }
        }

        // Sub procedure: Read product serial records from database to datatable
        private void readDatatable(ref DataTable dt)
        {
            string boxId = txtBoxId.Text;
            string sql = "select serialno, lot, line, stationid, linepass, testtime " +
                "FROM product_serial WHERE boxid='" + boxId + "'";
            ShSQL tf = new ShSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }

        // When cancel button is clicked, let the user check if it is OK that the records are deleted in add mode.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // When frmCapacity is remaining open, let the user close it
            string formName = "frmCapacity";
            bool bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl)
            {
                MessageBox.Show("You need to close another form before canceling.", "Notice",
                  MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            // When frmModuleReplace is remaining open, let the user close it
            formName = "frmModuleReplace";
            bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl)
            {
                MessageBox.Show("You need to close another form before canceling..", "Notice",
                  MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            // If there is no record in the datatable or the form is opened as for view, let the user close the form
            if (dtOverall.Rows.Count == 0 || !formEditMode)
            {
                Application.OpenForms["frmBoxid"].Focus();
                Close();
                return;
            }

            // Show alarm that all the temporary records in datatable will be completely deleted
            DialogResult result = MessageBox.Show("The current serial data has not been saved." + System.Environment.NewLine +
                "Do you rally cancel?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                dtOverall.Clear();
                updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
                MessageBox.Show("The temporary serial numbers are deleted.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                Application.OpenForms["frmBoxid"].Focus();
                Close();
            }
            else
            {
                return;
            }
        }


        // Do not allow user to close this form by right top close button or by Alt+F4 shor cut
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }

        private void dgvProductSerial_Sorted(object sender, EventArgs e)
        {
            updateDataGripViews(dtOverall, dtOQC, ref dgvProductSerial, ref dgvOQC);
        }

        // Play the MP3 file for alarming users
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,
           StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        private string aliasName = "MediaFile";

        private void soundAlarm()
        {
            string currentDir = System.Environment.CurrentDirectory;
            string fileName = currentDir + @"\warning.mp3";
            string cmd;

            if (sound)
            {
                cmd = "stop " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                cmd = "close " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                sound = false;
            }

            cmd = "open \"" + fileName + "\" type mpegvideo alias " + aliasName;
            if (mciSendString(cmd, null, 0, IntPtr.Zero) != 0) return;
            cmd = "play " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
            sound = true;
        }

        // Sub procedure: Check the records in dataview for debug
        private void printDataView(DataView dv)
        {
            foreach (DataRowView drv in dv)
            {
                System.Diagnostics.Debug.Print(drv["lot"].ToString() + ": " +
                    drv["tjudge"].ToString() + ": " + drv["inspectdate"].ToString());
            }
        }
        // Sub procedure: Check the records in datagrid view for debug
        private void printDataGridView(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < dgv.Columns.Count; j++)
                {
                    System.Diagnostics.Debug.Print(dgv[j, i].Value.ToString() + "  ");
                }
                System.Diagnostics.Debug.Print(Environment.NewLine);
            }

            System.Diagnostics.Debug.Print(Environment.NewLine);
            System.Diagnostics.Debug.Print(Environment.NewLine);
        }

        // Sub procedure: Check the records in data table for debug
        private void printDataTable(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    System.Diagnostics.Debug.Print(dt.Rows[i][j].ToString() + "  ");
                }
                System.Diagnostics.Debug.Print(Environment.NewLine);
            }

            System.Diagnostics.Debug.Print(Environment.NewLine);
            System.Diagnostics.Debug.Print(Environment.NewLine);
        }
    }
}