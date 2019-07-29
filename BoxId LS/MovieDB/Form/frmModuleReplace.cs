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
    public partial class frmModuleReplace : Form
    {
        //親フォームfrmBoxidへイベント発生を連絡（デレゲート）
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;
        DataTable dataTable;
        string testerTableThisMonth;
        string testerTableLastMonth;

        // コンストラクタ
        public frmModuleReplace()
        {
            InitializeComponent();
        }

        // ロード時の処理
        private void frmCapacity_Load(object sender, EventArgs e)
        {
            //フォームの場所を指定
            this.Left = 450;
            this.Top = 100;
            dataTable = new DataTable();
            defineDatatable(ref dataTable);
        }

        // サブプロシージャ: ＤＴの定義
        private void defineDatatable(ref DataTable dt)
        {
            dt.Columns.Add("serialno", Type.GetType("System.String"));
            dt.Columns.Add("model", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("fact", Type.GetType("System.String"));
            dt.Columns.Add("process", Type.GetType("System.String"));
            dt.Columns.Add("linepass", Type.GetType("System.String"));
            dt.Columns.Add("testtime", Type.GetType("System.DateTime"));
        }

        // サブプロシージャ：親フォームで呼び出し、親フォームの情報を、テキストボックスへ格納して引き継ぐ
        public void updateControls(string serno, int row)
        {
            txtBefore.Text = serno;
            txtRow.Text = row.ToString();
        }

        // 登録済のシリアルおよびその付帯情報を、ＵＰＤＡＴＥ文で置き換える
        private void btnReplace_Click(object sender, EventArgs e)
        {
            string snBefore = txtBefore.Text;
            string snAfter = txtAfter.Text;
            string filterkey = decideReferenceTable(snAfter);

            if (snAfter == String.Empty) return;

            setSerialInfoAndTesterResult(snAfter);

            string sql = "update product_serial set " +
                "serialno='" + dataTable.Rows[0]["serialno"] + "', " +
                "model='" + dataTable.Rows[0]["model"] + "', " +
                "lot='" + dataTable.Rows[0]["lot"] + "', " +
                "fact='" + dataTable.Rows[0]["fact"] + "', " +
                "process='" + dataTable.Rows[0]["process"] + "', " +
                "linepass='" + dataTable.Rows[0]["linepass"] + "', " +
                "testtime='" + dataTable.Rows[0]["testtime"] + "' " +
                "where serialno='" + txtBefore.Text + "'";

            System.Diagnostics.Debug.Print(sql);
            ShSQL tf = new ShSQL();
            bool res = tf.sqlExecuteNonQuery(sql, true);

            if (res)
            {
                //親フォームfrmBoxidのデータグリットビューを更新するため、デレゲートイベントを発生させる
                this.RefreshEvent(this, new EventArgs());
                Close();
            }
        }

        private string decideReferenceTable(string serno)
        {
            string tablekey = string.Empty;
            string filterkey = string.Empty;
            if (VBStrings.Mid(serno, 3, 2) == "3D")
            { tablekey = "ls12_003d"; filterkey = "LS3D"; }
            else if (VBStrings.Mid(serno, 3, 2) == "3E")
            { tablekey = "ls12_003e"; filterkey = "LS3E"; }
            else if (VBStrings.Mid(serno, 3, 2) == "3F")
            { tablekey = "ls12_003f"; filterkey = "LS3F"; }
            else if (VBStrings.Mid(serno, 3, 2) == "3J")
            { tablekey = "ls12_003j"; filterkey = "LS3J"; }
            else if (VBStrings.Mid(serno, 3, 2) == "4A")
            { tablekey = "ls12_004a"; filterkey = "LS4A"; }

            testerTableThisMonth = tablekey + DateTime.Today.ToString("yyyyMM");
            testerTableLastMonth = tablekey + ((VBStrings.Right(DateTime.Today.ToString("yyyyMM"), 2) != "01") ?
                (long.Parse(DateTime.Today.ToString("yyyyMM")) - 1).ToString() : (long.Parse(DateTime.Today.ToString("yyyy")) - 1).ToString() + "12");

            return filterkey;
        }
        // シリアル付帯情報を取得する
        private void setSerialInfoAndTesterResult(string serLong)
        {
            if (serLong != String.Empty)
            {
                // Get the tester data from current month's table and store it in datatable
                string sql = "SELECT serno, process, tjudge, inspectdate" +
                    " FROM " + testerTableThisMonth +
                    " WHERE serno = '" + serLong + "'";
                DataTable dt1 = new DataTable();
                ShSQL tf = new ShSQL();
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                System.Diagnostics.Debug.Print(sql);

                // Get the tester data from last month's table and store it in the same datatable
                sql = "SELECT serno, process, tjudge, inspectdate" +
                    " FROM " + testerTableLastMonth +
                    " WHERE serno = '" + serLong + "'";
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                System.Diagnostics.Debug.Print(sql);

                DataView dv = new DataView(dt1);
                dv.Sort = "tjudge desc, inspectdate";
                DataTable dt2 = dv.ToTable();

                // 一時テーブルへの登録準備
                string lot = VBStrings.Mid(serLong, 5, 3);
                string fact = VBStrings.Mid(serLong, 8, 1);
                string model = string.Empty;
                if (VBStrings.Mid(serLong, 3, 2) == "4A") model = "LS4A";
                else if (VBStrings.Mid(serLong, 3, 2) == "3D") model = "LS3D";
                else if (VBStrings.Mid(serLong, 3, 2) == "3E") model = "LS3E";
                else if (VBStrings.Mid(serLong, 3, 2) == "3F") model = "LS3F";
                else if (VBStrings.Mid(serLong, 3, 2) == "3J") model = "LS3J";
                else model = "Error";

                // テスターデータに該当がない場合でも、ユーザーに認識させるために表示する
                // また、本フォームのＤＡＴＡＴＡＢＬＥは、常に１件のみのデータ保持でよい
                dataTable.Clear();
                DataRow newrow = dataTable.NewRow();
                newrow["serialno"] = serLong;
                newrow["model"] = model;
                newrow["lot"] = lot;
                newrow["fact"] = fact;

                // テスターデータに該当がある場合、当然表示する
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

                // メモリ上のテーブルにレコードを追加
                dataTable.Rows.Add(newrow);
            }
        }

        // 他のフォームとの整合性を取るため、キャンセルボタンを設ける
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 閉じるボタンやショートカットでの終了を許さない
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }
    }
}