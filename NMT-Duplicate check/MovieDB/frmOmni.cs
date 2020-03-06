using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Data.Linq;
using System.Globalization;
using System.Security.Permissions;
using System.Diagnostics;


namespace JigQuick
{
    public partial class frmOmni : Form
    {
        #region Variables
        // コンフィグファイルと、出力テキストファイルは、デスクトップの指定のフォルダに保存する
        string appconfig = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\info.ini";
        //string outPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\ConverterTarget\";
        //string outPath2 = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\NtrsLog\";

        //ＪＩＧＱＵＩＣＫ用、非ローカル変数
        bool sound;
        bool duplicate;

        //ＮＴＲＳ用、非ローカル変数
        int okCount;
        int ngCount;
        int targetProcessCount;
        string model;
        string targetProcessCombined;
        string headTableThisMonth = string.Empty;
        string headTableLastMonth = string.Empty;
        string headTablesubThisMonth = string.Empty;
        string headTablesubLastMonth = string.Empty;

        /// <summary>
        // application name that is given from caller application for displaying itself with version on login screen
        /// </summary>
        private string applicationName;
        #endregion

        public frmOmni(string applicationname)
        {
            applicationName = applicationname;

            InitializeComponent();
        }

        private void frmInut_Load(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------------------------
            // 以下、ＮＴＲＳの初期設定
            string standByImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\STANDBY.bmp";
            pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
            pnlResult.BackgroundImage = System.Drawing.Image.FromFile(standByImagePath);
            pnlRetest.BackgroundImageLayout = ImageLayout.Zoom;
            pnlRetest.BackgroundImage = System.Drawing.Image.FromFile(standByImagePath);

            TfSQL tf = new TfSQL();
            tf.getComboBoxData("select distinct model from t_serno", ref lblModel);
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append(applicationName + "_");
                version.Append("VERSION_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                version.Append(deploy.Build);
                version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            // バーコード末尾のエンターキー以外は処理しない
            if (e.KeyCode != Keys.Enter) return;

            //Set model, table
            //string mdl = VBS.Mid(txtProduct.Text, 3, 2);
            //switch (mdl)
            //{
            //    case "3E":
            //        lblModel.Text = "LS12_003E";
            //        break;
            //    case "3F":
            //        lblModel.Text = "LS12_003F";
            //        break;
            //    case "3D":
            //        lblModel.Text = "LS12_003D";
            //        break;
            //    case "3J":
            //        lblModel.Text = "LS12_003J";
            //        break;
            //    case "3K":
            //        lblModel.Text = "LS12_003K";
            //        break;
            //    case "4A":
            //        lblModel.Text = "LS12_004A";
            //        break;
            //    default:
            //        if (VBS.Left(txtProduct.Text, 1) == "M")
            //        {
            //            lblModel.Text = "LS12_003MOD";
            //        }
            //        else
            //        {
            //            string mdl1 = VBS.Mid(txtProduct.Text, 1, 2);
            //            switch (mdl1)
            //            {
            //                case "1C":
            //                case "1D":
            //                    lblModel.Text = "LD4";
            //                    break;
            //                case "01":
            //                    lblModel.Text = "LD25";
            //                    break;
            //                default:
            //                    lblModel.Text = "LAA10_003";
            //                    break;
            //            }
            //            if (VBS.Mid(txtProduct.Text, 6, 1) == "L") lblModel.Text = "LS12_003L";
            //        }
            //        break;
            //}

            model = lblModel.Text;
            headTableThisMonth = model.ToLower() + DateTime.Today.ToString("yyyyMM");
            headTableLastMonth = model.ToLower() + ((VBS.Right(DateTime.Today.ToString("yyyyMM"), 2) != "01") ?
                (long.Parse(DateTime.Today.ToString("yyyyMM")) - 1).ToString() : (long.Parse(DateTime.Today.ToString("yyyy")) - 1).ToString() + "12");
            TfSQL tf = new TfSQL();
            try
            {
                if (!tf.CheckTableExist(headTableLastMonth))
                {
                    headTableLastMonth = headTableThisMonth;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // 空文字の場合、または文字列の長さが誤っている場合は、処理しない
            if (txtProduct.Text == string.Empty) return;
            //if (txtChild.Text.Length != 17 && txtChild.Text.Length != 24) return;

            // テキストボックスが読み取り専用状態の場合は、処理しない
            if (txtProduct.ReadOnly == true) return;

            // 部品ロット情報が出そろっていない場合は、処理しない
            if (duplicate) return;

            // ＮＴＲＳの処理を先行して行い、その結果がＮＧの場合は、ＪＩＧＱＵＩＣＫのテキスト出力を行わない
            bool res = ntrsScanProcess(txtProduct.Text);
            if (!res) return;

            //Counter
            //switch (lblModel.Text)
            //{
            //    case "LS12_003E":
            //    case "LS12_003F":
            //    case "LS12_003D":
            //    case "LS12_003J":
            //    case "LS12_003K":
            //    case "LS12_004A":
            //    case "LS12_003MOD":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 13 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LS12_003L":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && VBS.Mid(txtProduct.Text, 6, 1) == "L" && txtProduct.TextLength == 10 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LD4":
            //    case "LD25":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 10 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LAA10_003":
            if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 8 && dup == 0)
            {
                lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            }
            //        break;
            //}
        }

        public int dup = 0;
        // Check Duplicate barcode
        private void checkDuplicate()
        {
            if (txtProduct.Text != String.Empty)// && txtProduct.Text.Length != 24)
            {
                DateTime d = DateTime.Now;
                //DateTime dd = DateTime.Now.GetDateTimeFormats("yyyy/mm/dd");
                TfSQL tf = new TfSQL();
                string ser = tf.sqlExecuteScalarString("SELECT serial_no FROM t_serno WHERE serial_no = '" + txtProduct.Text + "'");
                //Check retest data
                bool res1 = checkretest(txtProduct.Text);
                if (ser != txtProduct.Text && !res1)
                {
                    tf.sqlExecuteScalarString("INSERT INTO t_serno(serial_no, regist_date, model) VALUES('" + txtProduct.Text + "','" + d + "','" + lblModel.Text + "')");
                    lblResult.Text = "Barcode is OK"; lblResult.ForeColor = Color.Green;

                    string okImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\OK_BEAR.png";
                    dup = 0;
                    pnlRetest.BackgroundImageLayout = ImageLayout.Zoom;
                    pnlRetest.BackgroundImage = System.Drawing.Image.FromFile(okImagePath);

                    string date = DateTime.Today.AddDays(1).ToString("yyyy/MM/dd");
                    string date1 = DateTime.Today.ToString("yyyy/MM/dd");
                    string count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = '" + lblModel.Text.Replace("_", "-") + "'");
                    lblCount.Text = "TOTAL: " + count;
                }
                else if (ser == txtProduct.Text && !res1)
                {
                    lblResult.Text = "Duplicate Barcode";
                    lblResult.ForeColor = Color.Red;

                    //string ngImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\NG_BEAR.png";
                    string duplImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\DUPLICATE.png";
                    dup = 1;
                    //pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
                    //pnlResult.BackgroundImage = System.Drawing.Image.FromFile(ngImagePath);
                    pnlRetest.BackgroundImageLayout = ImageLayout.Zoom;
                    pnlRetest.BackgroundImage = System.Drawing.Image.FromFile(duplImagePath);
                    txtProduct.BackColor = Color.Red;
                    txtProduct.ReadOnly = true;
                    soundAlarm();
                }
                else
                {
                    string retestImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\RETEST.png";
                    pnlRetest.BackgroundImageLayout = ImageLayout.Zoom;
                    pnlRetest.BackgroundImage = System.Drawing.Image.FromFile(retestImagePath);
                    txtProduct.BackColor = Color.Red;
                    txtProduct.ReadOnly = true;
                    soundAlarm();
                }
            }
        }
        public string ser;
        //Check data retest
        public bool checkretest(string series)
        {
            TfSQL tf = new TfSQL();
            DataTable dt1 = new DataTable();
            string module1 = txtProduct.Text;
            string mdlShort1 = module1;

            switch (lblModel.Text)
            {
                case "LD4":
                    ser = "select process, judge, inspectdate from " +
               "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LD4-LVT' and serno = '" + mdlShort1 + "') union all " +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LD4-LVT' and serno = '" + mdlShort1 + "')" +
               ") d group by judge, process order by judge desc, process) b";
                    break;
                case "LD25":
                    ser = "select process, judge, inspectdate from " +
               "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LD25_LVT' and serno = '" + mdlShort1 + "') union all " +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LD25_LVT' and serno = '" + mdlShort1 + "')" +
               ") d group by judge, process order by judge desc, process) b";
                    break;
                case "LAA10_003":
                    ser = "select process, judge, inspectdate from " +
               "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LAA_LVT' and serno = '" + mdlShort1 + "') union all " +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LAA_LVT' and serno = '" + mdlShort1 + "')" +
               ") d group by judge, process order by judge desc, process) b";
                    break;
                default:
                    ser = "select process, judge, inspectdate from " +
               "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'EN2-LVT' and serno = '" + mdlShort1 + "') union all " +
               "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'EN2-LVT' and serno = '" + mdlShort1 + "')" +
               ") d group by judge, process order by judge desc, process) b";
                    break;
            }

            tf.sqlDataAdapterFillDatatableFromTesterDb(ser, ref dt1);
            //for (int i = 0; i < dt1.Rows.Count; i++)
            //{
            //    System.Diagnostics.Debug.Print(dt1.Rows[i][0].ToString() + " " + dt1.Rows[i][1].ToString() + " " + dt1.Rows[i][2].ToString());
            //}

            bool resu = false;
            if (dt1.Rows.Count >= 2 && dt1.Rows[0][1].ToString() == "FAIL")
            {
                string retestImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\RETEST.png";
                pnlRetest.BackgroundImageLayout = ImageLayout.Zoom;
                pnlRetest.BackgroundImage = System.Drawing.Image.FromFile(retestImagePath);

                resu = true;
            }

            return resu;
        }

        private void resetViewColor(ref DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
            duplicate = false;
        }

        private string aliasName = "MediaFile";
        private void soundAlarm()
        {
            string currentDir = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images";
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

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

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

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        private string makeSqlWhereClause(string criteria)
        {
            string sql = " where (";
            foreach (string c in criteria.Split(','))
            {
                sql += "process = " + c + " or ";
            }
            sql = VBS.Left(sql, sql.Length - 3) + ") ";
            System.Diagnostics.Debug.Print(sql);
            return sql;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string standByImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\STANDBY.bmp";
            pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
            pnlResult.BackgroundImage = System.Drawing.Image.FromFile(standByImagePath);
            pnlRetest.BackgroundImage = Image.FromFile(standByImagePath);

            lblResult.ResetText();
            txtResultDetail.ResetText();
            txtProduct.ResetText();
            txtProduct.ResetText();
            txtProduct.ReadOnly = false;
            txtProduct.BackColor = Color.White;
            txtProduct.BackColor = Color.White;
            txtProduct.Focus();
            pnlRetest.Visible = true;
        }

        public class TestResult
        {
            public string process { get; set; }
            public string judge { get; set; }
            public string inspectdate { get; set; }
        }

        public class ProcessList
        {
            public string process { get; set; }
        }

        public string sql1;

        private bool ntrsScanProcess(string id)
        {
            TfSQL tf = new TfSQL();
            DataTable dt = new DataTable();
            string log = string.Empty;
            string module = txtProduct.Text;
            string mdlShort = module;

            string scanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            switch (lblModel.Text)
            {
                case "LD4":
                    sql1 = "select process, judge, inspectdate from " +
                   "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LD4-LVT' and serno = '" + mdlShort + "') union all " +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LD4-LVT' and serno = '" + mdlShort + "')" +
                   ") d group by judge, process order by judge desc, process) b where flag = 1";
                    break;
                case "LD25":
                    sql1 = "select process, judge, inspectdate from " +
                   "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LD25_LVT' and serno = '" + mdlShort + "') union all " +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LD25_LVT' and serno = '" + mdlShort + "')" +
                   ") d group by judge, process order by judge desc, process) b where flag = 1";
                    break;
                case "LAA10_003":
                    sql1 = "select process, judge, inspectdate from " +
                   "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'LAA_LVT' and serno = '" + mdlShort + "') union all " +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'LAA_LVT' and serno = '" + mdlShort + "')" +
                   ") d group by judge, process order by judge desc, process) b where flag = 1";
                    break;
                default:
                    sql1 = "select process, judge, inspectdate from " +
                   "(select process, judge, max(inspectdate) as inspectdate, row_number() OVER (PARTITION BY process ORDER BY max(inspectdate) desc) as flag from (" +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableThisMonth + " where process = 'EN2-LVT' and serno = '" + mdlShort + "') union all " +
                   "(select process, case when tjudge = '0' then 'PASS' else 'FAIL' end as judge, inspectdate from " + headTableLastMonth + " where process = 'EN2-LVT' and serno = '" + mdlShort + "')" +
                   ") d group by judge, process order by judge desc, process) b where flag = 1";
                    break;
            }

            System.Diagnostics.Debug.Print(sql1);
            tf.sqlDataAdapterFillDatatableFromTesterDb(sql1, ref dt);
            txtResultDetail.ResetText();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                System.Diagnostics.Debug.Print(dt.Rows[i][0].ToString() + " " + dt.Rows[i][1].ToString() + " " + dt.Rows[i][2].ToString());

                txtResultDetail.Text = dt.Rows[i][1].ToString();
            }

            bool result = false;
            if (txtResultDetail.Text == "PASS")// && txtProduct.BackColor != Color.Red)
            {
                string okImagePass = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\OK_BEAR.png";
                pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
                pnlResult.BackgroundImage = System.Drawing.Image.FromFile(okImagePass);
                checkDuplicate();

                result = true;
                // 次のモジュールのスキャンにそなえ、スキャン用テキストボックスのテキストを選択し、上書き可能にする
                txtProduct.SelectAll();
            }
            else if (txtResultDetail.Text == "")
            {
                string noImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\NODATA.png";
                pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
                pnlResult.BackgroundImage = System.Drawing.Image.FromFile(noImagePath);
                pnlRetest.Visible = false;

                result = true;
                // 次のモジュールのスキャンにそなえ、スキャン用テキストボックスのテキストを選択し、上書き可能にする
                txtProduct.ReadOnly = true;
                txtProduct.BackColor = Color.Red;

                // アラームでの警告
                soundAlarm();
            }
            else
            {
                string ngImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\NG_BEAR.png";
                pnlResult.BackgroundImageLayout = ImageLayout.Zoom;
                pnlResult.BackgroundImage = System.Drawing.Image.FromFile(ngImagePath);
                checkDuplicate();

                result = true;
                // 次のモジュールのスキャンをストップするめた、スキャン用テキストボックスを無効にする
                txtProduct.ReadOnly = true;
                txtProduct.BackColor = Color.Red;

                // アラームでの警告
                soundAlarm();
            }
            return result;
        }

        public void transProduct(string serial)
        {
            txtProduct.Text = serial;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            TfSQL tf = new TfSQL();
            string date = dtpDate.Value.AddDays(1).ToString("yyyy/MM/dd");
            string date1 = dtpDate.Value.ToString("yyyy/MM/dd");
            string count;
            switch (lblModel.Text)
            {
                case "LS12_003E":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003E'");
                    break;
                case "LS12_003F":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003F'");
                    break;
                case "LS12_003D":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003D'");
                    break;
                case "LS12_003J":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003J'");
                    break;
                case "LS12_003K":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003K'");
                    break;
                case "LS12_003L":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-003L'");
                    break;
                case "LS12_004A":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-004A'");
                    break;
                case "LS12_003MOD":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LS12-004A'");
                    break;
                case "LD4":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LD4'");
                    break;
                case "LD25":
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' and model = 'LD25'");
                    break;
                default:
                    count = tf.sqlExecuteScalarString("select count(serial_no) from t_serno where regist_date > '" + date1 + "' and regist_date <= '" + date + "' model = 'LAA10-003'");
                    break;
            }

            lblSearch.Text = "COUNT: " + count;
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            //switch (lblModel.Text)
            //{
            //    case "LS12_003E":
            //    case "LS12_003F":
            //    case "LS12_003D":
            //    case "LS12_003J":
            //    case "LS12_003K":
            //    case "LS12_004A":
            //    case "LS12_003MOD":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 13 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LS12_003L":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && VBS.Mid(txtProduct.Text, 6, 1) == "L" && txtProduct.TextLength == 10 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LD4":
            //    case "LD25":
            //        if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 10 && dup == 0)
            //        {
            //            lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            //        }
            //        break;
            //    case "LAA10_003":
            if (!String.IsNullOrEmpty(txtProduct.Text) && txtProduct.TextLength == 8 && dup == 0)
            {
                lblCounter.Text = (int.Parse(lblCounter.Text) + 1).ToString();
            }
            //        break;
            //}
        }

        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            lblCounter.Text = "0";
            txtProduct.Focus();
        }

        private void CkbModel_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbModel.Checked) lblModel.Enabled = true;
            else lblModel.Enabled = false;
        }

        private void lblModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblModel.Text == "LAA10_003")
            {
                btnResetCounter.Visible = true;
                lblCounter.Visible = true;
            }
            else
            {
                btnResetCounter.Visible = false;
                lblCounter.Visible = false;
            }
        }
    }
}