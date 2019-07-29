using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;

namespace WhQrPrinter
{
    public partial class Form1 : Form
    {

        DataTable dtCopy;
        bool PrintFromPremacText;

        // コンストラクタ
        public Form1()
        {
            // 念のため、本当に削除するのか、２度ユーザーに問う
            DialogResult result1 = MessageBox.Show("Do you print from Premac data ?" + System.Environment.NewLine + System.Environment.NewLine +
                "Click Yes for printing from CPFXE049.TXT @ Desktop, " + System.Environment.NewLine +
                "Click No  for printing from BarPrint.csv @ Desktop.",
                "Choose data file", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result1 == DialogResult.Yes)
                PrintFromPremacText = true;
            else if (result1 == DialogResult.No)
                PrintFromPremacText = false;

            InitializeComponent();
        }

        // ロード時の処理
        private void Form1_Load(object sender, EventArgs e)
        {
            // 当フォームの表示場所を指定
            this.Left = 100;
            this.Top = 30;

            // コンボボックスで指摘するプリント枚数のデフォルトは１枚
            cmbPieceCopy.SelectedIndex = 0;
            cmbPiecePremac.SelectedIndex = 0;

            // ①バーコードラベルコピー用のグリッドビューを準備する
            dtCopy = new DataTable();
            defineDatatable(ref dtCopy, ref dgvCopy);

            // ②プレマックデータプリント用のグリッドビューを準備する
            try
            {
                //クラスTfImportを使用し、データグリットビューにテキストを取り込む
                if (PrintFromPremacText)
                {
                    dgvPremac.DataSource = TfImport.LoadUserListFromPremacFile(
                        System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\CPFXE049.TXT");
                }
                else
                {
                    dgvPremac.DataSource = TfImport.LoadUserListFromExcelFile(
                        System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\BarPrint.csv");
                }

                //行ヘッダーに行番号を表示する
                for (int i = 0; i < dgvPremac.Rows.Count; i++)
                    dgvPremac.Rows[i].HeaderCell.Value = (i + 1).ToString();

                //行ヘッダーの幅を自動調節する
                dgvPremac.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                //列幅を自動調節する
                dgvPremac.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //DeliveredQTY列のセルのテキストの配置を右寄せにする
                dgvPremac.Columns["DeliveredQTY"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

                //全ての列をソート可能に設定する
                foreach (DataGridViewColumn column in dgvPremac.Columns)
                {
                    dgvPremac.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ①サブプロシージャ：ＤＡＴＡＴＡＢＬＥの定義
        private void defineDatatable(ref DataTable dt, ref DataGridView dgv)
        {
            // ＤＡＴＡＴＡＢＬＥフィールドの定義
            dt.Columns.Add("ItemNumber", Type.GetType("System.String"));
            dt.Columns.Add("ItemName", Type.GetType("System.String"));
            dt.Columns.Add("SupplierName", Type.GetType("System.String"));
            dt.Columns.Add("SupplierInvoice", Type.GetType("System.String"));
            dt.Columns.Add("Delivery", Type.GetType("System.String"));
            dt.Columns.Add("DeliveredQTY", Type.GetType("System.String"));
            dt.Columns.Add("Validity", Type.GetType("System.String"));

            // １行追加、グリットビューへのテーブルデータ挿入
            dt.Rows.Add(dt.NewRow());
            dgv.DataSource = dt;

            //行ヘッダーに行番号を表示する
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();

            //行ヘッダー幅および列幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // ①コピー用スキャン時の処理
        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            // バーコード末尾のエンターキー以外は処理しない
            if (e.KeyCode != Keys.Enter) return;

            // 空文字の場合は処理しない
            string scan = txtScan.Text;
            if (scan == String.Empty || scan == "") return;

            // セミコロンでＱＲ読み取り内容を分割し、グリットビューに表示する
            try
            {
                string[] split = scan.Split(';');
                dtCopy.Rows[0]["ItemNumber"] = split[0];
                dtCopy.Rows[0]["ItemName"] = split[1];
                dtCopy.Rows[0]["SupplierName"] = split[2];
                dtCopy.Rows[0]["SupplierInvoice"] = split[3];
                dtCopy.Rows[0]["Delivery"] = split[4];
                dtCopy.Rows[0]["DeliveredQTY"] = split[5];
                dtCopy.Rows[0]["Validity"] = split[6];

                // 連続してスキャンできるよう、テキストを選択状態にする
                txtScan.SelectAll();
            }
            // 分割できない文字列の場合は、グリットビューをクリア（空表示にする）
            catch (Exception ex)
            {
                dtCopy.Rows[0]["ItemNumber"] = string.Empty;
                dtCopy.Rows[0]["ItemName"] = string.Empty;
                dtCopy.Rows[0]["SupplierName"] = string.Empty;
                dtCopy.Rows[0]["SupplierInvoice"] = string.Empty;
                dtCopy.Rows[0]["Delivery"] = string.Empty;
                dtCopy.Rows[0]["DeliveredQTY"] = string.Empty;
                dtCopy.Rows[0]["Validity"] = string.Empty;

                // 連続してスキャンできるよう、テキストを選択状態にする
                txtScan.SelectAll();

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ①プリントコピーボタン押下時の処理
        private void btnPrintCopy_Click(object sender, EventArgs e)
        {
            if (dgvCopy.Rows.Count <= 0) return;

            // カレントセルの列番地を保持
            int x = dgvCopy.CurrentCellAddress.X;

            // 複数列のセル範囲に対して、Ｘに合致した１列のみ処理を行う
            foreach (DataGridViewCell cell in dgvCopy.SelectedCells)
            {
                if (cell.ColumnIndex == x)
                {
                    int curRow = cell.RowIndex;

                    for (int i = 0; i < dgvCopy.Columns.Count; i++)
                    {
                        if (dgvCopy[i, curRow].Value == null) dgvCopy[i, curRow].Value = string.Empty;
                    }       

                    string itemNo = dgvCopy["ItemNumber", curRow].Value.ToString();
                    string itemName = dgvCopy["ItemName", curRow].Value.ToString();
                    string supplier = dgvCopy["SupplierName", curRow].Value.ToString();
                    string invoice = dgvCopy["SupplierInvoice", curRow].Value.ToString();
                    string date = dgvCopy["Delivery", curRow].Value.ToString();
                    string qty = dgvCopy["DeliveredQTY", curRow].Value.ToString();
                    string validity = dgvCopy["Validity", curRow].Value.ToString();
                    int printPiece = int.Parse(cmbPieceCopy.Text);

                    if (itemNo.Trim().Length == 0 || itemName.Trim().Length == 0 ||
                            supplier.Trim().Length == 0 || invoice.Trim().Length == 0 || date.Trim().Length == 0)
                    {
                        MessageBox.Show("Please fill all of the following fields:" + Environment.NewLine
                            + "Item Number, Item Name, Supplier Name, Supplier Invoice, Delivery Date", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // コンボボックスで指定した枚数をプリントアウトする
                    for (int i = 0; i < printPiece; i++)
                        TfPrint.printBarCode(itemNo, itemName, supplier, invoice, date, qty, validity);
                }
            }
        }

        // ②プリントプレマックボタン押下時の処理
        private void btnPrintPremac_Click(object sender, EventArgs e)
        {
            if (dgvPremac.Rows.Count <= 0) return;

            // カレントセルの列番地を保持
            int x = dgvPremac.CurrentCellAddress.X;

            // 複数列のセル範囲に対して、Ｘに合致した１列のみ処理を行う
            foreach (DataGridViewCell cell in dgvPremac.SelectedCells)
            {
                if (cell.ColumnIndex == x)
                {
                    int curRow = cell.RowIndex;

                    for (int i = 0; i < dgvCopy.Columns.Count; i++)
                    {
                        if (dgvPremac[i, curRow].Value == null) dgvPremac[i, curRow].Value = string.Empty;
                    }

                    string itemNo = dgvPremac["ItemNumber", curRow].Value.ToString();
                    string itemName = dgvPremac["ItemName", curRow].Value.ToString();
                    string supplier = dgvPremac["SupplierName", curRow].Value.ToString();
                    string invoice = dgvPremac["SupplierInvoice", curRow].Value.ToString();
                    string date = dgvPremac["Delivery", curRow].Value.ToString();
                    string qty = dgvPremac["DeliveredQTY", curRow].Value.ToString();
                    string validity = dgvPremac["Validity", curRow].Value.ToString();
                    int printPiece = int.Parse(cmbPiecePremac.Text);

                    if (itemNo.Trim().Length == 0 || itemName.Trim().Length == 0 ||
                            supplier.Trim().Length == 0 || invoice.Trim().Length == 0 || date.Trim().Length == 0)
                    {
                        MessageBox.Show("Please fill all of the following fields:" + Environment.NewLine
                            + "Item Number, Item Name, Supplier Name, Supplier Invoice, Delivery Date", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // コンボボックスで指定した枚数をプリントアウトする
                    for (int i = 0; i < printPiece; i++)
                        TfPrint.printBarCode(itemNo, itemName, supplier, invoice, date, qty, validity);
                }
            }
        }

        private void txtScan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}