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
            dt.Columns.Add("lot", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("config1", Type.GetType("System.String"));
            dt.Columns.Add("config2", Type.GetType("System.String"));
            dt.Columns.Add("stationid", Type.GetType("System.String"));
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

            if (snAfter == String.Empty) return;

            setSerialInfoAndTesterResult(snAfter);

            string sql = "update product_serial set " +
                "serialno='" + dataTable.Rows[0]["serialno"] + "', " +
                "lot='" + dataTable.Rows[0]["lot"] + "', " +
                "line='" + dataTable.Rows[0]["line"] + "', " +
                "config1='" + dataTable.Rows[0]["config1"] + "', " +
                "config2='" + dataTable.Rows[0]["config2"] + "', " +
                "stationid='" + dataTable.Rows[0]["stationid"] + "', " +
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

        // シリアル付帯情報を取得する
        private void setSerialInfoAndTesterResult(string serLong)
        {
            if (serLong != String.Empty)
            {
                // テスターデータの当月テーブルから、テスト結果等を取得する
                string sql = "SELECT serno, lot, tjudge, inspectdate" +
                    " FROM " + "tester_data" +
                    " WHERE serno = '" + serLong + "'" +
                    " ORDER BY tjudge DESC,inspectdate ASC";
                DataTable dt1 = new DataTable();
                ShSQL tf = new ShSQL();
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                // テスターデータの前月テーブルから、テスト結果等を取得し、同じＤＡＴＡＴＡＢＬＥへレコードを追加する
                sql = "SELECT serno, lot, tjudge, inspectdate" +
                    " FROM " + "tester_data" +
                    " WHERE serno = '" + serLong + "'" +
                    " ORDER BY tjudge DESC,inspectdate ASC";
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                DataView dv = new DataView(dt1);
                dv.Sort = "tjudge desc, inspectdate";
                DataTable dt2 = dv.ToTable();

                // 一時テーブルへの登録準備
                string lot = VBStrings.Mid(serLong, 4, 5);
                string line = VBStrings.Mid(serLong, 8, 1);
                string config1 = VBStrings.Mid(serLong, 12, 4);
                string config2 = VBStrings.Mid(serLong, 16, 1);

                // テスターデータに該当がない場合でも、ユーザーに認識させるために表示する
                // また、本フォームのＤＡＴＡＴＡＢＬＥは、常に１件のみのデータ保持でよい
                dataTable.Clear();
                DataRow newrow = dataTable.NewRow();
                newrow["serialno"] = serLong;
                newrow["lot"] = lot;
                newrow["line"] = line;
                newrow["config1"] = config1;
                newrow["config2"] = config2;

                // テスターデータに該当がある場合、当然表示する
                if (dt2.Rows.Count != 0)
                {
                    string stationid = dt2.Rows[0][1].ToString();
                    string linepass = dt2.Rows[0][2].ToString();
                    DateTime testtime = (DateTime)dt2.Rows[0][3];

                    newrow["stationid"] = stationid;
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