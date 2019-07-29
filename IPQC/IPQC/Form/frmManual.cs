using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace IPQC
{
    public partial class frmManual : Form
    {
        //親フォームForm1へイベント発生を連絡する、デレゲート変数
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        //データグリッドビュー用ボタン
        DataGridViewButtonColumn Open;

        //その他、非ローカル変数
        double upp;
        double low;
        bool editMode;
        DataTable dtBuffer;
        DataTable dtHistory;
        DataTable dtUpLowIns;
        int clmSet = 5;
        int rowSet = 1;
        string instrument;
        string _ip;

        // コンストラクタ
        public frmManual()
        {
            InitializeComponent();
        }

        // ロード時の処理
        private void frmManual_Load(object sender, EventArgs e)
        {
            // 当フォームの表示場所を指定
            this.Left = 300;
            this.Top = 15;

            if (cmbLine.Text == "") btnMeasure.Enabled = false;

            //Exit app if user has been log in by another device
            TfSQL flag = new TfSQL();
            string ipadd = flag.sqlExecuteScalarString("select ip_address from qc_user where qcuser = '" + txtUser.Text + "'");
            bool expmiss = flag.sqlExecuteScalarBool("select export_permission from qc_user where qcuser = '" + txtUser.Text + "'");
            if (ipadd == "null") flag.sqlExecuteScalarString("UPDATE qc_user SET loginstatus=true, ip_address = '" + _ip + "' where qcuser = '" + txtUser.Text + "'");
            if (ipadd != "null" && ipadd != _ip)
            {
                DialogResult res = MessageBox.Show("User is logged in " + _ip + "," + System.Environment.NewLine +
                                    "Do you want to log out and log in again ?", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (res == DialogResult.OK) Application.Exit();
            }
            if (txtUser.Text != "Admin")
            {
                string[] a = txtUser.Text.Split('_');

                //User permission
                if (a[1] != "CHK" && txtUser.Text != "Admin")
                {
                    btnMeasure.Enabled = false;
                    btnRegister.Enabled = false;
                    btnDelete.Enabled = false;
                }
                if (expmiss == false && txtUser.Text != "Admin")
                {
                    btnExport.Enabled = false;
                }
            }

            // ＤＡＴＥＴＩＭＥＰＩＣＫＥＲを１０日前の日付にする
            dtpSet10daysBefore(dtpLotFrom);

            // ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの分以下を切り上げる
            dtpRoundUpHour(dtpLotTo);

            // ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの分以下を下げる
            dtpRounddownHour(dtpLotInput);

            

            // 各種処理用のテーブルを生成してデータを読み込む
            dtBuffer = new DataTable();
            defineBufferAndHistoryTable(ref dtBuffer);
            dtHistory = new DataTable(); 
            defineBufferAndHistoryTable(ref dtHistory);
            readDtHistory(ref dtHistory);
            dtUpLowIns = new DataTable();
            setLimitSetAndCommand(ref dtUpLowIns);

            
            // グリットビューの更新
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

            

            // グリットビュー右端にボタンを追加（初回のみ）
            addButtonsToDataGridView(dgvHistory);
        }
        public void DisableSortMode(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public void updateControls(string model, string process, string inspect, string user, string ip)
        {
            txtModel.Text = model;
            txtProcess.Text = process;
            txtInspect.Text = inspect;
            txtUser.Text = user;
            _ip = ip;
            string sql_line = "select line from tbl_model_line where model = '" + txtModel.Text + "' order by line";
            TfSQL ln = new TfSQL();
            ln.getComboBoxData(sql_line, ref cmbLine);
            //cmbLine.SelectedIndex = 0;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            readDtHistory(ref dtHistory);
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);
        }
        // サブプロシージャ：ＤＢからのＤＴＨＩＳＴＯＲＹへの読み込み
        private void readDtHistory(ref DataTable dt)
        {
            dt.Clear();

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            DateTime lotFrom = dtpLotFrom.Value;
            DateTime lotTo = dtpLotTo.Value;
            string line = cmbLine.Text;

            string sql = "select inspect, lot, inspectdate, line, qc_user, status, " +
                                "m1, m2, m3, m4, m5, x, r FROM tbl_measure_history " +
                         "WHERE model = '" + model + "' AND " +
                                "process = '" + process + "' AND " +
                                "inspect = '" + inspect + "' AND " +
                                "lot >= '" + lotFrom.ToString() + "' AND " +
                                "lot <= '" + lotTo.ToString() + "' AND " +
                                "line = '" + line + "' AND " +
                                "qc_user != '1. Upper' AND qc_user != '2. Lower' AND qc_user not like '%Point%' " +
                         "order by lot, inspectdate";

            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }
        private void readDtHistoryCust(ref DataTable dt)
        {
            dt.Clear();

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            DateTime lotFrom = dtpLotFrom.Value;
            DateTime lotTo = dtpLotTo.Value;
            string line = cmbLine.Text;

            string sql = "select inspect, lot, inspectdate, line, qc_user, status, " +
                                "m1, m2, m3, m4, m5, x, r FROM tbl_measure_history " +
                         "WHERE model = '" + model + "' AND " +
                                "process = '" + process + "' AND " +
                                "inspect = '" + inspect + "' AND " +
                                "lot >= '" + lotFrom.ToString() + "' AND " +
                                "lot <= '" + lotTo.ToString() + "' AND " +
                                "line = '" + line + "' AND qc_user != '1. Upper' AND qc_user != '2. Lower' AND " +
                                "qc_user not like '%Point%' AND status = '' " +
                         "order by lot, inspectdate";

            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }
        // サブプロシージャ：履歴テーブルの定義
        private void defineBufferAndHistoryTable(ref DataTable dt)
        {
            dt.Columns.Add("inspect", Type.GetType("System.String"));
            dt.Columns.Add("lot", Type.GetType("System.DateTime"));
            dt.Columns.Add("inspectdate", Type.GetType("System.DateTime"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("qc_user", Type.GetType("System.String"));
            dt.Columns.Add("status", Type.GetType("System.String"));
            dt.Columns.Add("m1", Type.GetType("System.Double"));
            dt.Columns.Add("m2", Type.GetType("System.Double"));
            dt.Columns.Add("m3", Type.GetType("System.Double"));
            dt.Columns.Add("m4", Type.GetType("System.Double"));
            dt.Columns.Add("m5", Type.GetType("System.Double"));
            dt.Columns.Add("x", Type.GetType("System.Double"));
            dt.Columns.Add("r", Type.GetType("System.Double"));
        }

        // サブプロシージャ：上限・下限、行セット・列セット、コマンド、の設定
        private void setLimitSetAndCommand(ref DataTable dt)
        {
            dt.Clear();
            string sql = "select upper, lower, clm_set, row_set, instrument from tbl_measure_item " +
                "where model = '" + txtModel.Text + "' and " +
                      "inspect = '" + txtInspect.Text + "' and process = '" + txtProcess.Text + "'";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);

            upp = (double)dt.Rows[0]["upper"];
            txtUsl.Text = upp.ToString();
            low = (double)dt.Rows[0]["lower"];
            txtLsl.Text = low.ToString();
            rowSet = (int)dt.Rows[0]["row_set"];
            clmSet = (int)dt.Rows[0]["clm_set"];
            instrument = (string)dt.Rows[0]["instrument"];
        }

        // サブプロシージャ：データグリットビューの更新
        private void updateDataGripViews(DataTable dt1, DataTable dt2, ref DataGridView dgv1, ref DataGridView dgv2)
        {

            // データグリットビューへＤＴＡＡＴＡＢＬＥを格納
            dgv1.DataSource = dt1;
            dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DisableSortMode(dgvBuffer);

            dgv2.DataSource = dt2;
            dgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DisableSortMode(dgvHistory);


            // スペック外のセルをマーキングする
            colorHistoryViewBySpec(dtHistory, ref dgvHistory);

            // 一番下の行を表示する
            if (dgv2.Rows.Count >= 1)
                dgv2.FirstDisplayedScrollingRowIndex = dgv2.Rows.Count - 1;
        }

        // 検索ボタン押下時、データを読み込み、ＤＡＴＡＧＲＩＤＶＩＥＷを更新する
       

        // サブサブプロシージャ：グリットビュー右端にボタンを追加
        private void addButtonsToDataGridView(DataGridView dgv)
        {
            bool adm_flag = false;
            TfSQL flag = new TfSQL();
            bool fl = flag.sqlExecuteScalarBool("select admin_flag from qc_user where qcuser = '" + txtUser.Text + "'");
            if (fl == true) adm_flag = true;
            Open = new DataGridViewButtonColumn();
            Open.Text = "Open";
            Open.UseColumnTextForButtonValue = true;
            Open.Width = 45;
            dgv.Columns.Add(Open);

            if (adm_flag == true)
            {
                btnDelete.Visible = true;
            }
        }

        // 測定値の新規登録
        private void btnMeasure_Click(object sender, EventArgs e)
        {
            // 編集モードフラグを下し、登録・修正ボタンを「登録」の表示にする
            editMode = false;
            btnRegister.Text = "Register";
            dtpLotInput.Enabled = true;

            // ＨＩＳＴＯＲＹデータグリッドビューのマーキングをクリアする
            colorViewReset(ref dgvHistory);
            colorViewReset(ref dgvBuffer);

            // 新規登録用バッファーテーブル、バッファーグリットビューを初期化する
            dtBuffer.Clear();

            // セット数分の行の設定（特殊アイテムは、変数 rowSet2 を使用する）
            int rowSet2;
            List<string> list1 = new List<string> { "BRAKET05", "CASE07" };
            bool specialItem = txtInspect.Text == "METALSLOP";
            if (list1.Contains(txtInspect.Text))
            {
                rowSet2 = 3;

                for (int i = 1; i <= rowSet2; i++)
                {
                    DataRow dr = dtBuffer.NewRow();
                    dr["inspect"] = txtInspect.Text;
                    dr["lot"] = dtpLotInput.Value;
                    dr["inspectdate"] = DateTime.Now;
                    dr["line"] = cmbLine.Text;
                    dr["qc_user"] = txtUser.Text;
                    dr["status"] = txtStatus.Text;

                    if (i == 1) dr["qc_user"] = "1. Upper";
                    else if (i == 2) dr["qc_user"] = "2. Lower";
                    else if (i == 3) dr["qc_user"] = txtUser.Text;

                    dtBuffer.Rows.Add(dr);
                }
            }
            else if (specialItem)
            {
                rowSet2 = 5;

                for (int i = 1; i <= rowSet2; i++)
                {
                    DataRow dr = dtBuffer.NewRow();
                    dr["inspect"] = txtInspect.Text;
                    dr["lot"] = dtpLotInput.Value;
                    dr["inspectdate"] = DateTime.Now;
                    dr["line"] = cmbLine.Text;
                    dr["qc_user"] = txtUser.Text;
                    dr["status"] = txtStatus.Text;
                    if (specialItem && i == 1) dr["qc_user"] = "Point 1";
                    else if (specialItem && i == 2) dr["qc_user"] = "Point 2";
                    else if (specialItem && i == 3) dr["qc_user"] = "Point 3";
                    else if (specialItem && i == 4) dr["qc_user"] = "Point 4";
                    else if (specialItem && i == 5) dr["qc_user"] = txtUser.Text;

                    dtBuffer.Rows.Add(dr);
                }
            }
            else
            {
                rowSet2 = rowSet;

                for (int i = 1; i <= rowSet2; i++)
                {
                    DataRow dr = dtBuffer.NewRow();
                    dr["inspect"] = txtInspect.Text;
                    dr["lot"] = dtpLotInput.Value;
                    dr["inspectdate"] = DateTime.Now;
                    dr["line"] = cmbLine.Text;
                    dr["qc_user"] = txtUser.Text;
                    dr["status"] = txtStatus.Text;
                    dtBuffer.Rows.Add(dr);
                }
            }
            // グリットビューの更新
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);
        }

        // 既存測定値の修正
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int curRow = int.Parse(e.RowIndex.ToString());

            if (dgvHistory.Columns[e.ColumnIndex] == Open && curRow >= 0)
            {
                // 編集モードフラグを立て、登録・修正ボタンを「修正」の表示にする
                editMode = true;
                btnRegister.Text = "Update";
                dtpLotInput.Enabled = false;

                // 新規登録用バッファーテーブル、バッファーグリットビューを初期化し、ボタンに対応する値を格納する
                dtBuffer.Clear();

                string sql = "select inspect, lot, inspectdate, line, qc_user, status, " +
                                    "m1, m2, m3, m4, m5 FROM tbl_measure_history WHERE " +
                             "model = '" + txtModel.Text + "' AND " +
                             "inspect = '" + dgvHistory["inspect", curRow].Value.ToString() + "' AND " +
                             "lot = '" + (DateTime)dgvHistory["lot", curRow].Value + "' AND " +
                             "inspectdate = '" + (DateTime)dgvHistory["inspectdate", curRow].Value + "' AND " +
                             "line = '" + dgvHistory["line", curRow].Value.ToString() + "' " +
                             "order by qc_user";
                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                tf.sqlDataAdapterFillDatatable(sql, ref dtBuffer);

                // グリットビューの更新
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // 変更ターゲット行を表示する
                if (dgvHistory.Rows.Count >= 1)
                    dgvHistory.FirstDisplayedScrollingRowIndex = curRow;

                // サブプロシージャ：編集中の行をマーキングする
                colorViewForEdit(ref dgvHistory, curRow);
                colorViewForEdit(ref dgvBuffer, 0);
            }
        }

        // サブプロシージャ：編集中の行をマーキングする
        private void colorViewForEdit(ref DataGridView dgv, int row)
        {
            if (dgv.Rows.Count == 0) return;

            int rowCount = dgv.RowCount;
            int clmCount = dgv.ColumnCount;
            DateTime inspectdate = (DateTime)dgv["inspectdate", row].Value;

            for (int i = 0; i < rowCount; ++i)
            {
                if ((DateTime)dgv["inspectdate", i].Value == inspectdate)
                {
                    for (int j = 0; j < clmCount; ++j)
                        dgv[j, i].Style.BackColor = Color.Yellow;
                }
                else
                {
                    for (int k = 0; k < clmCount; ++k)
                        dgv[k, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }
        }

        // サブプロシージャ：マーキングをクリアする
        private void colorViewReset(ref DataGridView dgv)
        {
            int rowCount = dgv.RowCount;
            int clmCount = dgv.ColumnCount;

            for (int i = 0; i < rowCount; ++i)
            {
                for (int k = 0; k < clmCount; ++k)
                    dgv[k, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
        }

        // サブプロシージャ：スペック外のセルをマーキングする（履歴テーブル）
        private void colorHistoryViewBySpec(DataTable dt, ref DataGridView dgv)
        {
            int rowCount = dgv.RowCount;
            int clmStart = 6;
            int clmEnd = 10;

            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = clmStart; j <= clmEnd; ++j)
                {
                    double m = 0;
                    bool b = double.TryParse(dt.Rows[i][j].ToString(), out m);
                    if (m >= low && m <= upp)
                        dgv[j, i].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    else
                    {
                        if (dgv[j, i].Value.ToString() != "")
                        {
                            dgv[j, i].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        // 編集完了時、スペック外のセルをマーキングする（一時テーブル）
        private void dgvBuffer_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            List<string> list2 = new List<string> { "BRAKET05", "CASE07" };
            if (txtInspect.Text == "METALSLOP")
            {
                if (dgv[e.ColumnIndex, 0].Value.ToString() != string.Empty &&
                        dgv[e.ColumnIndex, 1].Value.ToString() != string.Empty &&
                        dgv[e.ColumnIndex, 2].Value.ToString() != string.Empty &&
                        dgv[e.ColumnIndex, 3].Value.ToString() != string.Empty)
                {
                    double min, max;
                    int i;
                    double d1 = 0, d2 = 0, d3 = 0, d4 = 0, d5 = 0;
                    double.TryParse(dgv[e.ColumnIndex, 0].Value.ToString(), out d1);
                    double.TryParse(dgv[e.ColumnIndex, 1].Value.ToString(), out d2);
                    double.TryParse(dgv[e.ColumnIndex, 2].Value.ToString(), out d3);
                    double.TryParse(dgv[e.ColumnIndex, 3].Value.ToString(), out d4);
                    double[] mang = new double[] { d1, d2, d3, d4 };

                    min = max = mang[0];

                    for (i = 0; i <= 3; i++)
                    {
                        if (min > mang[i]) min = mang[i];
                        if (max < mang[i]) max = mang[i];
                    }
                    d5 = Math.Round(Math.Abs(max - min), 4);
                    dgv[e.ColumnIndex, 4].Value = d5;

                    if (d5 >= low && d5 <= upp)
                        dgv[e.ColumnIndex, 4].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    else
                        dgv[e.ColumnIndex, 4].Style.BackColor = Color.Red;
                }
            }
            else if (list2.Contains(txtInspect.Text))
            {
                if (dgv[e.ColumnIndex, 0].Value.ToString() != string.Empty &&
                        dgv[e.ColumnIndex, 1].Value.ToString() != string.Empty)
                {
                    double d1 = 0, d2 = 0, d3 = 0;
                    double.TryParse(dgv[e.ColumnIndex, 0].Value.ToString(), out d1);
                    double.TryParse(dgv[e.ColumnIndex, 1].Value.ToString(), out d2);

                    d3 = Math.Round(Math.Abs(d1 - d2), 4);
                    dgv[e.ColumnIndex, 2].Value = d3;

                    if (d3 >= low && d3 <= upp)
                        dgv[e.ColumnIndex, 2].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                    else
                        dgv[e.ColumnIndex, 2].Style.BackColor = Color.Red;
                }
            }
            else
            {
                double d = 0;
                double.TryParse(dgv[e.ColumnIndex, e.RowIndex].Value.ToString(), out d);

                if (d >= low && d <= upp)
                    dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                else
                    dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
            }
        }
            

        // 測定値の列のみ編集可能とする
        private void dgvBuffer_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            List<string> list2 = new List<string> { "BRAKET05", "CASE07" };
            //編集できるか判断する（特定の検査項目は、ＵＰＰＥＲ・ＬＯＷＥＲ・差異を入力、差異は直接入力不可）
            if (e.ColumnIndex <= 5 || e.ColumnIndex >= 11 || (txtInspect.Text == "METALSLOP" && e.RowIndex == 5) || (list2.Contains(txtInspect.Text) && e.RowIndex == 3))
            {
                //編集できないようにする
                e.Cancel = true;
            }
        }

        // 測定値の取り込みが終わったら、データベースへ登録する
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (dtBuffer.Rows.Count <= 0) return;

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            string status = txtStatus.Text;
            DateTime lot = DateTime.Parse(dtBuffer.Rows[0]["lot"].ToString());
            DateTime inspectdate = DateTime.Parse(dtBuffer.Rows[0]["inspectdate"].ToString());
            string line = cmbLine.Text;

            // ブァッファーテーブル内で、平均とレンジを計算する
            calculateAverageAndRangeInDataTable(ref dtBuffer);

            // ＩＰＱＣＤＢ 測定履歴テーブルに登録する
            TfSQL tf = new TfSQL();
            bool res = tf.sqlMultipleInsert(model, process, inspect, lot, inspectdate, line, status, dtBuffer);

            if (res)
            {
                // バックグラウンドでＰＱＭテーブルに登録する
                //DataTable dtTemp = new DataTable();
                //dtTemp = dtBuffer.Copy();
                //registerMeasurementToPqmTable(dtTemp);

                // 登録済の状態を、当フォームに表示する
                dtBuffer.Clear();
                readDtHistory(ref dtHistory);
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // 編集モードフラグを立て、登録・修正ボタンを「登録」の表示に戻す
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // サブプロシージャ: データテーブル内部で、平均とレンジ（最大－最小）を求め、格納する
        private void calculateAverageAndRangeInDataTable(ref DataTable dt)
        {
            if (dt.Rows.Count == 0) return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double[] ary = new double[5];
                double max = double.MinValue;
                double min = double.MaxValue;
                double sum = 0;
                double avg = 0;
                int cnt = 0;
                string idx = string.Empty;

                for (int j = 0; j < 5; j++)
                {
                    idx = "m" + (j + 1);
                    if (!string.IsNullOrEmpty(dt.Rows[i][idx].ToString()))
                    {
                        ary[j] = (double)dt.Rows[i][idx];
                        if (max < ary[j]) max = ary[j];
                        if (min > ary[j]) min = ary[j];
                        sum += ary[j];
                        cnt += 1;
                    }
                }
                avg = sum / cnt;
                dt.Rows[i]["x"] = Math.Round(avg, 4);
                dt.Rows[i]["r"] = Math.Abs(max - min);
            }
        }

        // サブプロシージャ: ＰＱＭテーブルへの登録（バックグラウンド処理）
        //private void registerMeasurementToPqmTable(DataTable dt)
        //{
        //    var task = Task.Factory.StartNew(() =>
        //    {
        //        string model = txtModel.Text;
        //        string process = txtProcess.Text;
        //        string inspect = txtInspect.Text;
        //        DateTime lot = DateTime.Parse(dt.Rows[0]["lot"].ToString());
        //        DateTime inspectdate = DateTime.Parse(dt.Rows[0]["inspectdate"].ToString());
        //        string line = cmbLine.Text;

        //        TfSql
                
                
        //        Tfc = new TfSqlPqm();
        //        Tfc.sqlMultipleInsertMeasurementToPqmTable(model, process, inspect, lot, inspectdate, line, dt, upp, low);
        //    }); 
        //}

        // 削除ボタン押下時の処理
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtBuffer.Rows.Count <= 0) return;

            DialogResult result = MessageBox.Show("Do you really want to delete the selected row?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                MessageBox.Show("Delete process was canceled.",
                "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }
            else if (result == DialogResult.Yes)
            {
                // データの削除
                string sql = "delete from tbl_measure_history where " +
                    "model='" + txtModel.Text + "' and " +
                    "inspect='" + txtInspect.Text + "' and " +
                    "lot ='" + dtBuffer.Rows[0]["lot"] + "' and " +
                    "inspectdate ='" + dtBuffer.Rows[0]["inspectdate"] + "' and " +
                    "line ='" + cmbLine.Text + "'";

                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                int res = tf.sqlExecuteNonQueryInt(sql, false);

                // バックグラウンドでＰＱＭテーブル内の削除
                DataTable dtTemp = new DataTable();
                dtTemp = dtBuffer.Copy();
               // deleteFromPqmTable(dtTemp);

                // 新規登録用バッファーテーブル、バッファーグリットビューを初期化する
                dtBuffer.Clear();
                
                // 削除後テーブルの再読み込み
                readDtHistory(ref dtHistory);

                // ＨＩＳＴＯＲＹデータグリッドビューのマーキングをクリアする
                colorViewReset(ref dgvHistory);

                // グリットビューの更新
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // 編集モードフラグを下し、登録・修正ボタンを「登録」の表示にする
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // サブプロシージャ: ＰＱＭテーブルでの削除（バックグラウンド処理）
        //private void deleteFromPqmTable(DataTable dt)
        //{
        //    var task = Task.Factory.StartNew(() =>
        //    {
        //        string model = txtModel.Text;
        //        string process = txtProcess.Text;
        //        string inspect = txtInspect.Text;
        //        DateTime lot = DateTime.Parse(dt.Rows[0]["lot"].ToString());
        //        DateTime inspectdate = DateTime.Parse(dt.Rows[0]["inspectdate"].ToString());
        //        string line = cmbLine.Text;

        //        TfSqlPqm Tfc = new TfSqlPqm();
        //        Tfc.sqlDeleteFromPqmTable(model, process, inspect, lot, inspectdate, line);
        //    });
        //}

        // キャンセルボタン押下時の処理
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // サブサブプロシージャ：ＤＡＴＥＴＩＭＥＰＩＣＫＥＲを１０日前の日付にする
        private void dtpSet10daysBefore(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value.Date.AddDays(-10);
            dtp.Value = dt;
        }

        // サブサブプロシージャ：ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの分以下を切り上げる
        private void dtpRoundUpHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddHours(1).AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // サブサブプロシージャ：ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの分以下を下げる
        private void dtpRounddownHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // ＸＲグラフ作成ボタン押下時の処理     
        private void btnExport_Click(object sender, EventArgs e)
        {
            //readDtHistoryCust(ref dtHistory);
            TfSQL sampl = new TfSQL();
            string sample = sampl.sqlExecuteScalarString("select clm_set from tbl_measure_item where inspect = '" + txtInspect.Text + "'");
            string descrip = sampl.sqlExecuteScalarString("select description from tbl_measure_item where inspect = '" + txtInspect.Text + "'");
            ExcelClassnew xl = new ExcelClassnew();

            string dtpFrom = dtpLotFrom.Value.ToString("yyyy/MM/dd");
            string dtpTo = dtpLotTo.Value.ToString("yyyy/MM/dd");

            xl.exportExcel(txtModel.Text, cmbLine.Text, txtUser.Text, txtUsl.Text, txtLsl.Text, txtProcess.Text, txtInspect.Text, sample, descrip, dgvHistory, dtpFrom, dtpTo);
        }

        // サブサブプロシージャ：ＸＲ管理図用データテーブルの生成      
        private DataTable returnXrChartData()
        {
            DataTable dt = new DataTable();
            dt = ((DataTable)dgvHistory.DataSource).Copy();
            dt.Columns.Add("llim", Type.GetType("System.Double"));
            dt.Columns.Add("ulim", Type.GetType("System.Double"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["llim"] = double.Parse(txtLsl.Text);
                dt.Rows[i]["ulim"] = double.Parse(txtUsl.Text);
            }

            return dt;
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
            //User permission
            if (VBStrings.Right(txtUser.Text, 3) != "CHK" && txtUser.Text != "Admin")
            {
                btnMeasure.Enabled = false;
            }
            else btnMeasure.Enabled = true;
        }
    }
}