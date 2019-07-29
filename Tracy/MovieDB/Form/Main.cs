using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Permissions;
using Npgsql;
using System.Globalization;

namespace Tracy
{
    public partial class Main : Form
    {
        //親フォームForm5へ、イベント発生を連絡（デレゲート）
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // 非ローカル変数
        DataTable dtBatchNo;
        bool formAdmin;
        bool formAdminUser;

        // コンストラクタ
        public Main()
        {
            InitializeComponent();
        }

        // ロード時の処理
        private void Main_Load(object sender, EventArgs e)
        {
            //フォームの場所を指定
            this.Left = 10;
            this.Top = 10;
            dtBatchNo = new DataTable();
            defineAndReadDatatable(ref dtBatchNo);
            updateDataGridViews(ref dtBatchNo, ref dgvBatchNo);

            // コンボボックスへ候補をセットする（モデルＮＯ）
            string sql = "select model_no FROM t_model_sub_assy group by model_no order by model_no";
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmbModelNo);

            //string permiss = tf.sqlExecuteScalarString("select permission from t_leader_id where leader_id = '" + txtLeaderId.Text + "'");

            //if (permiss == "approve")
            //{
            //    dgvBatchNo.ReadOnly = false;
            //    //dgvBatchNo.Columns[0].ReadOnly = false;
            //    dgvBatchNo.Columns[1].ReadOnly = true;
            //    dgvBatchNo.Columns[2].ReadOnly = true;
            //    dgvBatchNo.Columns[3].ReadOnly = true;
            //    dgvBatchNo.Columns[4].ReadOnly = true;
            //    dgvBatchNo.Columns[5].ReadOnly = true;
            //    dgvBatchNo.Columns[6].ReadOnly = true;
            //    dgvBatchNo.Columns[7].ReadOnly = true;
            //    dgvBatchNo.Columns[8].ReadOnly = true;
            //    dgvBatchNo.Columns[9].ReadOnly = true;
            //    dgvBatchNo.Columns[10].ReadOnly = true;
            //    dgvBatchNo.Columns[11].ReadOnly = true;
            //    dgvBatchNo.Columns[12].ReadOnly = true;
            //    dgvBatchNo.Columns[13].ReadOnly = true;
            //    dgvBatchNo.Columns[14].ReadOnly = true;
            //    dgvBatchNo.Columns[15].ReadOnly = true;
            //    dgvBatchNo.Columns[0].Visible = true;
            //}
            //else if (permiss == "check" || permiss == "user")
            //{
            //    dgvBatchNo.Columns[0].Visible = false;
            //    btnApprove.Visible = false;
            //    ckbAll.Visible = false;
            //}

            for (int i = 0; i < dgvBatchNo.Rows.Count; i++)
            {
                if (dgvBatchNo.Rows[i].Cells[1].Value.ToString() != "")
                {
                    dgvBatchNo.Rows[i].Cells[0].Value = true;
                    dgvBatchNo.Rows[i].Cells[0].ReadOnly = true;
                }
                else
                {
                    dgvBatchNo.Rows[i].Cells[0].Value = false;
                }

                if (dgvBatchNo.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dgvBatchNo.Rows[i].Cells[0].ReadOnly = true;
                }
            }
        }
        // コンボボックス項目選択時の処理（モデルＮＯ）
        private void cmbModelNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql;
            TfSQL tf = new TfSQL();

            string model = cmbModelNo.Text;
            sql = "select model_name FROM t_model_sub_assy where model_no ='" + model + "'";
            System.Diagnostics.Debug.Print(sql);
            txtModelName.Text = tf.sqlExecuteScalarString(sql);

            // コンボボックスへ候補をセットする（サブ組ＮＯ）
            sql = "select sub_assy_no FROM t_model_sub_assy where model_no ='" + model + "'";
            System.Diagnostics.Debug.Print(sql);
            tf.getComboBoxData(sql, ref cmbSubAssyNo);
            cmbSubAssyNo.Enabled = true;

