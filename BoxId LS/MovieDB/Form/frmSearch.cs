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
using Excel = Microsoft.Office.Interop.Excel; 

namespace BoxIdDb
{
    public partial class frmSearch : Form
    {

        // コンストラクタ
        public frmSearch()
        {
            InitializeComponent();
        }

        // ロード時の処理
        private void frmModule_Load(object sender, EventArgs e)
        {
            //フォームの場所を指定
            this.Left = 350;
            this.Top = 30;
            comboAndTextBoxSetUp();
            btnSearch_Click(sender, e);
        }

        // サブプロシージャ：コンボボックスとテキストボックスに、オートコンプリート機能の追加
        private void comboAndTextBoxSetUp()
        {
            //string sql = "select boxid FROM box_id_cfg";
            //tf.getAutoCompleteData(sql, ref txtBoxIdFrom);
            //tf.getAutoCompleteData(sql, ref txtBoxIdTo);

            //sql = "select distinct serialno FROM product_serial_printdate";
            //tf.getAutoCompleteData(sql, ref txtProductSerialFrom);
            //tf.getAutoCompleteData(sql, ref txtProductSerialTo);

            //string sql = "select distinct config2 FROM box_id_cfg";
            //ShSQL tf = new ShSQL();
            //tf.getComboBoxData(sql, ref cmbConfig);
        }

        // フォームの条件をＡＮＤで結合し、ＳＱＬ問い合わせ結果をデータグリッドビューに反映
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string idFrom = txtBoxIdFrom.Text;
            string idTo = txtBoxIdTo.Text;
            DateTime dateFrom = dtpPrintDateFrom.Value;
            DateTime dateTo = dtpPrintDateTo.Value;
            string serFrom = txtProductSerialFrom.Text;
            string serTo = txtProductSerialTo.Text;
            string config = cmbConfig.Text;

            string sql1 = "select boxid, printdate, serialno, lot, fact, process, linepass, testtime FROM product_serial_printdate WHERE ";

            bool[] cr = { idFrom == String.Empty ? false : true,
                          idTo  == String.Empty ? false : true,
                                                  true,
                                                  true,
                          serFrom == String.Empty ? false : true,
                          serTo == String.Empty ? false : true,
                          config == String.Empty ? false : true };

            string sql2 = (!cr[0] ? String.Empty : "boxid >= '" + idFrom + "' AND ") +
                          (!cr[1] ? String.Empty : "boxid <= '" + idTo + "' AND ") +
                                                   "printdate >= '" + dateFrom.ToString() + "' AND " +
                                                   "printdate <= '" + dateTo.ToString() + "' AND " +
                          (!cr[4] ? String.Empty : "serialno >= '" + serFrom + "' AND ") +
                          (!cr[5] ? String.Empty : "serialno <= '" + serTo + "' AND ") +
                          (!cr[6] ? String.Empty : "config2 = '" + config + "' AND ");

            string sql3 = sql1 + VBStrings.Left(sql2, sql2.Length - 5);
            System.Diagnostics.Debug.Print(sql3);

            btnSearch.Enabled = false;
            
            if (rdbOn.Checked)
            {
                DialogResult result1 = MessageBox.Show("With the summary function On, the process takes time." + System.Environment.NewLine +
                    "Do you poceed with the summary function On ?" , "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result1 == DialogResult.No) return;
            }
            
            DataTable dataTable = new DataTable();
            ShSQL tf = new ShSQL();
            tf.sqlDataAdapterFillDatatable(sql3, ref dataTable);

            bool count = dataTable.Rows.Count > 200000 ? true : false;
            if (rdbOn.Checked && count)
            {
                MessageBox.Show("The record count is over 200,000.  The summary function is turned off.", "Notice", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                rdbOff.Checked = true;
            }

            if (rdbOn.Checked)
            {
                updateDataGripViews(dataTable, ref dgvProductSerial, true);
            }
            else if (rdbOff.Checked)
            {
                updateDataGripViews(dataTable, ref dgvProductSerial, false);
            }

            btnSearch.Enabled = true;
        }

        // サブプロシージャ：データグリットビューの更新
        private void updateDataGripViews(DataTable dt, ref DataGridView dgv, bool summary)
        {
            // データグリットビューへＳＱＬ問い合わせ結果を格納
            updateDataGripViewsSub(dt, ref dgv, summary);

            //行ヘッダーに行番号を表示する
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();

            //行ヘッダーの幅を自動調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);        

            // 一番下の行を表示する
            if (dgv.Rows.Count >= 1)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;

        }

        // サブプロシージャ：メインデータグリットビューへデータテーブルを格納、および集計グリッドビューの作成
        private void updateDataGripViewsSub(DataTable dt, ref DataGridView dgv, bool summary)
        {
            dgv.DataSource = dt;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgvLine.Rows.Clear();
            dgvConfig.Rows.Clear();
            dgvPassFail.Rows.Clear();

            if (summary)
            {
                string[] criteriaFact = { "1", "2", "3", "4", "Total" };
                makeDatatableSummary(dt, ref dgvLine, criteriaFact, "fact");

                string[] criteriaPassFail = { "PASS", "FAIL", "No Data", "Total" };
                makeDatatableSummary(dt, ref dgvPassFail, criteriaPassFail, "linepass");

                //string[] criteriaPassFail = { "PASS", "FAIL", "No Data", "Total" };
                //makeDatatableSummary(dt, ref dgvPassFail, criteriaPassFail, "linepass");
            }
        }

        // サブサブプロシージャ：集計用のデータテーブルを、データグリッドビューに格納
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
                dr[criteria[i]] = (i != criteria.Length - 1  ? count : total);
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
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)dgvProductSerial.DataSource;
            ExcelClass xl = new ExcelClass();
            //xl.ExportToExcel(dt);
            xl.ExportToCsv(dt, System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\boxid.csv");
        }

        private void dgvLine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}