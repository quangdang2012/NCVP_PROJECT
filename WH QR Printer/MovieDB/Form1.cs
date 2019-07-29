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

        // �R���X�g���N�^
        public Form1()
        {
            // �O�̂��߁A�{���ɍ폜����̂��A�Q�x���[�U�[�ɖ₤
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

        // ���[�h���̏���
        private void Form1_Load(object sender, EventArgs e)
        {
            // ���t�H�[���̕\���ꏊ���w��
            this.Left = 100;
            this.Top = 30;

            // �R���{�{�b�N�X�Ŏw�E����v�����g�����̃f�t�H���g�͂P��
            cmbPieceCopy.SelectedIndex = 0;
            cmbPiecePremac.SelectedIndex = 0;

            // �@�o�[�R�[�h���x���R�s�[�p�̃O���b�h�r���[����������
            dtCopy = new DataTable();
            defineDatatable(ref dtCopy, ref dgvCopy);

            // �A�v���}�b�N�f�[�^�v�����g�p�̃O���b�h�r���[����������
            try
            {
                //�N���XTfImport���g�p���A�f�[�^�O���b�g�r���[�Ƀe�L�X�g����荞��
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

                //�s�w�b�_�[�ɍs�ԍ���\������
                for (int i = 0; i < dgvPremac.Rows.Count; i++)
                    dgvPremac.Rows[i].HeaderCell.Value = (i + 1).ToString();

                //�s�w�b�_�[�̕����������߂���
                dgvPremac.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                //�񕝂��������߂���
                dgvPremac.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                //DeliveredQTY��̃Z���̃e�L�X�g�̔z�u���E�񂹂ɂ���
                dgvPremac.Columns["DeliveredQTY"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

                //�S�Ă̗���\�[�g�\�ɐݒ肷��
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

        // �@�T�u�v���V�[�W���F�c�`�s�`�s�`�a�k�d�̒�`
        private void defineDatatable(ref DataTable dt, ref DataGridView dgv)
        {
            // �c�`�s�`�s�`�a�k�d�t�B�[���h�̒�`
            dt.Columns.Add("ItemNumber", Type.GetType("System.String"));
            dt.Columns.Add("ItemName", Type.GetType("System.String"));
            dt.Columns.Add("SupplierName", Type.GetType("System.String"));
            dt.Columns.Add("SupplierInvoice", Type.GetType("System.String"));
            dt.Columns.Add("Delivery", Type.GetType("System.String"));
            dt.Columns.Add("DeliveredQTY", Type.GetType("System.String"));
            dt.Columns.Add("Validity", Type.GetType("System.String"));

            // �P�s�ǉ��A�O���b�g�r���[�ւ̃e�[�u���f�[�^�}��
            dt.Rows.Add(dt.NewRow());
            dgv.DataSource = dt;

            //�s�w�b�_�[�ɍs�ԍ���\������
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();

            //�s�w�b�_�[������ї񕝂𒲐߂���
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // �@�R�s�[�p�X�L�������̏���
        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            // �o�[�R�[�h�����̃G���^�[�L�[�ȊO�͏������Ȃ�
            if (e.KeyCode != Keys.Enter) return;

            // �󕶎��̏ꍇ�͏������Ȃ�
            string scan = txtScan.Text;
            if (scan == String.Empty || scan == "") return;

            // �Z�~�R�����łp�q�ǂݎ����e�𕪊����A�O���b�g�r���[�ɕ\������
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

                // �A�����ăX�L�����ł���悤�A�e�L�X�g��I����Ԃɂ���
                txtScan.SelectAll();
            }
            // �����ł��Ȃ�������̏ꍇ�́A�O���b�g�r���[���N���A�i��\���ɂ���j
            catch (Exception ex)
            {
                dtCopy.Rows[0]["ItemNumber"] = string.Empty;
                dtCopy.Rows[0]["ItemName"] = string.Empty;
                dtCopy.Rows[0]["SupplierName"] = string.Empty;
                dtCopy.Rows[0]["SupplierInvoice"] = string.Empty;
                dtCopy.Rows[0]["Delivery"] = string.Empty;
                dtCopy.Rows[0]["DeliveredQTY"] = string.Empty;
                dtCopy.Rows[0]["Validity"] = string.Empty;

                // �A�����ăX�L�����ł���悤�A�e�L�X�g��I����Ԃɂ���
                txtScan.SelectAll();

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // �@�v�����g�R�s�[�{�^���������̏���
        private void btnPrintCopy_Click(object sender, EventArgs e)
        {
            if (dgvCopy.Rows.Count <= 0) return;

            // �J�����g�Z���̗�Ԓn��ێ�
            int x = dgvCopy.CurrentCellAddress.X;

            // ������̃Z���͈͂ɑ΂��āA�w�ɍ��v�����P��̂ݏ������s��
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

                    // �R���{�{�b�N�X�Ŏw�肵���������v�����g�A�E�g����
                    for (int i = 0; i < printPiece; i++)
                        TfPrint.printBarCode(itemNo, itemName, supplier, invoice, date, qty, validity);
                }
            }
        }

        // �A�v�����g�v���}�b�N�{�^���������̏���
        private void btnPrintPremac_Click(object sender, EventArgs e)
        {
            if (dgvPremac.Rows.Count <= 0) return;

            // �J�����g�Z���̗�Ԓn��ێ�
            int x = dgvPremac.CurrentCellAddress.X;

            // ������̃Z���͈͂ɑ΂��āA�w�ɍ��v�����P��̂ݏ������s��
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

                    // �R���{�{�b�N�X�Ŏw�肵���������v�����g�A�E�g����
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