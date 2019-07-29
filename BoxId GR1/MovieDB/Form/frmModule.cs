using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Permissions;
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
        //string directory = @"Z:\(01)Motor\(00)Public\11-Suka-Sugawara\LD model\printer\print\";
        string directory = @"\\192.168.145.7\ncvp\print\";
        // Other global variables
        bool formEditMode;
        string m_model;
        int okCount;
        bool inputBoxModeOriginal;
        DataTable dtOverall;
        public int limit1 = 0;
        int limit = 350;

        // Constructor
        public frmModule()
        {
            InitializeComponent();
        }

        // Load event
        private void frmModule_Load(object sender, EventArgs e)
        {
            // Store user name to the variable
            //user = txtUser.Text;

            txtLimit.Text = limit.ToString();

            // Get the printing folder directory from the application setting file and store it to the variable
            //directory = @"Z:\(01)Motor\(00)Public\11-Suka-Sugawara\LD model\printer\print\";
            //directory = @"Z:\(01)KK03\QA\(00)Public\03 OQC\02. LS\print\";
            // Set this form's position on the screen
            this.Left = 350;
            this.Top = 30;

            // Generate datatbles to hold modules records
            dtOverall = new DataTable();
            defineAndReadDtOverall(ref dtOverall);
            updateDataGripViews(dtOverall, ref dgvProductSerial);
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
        public void updateControls(string boxId, DateTime printDate, string user, bool editMode)
        {
            txtBoxId.Text = boxId;
            dtpPrintDate.Value = printDate;
            txtUser.Text = user;
            if (!String.IsNullOrEmpty(user)) { txtUser.Enabled = false; }
            else txtUser.Enabled = true;

            txtProductSerial.Enabled = editMode;
            btnPrint.Enabled = !editMode;
            btnRegisterBoxId.Enabled = !editMode;
            btnDeleteAll.Visible = editMode;
            btnDeleteSelection.Visible = editMode;
            formEditMode = editMode;
            this.Text = editMode ? "Product Serial - Edit Mode" : "Product Serial - Browse Mode";
            //btnRegisterBoxId.Enabled = editMode;
            btnRegisterBoxId.Text = editMode ? "Register Box ID" : "Re-Print";
        }

        // Sub procedure: Get module recors from database and set them into this form's datatable
        private void defineAndReadDtOverall(ref DataTable dt)
        {
            string boxId = txtBoxId.Text;

            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("thurst", Type.GetType("System.String"));
            dt.Columns.Add("thurst_mc", Type.GetType("System.String"));
            dt.Columns.Add("noise", Type.GetType("System.String"));
            dt.Columns.Add("noise_mc", Type.GetType("System.String"));

            if (!formEditMode)
            {
                string sql = "select serialno, model, lot, line, thurst, thurst_mc, noise, noise_mc  " +
                    "FROM t_product_serial WHERE boxid='" + boxId + "' order by serialno";
                ShSQL tf = new ShSQL();
                tf.sqlDataAdapterFillDatatable(sql, ref dt);
            }
        }

        // Sub procedure: Update datagridviews
        private void updateDataGripViews(DataTable dt1, ref DataGridView dgv1)
        {
            // Store the ENABLED status to the variable, then make the text boxs disenabled
            inputBoxModeOriginal = txtProductSerial.Enabled;
            txtProductSerial.Enabled = true;

            // Bind datatable to the datagridview
            updateDataGripViewsSub(dt1, ref dgv1);

            // Mark the records with the test result FAIL or missing
            colorViewForFailAndBlank(ref dgv1);

            // Mark config with duplicate or character length error
            //colorMixedConfig(dt1, ref dgv1);

            // Mark the records with duplicate product serial or the serial with not enough character length
            colorViewForDuplicateSerial(ref dgv1);

            // Show row number to the row header
            for (int i = 0; i < dgv1.Rows.Count; i++)
                dgv1.Rows[i].HeaderCell.Value = (i + 1).ToString();

            // Adjust the width of the row header
            dgv1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // Show the bottom of the datagridview
            if (dgv1.Rows.Count >= 1)
                dgv1.FirstDisplayedScrollingRowIndex = dgv1.Rows.Count - 1;

            // Set the text box back to the original state
            txtProductSerial.Enabled = inputBoxModeOriginal;

            // Store the OK record count to the variable and show in the text box
            okCount = getOkCount(dt1);
            txtOkCount.Text = okCount.ToString();

            // If the OK record count has already reached to the capacity, disenable the scan text box
            if (okCount == limit)
                txtProductSerial.Enabled = false;
            else
                txtProductSerial.Enabled = true;

            // If the OK record coutn has already reached to the capacity, enable the register button
            if (okCount == limit && dgv1.Rows.Count == limit)
                btnRegisterBoxId.Enabled = true;
            else
                btnRegisterBoxId.Enabled = false;

        }

        // Sub procedure: Count the without-duplicate OK records
        private int getOkCount(DataTable dt)
        {
            if (dt.Rows.Count <= 0) return 0;
            DataTable distinct = dt.DefaultView.ToTable(true, new string[] { "serialno", "thurst", "noise" });
            DataRow[] dr = distinct.Select("thurst = 'OK' and noise = 'OK'");
            int dist = dr.Length;
            return dist;
        }

        // Sub procedure: Bind main datatable to the datagridview and make summary datatables
        private void updateDataGripViewsSub(DataTable dt1, ref DataGridView dgv1)
        {
            dgv1.DataSource = dt1;
            dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] criteriaLine = { "LRP", "L01", "L02", "L03", "L04", "L05", "L06", "L07", "Total" };
            makeDatatableSummary(dt1, ref dgvLine, criteriaLine, "line");

            //string[] criteriaPassFail = { "OK", "NG", "No Data", "Total" };
            //makeDatatableSummary(dt1, ref dgvPassFail, criteriaPassFail, "linepass");

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
                //if (criteria[i] == "Total" && header == "lot")
                //{
                //    dr[criteria[i]] = dgvProductSerial.Rows.Count;
                //    dr[criteria[i - 1]] = dgvProductSerial.Rows.Count - total;
                //}
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

        // Sub procesure: Mark product serials with duplicate or character length error
        private void colorViewForDuplicateSerial(ref DataGridView dgv)
        {
            DataTable dt = ((DataTable)dgv.DataSource).Copy();
            if (dt.Rows.Count <= 0) return;

            for (int i = 0; i < dtOverall.Rows.Count; i++)
            {
                string serial = dgv[0, i].Value.ToString();
                DataRow[] dr = dt.Select("serialno = '" + serial + "'");
                if (dr.Length >= 2)
                {
                    dgv[0, i].Style.BackColor = Color.Red;
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
                }
                else
                {
                    dgv.Columns["model"].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // Event when a module is scanned 
        string line;
        private void txtProductSerial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    // Disenalbe the textbox to block scanning
                    txtProductSerial.Enabled = false;

                    string serLong = txtProductSerial.Text;
                    string serShort = serLong;
                    
                    if (serLong != String.Empty)
                    {
                        // Get the tester data from current month's table and store it in datatable
                        //string sql = "select a90_barcode, a90_line as line, a90_thurst_status as thurst, a90_factory as thurst_mc, noise, eq_id as noise_mc from thurst_noise where a90_barcode = '" + serShort + "'";
                        string sql = "select a.a90_barcode, a90_line as line, a90_thurst_status as thurst, a90_factory as thurst_mc, b.judgment AS noise, b.eq_id as noise_mc from (select row_number() over(partition by a90_barcode order by oid desc) thurtid, a90_barcode, a90_model, a90_line, a90_thurst_status, a90_factory from t_checkpusha90 where a90_barcode = '" + serShort + "') a full join (select row_number() over(partition by barcode order by noise_id desc) noiseid,barcode, judgment, eq_id from t_noisecheck_a90 where barcode = '" + serShort + "') b on a.a90_barcode = b.barcode where thurtid = 1 OR noiseid = 1";
                        DataTable dt1 = new DataTable();
                        ShSQL tf = new ShSQL();
                        tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                        System.Diagnostics.Debug.Print(sql);

                        string lot = string.Empty;

                        lot = VBStrings.Mid(serShort, 1, 5);

                        // Even when no tester data is found, the module have to appear in the datagridview
                        DataRow newrow = dtOverall.NewRow();
                        newrow["serialno"] = serLong;
                        newrow["model"] = "LPD_5SG";
                        newrow["lot"] = lot;

                        // If tester data exists, show it in the datagridview
                        if (dt1.Rows.Count != 0)
                        {
                             //= dt1.Rows[0][1].ToString();
                            switch (VBStrings.Right(lot,1))
                            {
                                case "C":
                                case "D":
                                    line = "L01";
                                    break;
                                case "E":
                                case "F":
                                    line = "L02";
                                    break;
                                case "G":
                                case "H":
                                    line = "L03";
                                    break;
                                case "I":
                                case "K":
                                    line = "L04";
                                    break;
                                default:
                                    line = "ERROR";
                                    break;
                            }
                            string thurst = dt1.Rows[0][2].ToString();
                            string thurst_mc = dt1.Rows[0][3].ToString();
                            string noise = dt1.Rows[0][4].ToString();
                            string noise_mc = dt1.Rows[0][5].ToString();

                            newrow["line"] = line;
                            newrow["thurst"] = thurst;
                            newrow["thurst_mc"] = thurst_mc;
                            if (noise != "")
                            { newrow["noise"] = noise.Substring(0, 2); }
                            newrow["noise_mc"] = noise_mc;
                        }

                        // Add the row to the datatable
                        dtOverall.Rows.Add(newrow);

                        updateDataGripViews(dtOverall, ref dgvProductSerial);
                        CountLot();
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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string boxId = txtBoxId.Text;
            if (!formEditMode)
            {
                //if (okCount == limit && dgvProductSerial.Rows.Count == limit)
                //{
                //config = dtOverall.Rows[0]["config"].ToString();
                printBarcode(directory, boxId, "LPD5SG-039", dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);
                //}
            }
            else
            {
                string boxIdNew = getNewBoxId();
                if (okCount == limit && dgvProductSerial.Rows.Count == limit)
                {
                    // Print barcode
                    printBarcode(directory, boxIdNew, "LPD5SG-039", dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);

                    // Clear the datatable
                    dtOverall.Clear();

                    txtBoxId.Text = boxIdNew;
                    dtpPrintDate.Value = DateTime.ParseExact(VBStrings.Mid(boxIdNew, 5, 6), "yyMMdd", CultureInfo.InvariantCulture);
                }
            }
        }
        // Issue new box id, register product serials, and save text file for barcode printing
        private void btnRegisterBoxId_Click(object sender, EventArgs e)
        {
            btnRegisterBoxId.Enabled = false;

            if (String.IsNullOrEmpty(txtUser.Text))
            {
                MessageBox.Show("Please input your name in User textbox before saving this boxID!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string boxId = txtBoxId.Text;

            // If this form's mode is not for EDIT, this botton works for RE-PRINTING barcode lable
            if (!formEditMode)
            {
                printBarcode(directory, boxId, "LPD5SG-039", dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);
                btnRegisterBoxId.Enabled = true;
                btnDeleteSelection.Enabled = false;
                btnDeleteAll.Enabled = false;
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
            bool res2 = true;

            if (res1 & res2)
            {
                tf.sqlExecuteNonQuery("insert into t_box_id(boxid, printdate, user_cd, child_model) VALUES('" + boxIdNew + "','" + DateTime.Now + "','" + txtUser.Text + "','LPD5SG-039')", false);
                // Print barcode
                printBarcode(directory, boxIdNew, "LPD5SG-039", dgvDateCode, ref dgvDateCode2, ref txtBoxIdPrint);
                // Clear the datatable
                dtOverall.Clear();
                dt = null;

                txtBoxId.Text = boxIdNew;
                dtpPrintDate.Value = DateTime.ParseExact(VBStrings.Mid(boxIdNew, 5, 6), "yyMMdd", CultureInfo.InvariantCulture);

                // Generate delegate event to update parant form frmBoxid's datagridview (box id list)
                this.RefreshEvent(this, new EventArgs());

                this.Focus();
                MessageBox.Show("The box id " + boxIdNew + " and " + Environment.NewLine +
                    "its product serials were registered.", "Process Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxId.Text = String.Empty;
                txtProductSerial.Text = String.Empty;
                updateDataGripViews(dtOverall, ref dgvProductSerial);
                btnRegisterBoxId.Enabled = false;
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
                 "FROM product_serial_printdate WHERE printdate BETWEEN '" + System.DateTime.Today.AddDays(-30) + "' AND '" + System.DateTime.Today.AddDays(1) + "'";

            DataTable dt2 = new DataTable();
            ShSQL tf = new ShSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt2);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string serial = dt1.Rows[i]["serialno"].ToString();
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
            string sql = "select MAX(boxid) FROM t_box_id";
            System.Diagnostics.Debug.Print(sql);
            ShSQL yn = new ShSQL();
            string boxIdOld = yn.sqlExecuteScalarString(sql);

            DateTime dateOld = new DateTime(0);
            long numberOld = 0;
            string boxIdNew;

            if (boxIdOld != string.Empty)
            {
                dateOld = DateTime.ParseExact(VBStrings.Mid(boxIdOld, 5, 6), "yyMMdd", CultureInfo.InvariantCulture);
                numberOld = long.Parse(VBStrings.Right(boxIdOld, 3));
            }

            if (dateOld != DateTime.Today)
            {
                boxIdNew = "GR1" + "-" + DateTime.Today.ToString("yyMMdd") + "001";
            }
            else
            {
                boxIdNew = "GR1" + "-" + DateTime.Today.ToString("yyMMdd") + (numberOld + 1).ToString("000");
            }

            return boxIdNew;
        }

        // Sub procedure: Print barcode, by generating a text file in shared folder and let another application print it
        private void printBarcode(string dir, string id, string m_model, DataGridView dgv1, ref DataGridView dgv2, ref TextBox txt)
        {
            ShPrint tf = new ShPrint();
            tf.createBoxidFiles(dir, id, m_model, dgv1, ref dgv2, ref txt);
        }

        public string check;
        // Delete records on datagridview selected by the user
        private void btnDeleteSelection_Click(object sender, EventArgs e)
        {
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
                }
                dtOverall.AcceptChanges();
                updateDataGripViews(dtOverall, ref dgvProductSerial);
                txtProductSerial.Focus();
                CountLot();
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
                    CountLot();
                    updateDataGripViews(dtOverall, ref dgvProductSerial);
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
                    updateDataGripViews(dtOverall, ref dgvProductSerial);
                    this.Focus();
                };

                f4.updateControls(limit.ToString());
                f4.Show();
            }
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
                    string sql = "delete from t_product_serial where boxid = '" + boxid + "'";
                    string sql1 = "delete from t_box_id where boxid = '" + boxid + "'";
                    ShSQL tf = new ShSQL();
                    tf.sqlExecuteNonQuery(sql, true);
                    tf.sqlExecuteNonQuery(sql1, true);

                    dtOverall.Clear();
                    // Update datagridviw
                    updateDataGripViews(dtOverall, ref dgvProductSerial);
                }
            }
        }

        public void CountLot()
        {
            lblLotNum.Text = dgvDateCode.ColumnCount.ToString();
            if (dgvDateCode.ColumnCount > 35) { MessageBox.Show("The quantity of lot is more than 35. You can not print the shipping label!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); lblLotNum.BackColor = Color.Red; }
            else lblLotNum.BackColor = Color.LightGreen;
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
                "Do you really cancel?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                dtOverall.Clear();
                updateDataGripViews(dtOverall, ref dgvProductSerial);
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

        private void btnReplaceSerial_Click(object sender, EventArgs e)
        {
            btnRegisterBoxId.Enabled = false;
            new frmReplace(txtBoxId.Text).ShowDialog();
        }

        private void ckbDeleteBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbDeleteBox.Checked) btnDeleteBoxId.Visible = false;
            else btnDeleteBoxId.Visible = true;
        }
    }
}