            // コンボボックスへ候補をセットする（ライン）
            sql = "select line FROM t_model_line where model_no ='" + model + "'";
            System.Diagnostics.Debug.Print(sql);
            tf.getComboBoxData(sql, ref cmbLine);
            cmbLine.Enabled = true;
        }

        // コンボボックス項目選択時の処理（サブ組ＮＯ）
        private void cmbSubAssyNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string subAssy = cmbSubAssyNo.Text;
            string sql = "select sub_assy_name FROM t_model_sub_assy where sub_assy_no ='" + subAssy + "'";
            TfSQL tf = new TfSQL();
            txtSubAssyName.Text = tf.sqlExecuteScalarString(sql);
        }

        // サブプロシージャ：データグリットビューの更新。親フォームで呼び出し、親フォームの情報を引き継ぐ
        public void updateControls(string leaderId, string leaderName, bool adminuser)
        {
            txtLeaderId.Text = leaderId;
            txtLeaderName.Text = leaderName;
            formAdminUser = adminuser;
        }

        // サブプロシージャ：データテーブルの定義
        private void defineAndReadDatatable(ref DataTable dt)
        {
            //dt.Columns.Add("    Approve", Type.GetType("System.Boolean"));
            //dt.Columns.Add("approve_by", Type.GetType("System.String"));
            //dt.Columns.Add("check_by", Type.GetType("System.String"));
            dt.Columns.Add("batch_no", Type.GetType("System.String"));
            dt.Columns.Add("model_no", Type.GetType("System.String"));
            dt.Columns.Add("model_name", Type.GetType("System.String"));
            dt.Columns.Add("sub_assy_no", Type.GetType("System.String"));
            dt.Columns.Add("sub_assy_name", Type.GetType("System.String"));
            dt.Columns.Add("batch_date", Type.GetType("System.DateTime"));
            dt.Columns.Add("shift", Type.GetType("System.String"));
            dt.Columns.Add("line", Type.GetType("System.String"));
            dt.Columns.Add("leader_id", Type.GetType("System.String"));
            dt.Columns.Add("leader_name", Type.GetType("System.String"));
            dt.Columns.Add("in_qty", Type.GetType("System.Double"));
            dt.Columns.Add("out_qty", Type.GetType("System.Double"));
            dt.Columns.Add("in_time", Type.GetType("System.DateTime"));
            dt.Columns.Add("out_time", Type.GetType("System.DateTime"));
            dt.Columns.Add("remark", Type.GetType("System.String"));
        }

        // サブプロシージャ：データグリットビューの更新
        public void updateDataGridViews(ref DataTable dt, ref DataGridView dgv)
        {
            string batchNo = txtBatchNo.Text;
            string modelNo = cmbModelNo.Text;
            string modelName = txtModelName.Text;
            string subAssyNo = cmbSubAssyNo.Text;
            string subAssyName = txtSubAssyName.Text;
            DateTime batchDate = dtpBatchDate.Value.Date;
            DateTime batchNextDate = dtpBatchDate.Value.Date.AddDays(1);
            string shift = cmbShift.Text;
            string line = cmbLine.Text;
            string leader = txtLeaderId.Text;
            string leaderName = txtLeaderName.Text;
            bool b_batch = chkBatch.Checked;
            bool b_model = chkModel.Checked;
            bool b_subAssy = chkSubAssy.Checked;
            bool b_batchDate = chkBatchDate.Checked;
            bool b_shift = chkShift.Checked;
            bool b_line = chkLine.Checked;
            bool b_leader = chkLeader.Checked;



            //string sql1 = "select approve_by, check_by, batch_no, model_no, model_name, sub_assy_no, sub_assy_name, batch_date, shift, line, leader_id, leader_name, in_qty, out_qty, in_time, out_time, remark from t_batch_no where ";

            string sql1 = "select batch_no, model_no, model_name, sub_assy_no, sub_assy_name, batch_date, shift, line, leader_id, leader_name, in_qty, out_qty, in_time, out_time, remark from t_batch_no where ";

            bool[] cr = { batchNo   == String.Empty ? false : true,
                          modelNo   == String.Empty ? false : true,
                          subAssyNo == String.Empty ? false : true,
                                                              true,
                          shift     == String.Empty ? false : true,
                          line      == String.Empty ? false : true,
                          leader    == String.Empty ? false : true};

            bool[] ck = { b_batch,
                          b_model,
                          b_subAssy,
                          b_batchDate,
                          b_shift,
                          b_line,
                          b_leader};

            string sql2 = (!(cr[0] && ck[0]) ? String.Empty : "batch_no like '" + batchNo + "%' AND ") +
                          (!(cr[1] && ck[1]) ? String.Empty : "model_no = '" + modelNo + "' AND ") +
                          (!(cr[2] && ck[2]) ? String.Empty : "sub_assy_no = '" + subAssyNo + "' AND ") +
                          (!(cr[3] && ck[3]) ? String.Empty : "batch_date >= '" + batchDate + "' AND batch_date < '" + batchNextDate + "' AND ") +
                          (!(cr[4] && ck[4]) ? String.Empty : "shift = '" + shift + "' AND ") +
                          (!(cr[5] && ck[5]) ? String.Empty : "line = '" + line + "' AND ") +
                          (!(cr[6] && ck[6]) ? String.Empty : "leader_id = '" + leader + "' AND ");

            bool b_all = (cr[0] && ck[0]) || (cr[1] && ck[1]) || (cr[2] && ck[2]) || (cr[3] && ck[3]) || (cr[4] && ck[4]) ||
                         (cr[5] && ck[5]) || (cr[6] && ck[6]);

            System.Diagnostics.Debug.Print(b_all.ToString());
            System.Diagnostics.Debug.Print(cr[0].ToString() + " " + ck[0].ToString() + " " + cr[1].ToString() + " " + ck[1].ToString() + " " +
                cr[2].ToString() + " " + ck[2].ToString() + " " + cr[3].ToString() + " " + ck[3].ToString() + " " +
                cr[4].ToString() + " " + ck[4].ToString() + cr[5].ToString() + " " + ck[5].ToString() + " " + cr[6].ToString() + " " + ck[6].ToString());

            if (!b_all)
            {
                MessageBox.Show("Please select at least one check box and fill the criteria.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            string sql3 = sql1 + VBStrings.Left(sql2, sql2.Length - 5);
            System.Diagnostics.Debug.Print(sql3);

            dt.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql3, ref dt);

            // データグリットビューへＤＴＡＡＴＡＢＬＥを格納
            dgv.DataSource = dt;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //行ヘッダーに行番号を表示する
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();

                if (dgv.Rows[i].Cells[1].Value.ToString() != "")
                {
                    dgv.Rows[i].Cells[0].Value = true;
                    dgv.Rows[i].Cells[0].ReadOnly = true;
                }
                else
                {
                    dgv.Rows[i].Cells[0].Value = false;
                }

                if (dgv.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dgv.Rows[i].Cells[0].ReadOnly = true;
                }
            }
            //行ヘッダーの幅を自動調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // 一番下の行を表示する
            if (dgv.Rows.Count != 0)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
        }

        // フォーム３を更新モードで開く
        private void btnEditBatch_Click(object sender, EventArgs e)
        {
            // セルの選択行がない場合は、プロシージャを抜ける
            int curRow = dgvBatchNo.CurrentCell.RowIndex;
            if (curRow < 0) return;

            //既にForm3 が開かれている場合は、閉じるように促す
            if (TfGeneral.checkOpenFormExists("Form3"))
            {
                MessageBox.Show("You need to close another already open window.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            string batchNo = dtBatchNo.Rows[curRow]["batch_no"].ToString();
            string modelNo = dtBatchNo.Rows[curRow]["model_no"].ToString();
            string modelName = dtBatchNo.Rows[curRow]["model_name"].ToString();
            string subAssyNo = dtBatchNo.Rows[curRow]["sub_assy_no"].ToString();
            string subAssyName = dtBatchNo.Rows[curRow]["sub_assy_name"].ToString();
            DateTime batchDate = DateTime.Parse(dtBatchNo.Rows[curRow]["batch_date"].ToString());
            string shift = dtBatchNo.Rows[curRow]["shift"].ToString();
            string line = dtBatchNo.Rows[curRow]["line"].ToString();
            string leader = dtBatchNo.Rows[curRow]["leader_id"].ToString();
            string leaderName = dtBatchNo.Rows[curRow]["leader_name"].ToString();
            string input = dtBatchNo.Rows[curRow]["in_qty"].ToString();
            string output = dtBatchNo.Rows[curRow]["out_qty"].ToString();
            DateTime inputTime = DateTime.Parse(dtBatchNo.Rows[curRow]["in_time"].ToString());
            DateTime outputTime = DateTime.Parse(dtBatchNo.Rows[curRow]["out_time"].ToString());
            string remark = dtBatchNo.Rows[curRow]["remark"].ToString();

            // ログイン時のリーダーアカウントと、選択したレコードのリーダーが一致すれば、管理モード（全てのレコードを編集可能）
            if (txtLeaderId.Text == leader)
                formAdmin = true;
            else if (txtLeaderId.Text == leader)
                formAdmin = false;

            // 管理ユーザーの場合に限り、常に管理モード
            if (formAdminUser)
                formAdmin = true;

            Input f3 = new Input();
            //子イベントをキャッチして、データグリッドを更新する
            f3.RefreshEvent += delegate (object sndr, EventArgs excp)
            {
                updateDataGridViews(ref dtBatchNo, ref dgvBatchNo);
            };

            f3.updateControls(batchNo, modelNo, modelName, subAssyNo, subAssyName, batchDate, shift, line, leader, leaderName, input, output, inputTime, outputTime, remark, false, formAdmin);
            f3.truyenbien(txtLeaderId.Text, txtLeaderName.Text);
            f3.Show();
            f3.btnDelete();
        }

        // フォーム３を追加モードで開く
        private void btnAddBatch_Click(object sender, EventArgs e)
        {
            //既にForm3 が開かれている場合は、閉じるように促す
            if (TfGeneral.checkOpenFormExists("Form3"))
            {
                MessageBox.Show("You need to close another already open window.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            string batchNo = string.Empty;
            string modelNo = cmbModelNo.Text;
            string modelName = txtModelName.Text;
            string subAssyNo = cmbSubAssyNo.Text;
            string subAssyName = txtSubAssyName.Text;
            DateTime batchDate = DateTime.Now;
            string shift = cmbShift.Text;
            string line = cmbLine.Text;
            string leader = txtLeaderId.Text;
            string leaderName = txtLeaderName.Text;
            string input = string.Empty;
            string output = string.Empty;
            DateTime inputTime = dtvRounddownSeconds(DateTime.Now);
            DateTime outputTime = inputTime;
            string remark = string.Empty;

            bool check = (modelNo != string.Empty && modelName != string.Empty && subAssyNo != string.Empty &&
                subAssyName != string.Empty && shift != string.Empty && line != string.Empty);

            if (!check)
            {
                MessageBox.Show("You need to fill the following items before adding new batch no:" + System.Environment.NewLine +
                    "Model, SubAssy, Shift, Line", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }

            // 追加モードの場合は、ログイン時のリーダーアカウントに関係なく、管理モード（全てのレコードを編集可能）
            formAdmin = true;

            Input f3 = new Input();
            //子イベントをキャッチして、データグリッドを更新する

            f3.RefreshEvent += delegate (object sndr, EventArgs excp)
            {
                updateDataGridViews(ref dtBatchNo, ref dgvBatchNo);
            };

            f3.updateControls(batchNo, modelNo, modelName, subAssyNo, subAssyName, batchDate, shift, line, leader, leaderName, input, output, inputTime, outputTime, remark, true, formAdmin);
            f3.Show();
        }

        // 検索ボタン押下、実際はグリットビューの更新をするだけ
        private void btnSearchBoxId_Click(object sender, EventArgs e)
        {
            updateDataGridViews(ref dtBatchNo, ref dgvBatchNo);
        }

        //Form1を閉じる際、非表示になっている親フォームForm5を閉じる
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //親フォームForm5を閉じるよう、デレゲートイベントを発生させる
            this.RefreshEvent(this, new EventArgs());
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

        // フォーム３が開かれていないことを確認してから、閉じる
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string formName = "Form3";
            bool bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl)
            {
                MessageBox.Show("You need to close Form Parts Lot and Operator first.", "Notice",
                  MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }
            Close();
        }

        // サブサブプロシージャ：ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの分以下を下げる
        private void dtpRounddownHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddHours(-hour).AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // サブサブプロシージャ：ＤＡＴＥＴＩＭＥＰＩＣＫＥＲの秒以下を下げる
        private DateTime dtvRounddownSeconds(DateTime dtv)
        {
            int hour = dtv.Hour;
            int minute = dtv.Minute;
            int second = dtv.Second;
            int millisecond = dtv.Millisecond;
            return dtv.AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // データをエクセルへエクスポート
        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)dgvBatchNo.DataSource;
            ExcelClass xl = new ExcelClass();
            xl.ExportToExcel(dt);
            //xl.ExportToCsv(dt, System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\ipqcdb.csv");
        }
        private void update(string str)
        {
            TfSQL tf = new TfSQL();
            string appr = "update t_batch_no set approve_by = '" + txtLeaderName.Text + "' where batch_no = '" + str + "'";
            tf.sqlExecuteScalarString(appr);
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvBatchNo.Rows.Count; i++)
            {
                if (dgvBatchNo.Rows[i].Cells[0].Value.ToString() == "True" && dgvBatchNo.Rows[i].Cells[1].Value.ToString() == "")
                {
                    update(dgvBatchNo.Rows[i].Cells[3].Value.ToString());
                    //updateDataGridViews(ref dtBatchNo, ref dgvBatchNo);
                    dgvBatchNo.Rows[i].Cells[0].Value = true;
                    dgvBatchNo.Rows[i].Cells[0].ReadOnly = true;
                }
            }
            btnSearchBoxId_Click(null, null);
        }

        private void ckbAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvBatchNo.Rows.Count; i++)
            {
                if (ckbAll.Checked == true && dgvBatchNo.Rows[i].Cells[2].Value.ToString() != "")
                {
                    dgvBatchNo.Rows[i].Cells[0].Value = true;
                }
                else if (ckbAll.Checked == false && dgvBatchNo.Rows[i].Cells[1].Value.ToString() == "" || ckbAll.Checked == false && dgvBatchNo.Rows[i].Cells[2].Value.ToString() == "")
                {
                    dgvBatchNo.Rows[i].Cells[0].Value = false;
                }
            }
        }
    }
}