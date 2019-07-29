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
    public partial class NCVH : Form
    {
        public NCVH()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void NCVH_Load(object sender, EventArgs e)
        {
            // ���t�H�[���̕\���ꏊ���w��
            this.Left = 100;
            this.Top = 30;

            // �R���{�{�b�N�X�Ŏw�E����v�����g�����̃f�t�H���g�͂P��
            cmbPiecePremac.SelectedIndex = 0;
        }
        
        private void btnGetFile_Click(object sender, EventArgs e)
        {
            // �A�v���}�b�N�f�[�^�v�����g�p�̃O���b�h�r���[����������
            try
            {
                OpenFileDialog o1 = new OpenFileDialog();
                o1.ShowDialog();
                string path = o1.FileName;
                DataTable dt = new DataTable();
                
                dgvNCVH.DataSource = TfImport.LoadUserListNCVHExcelFile(path, dt);

                for (int i = 0; i < dgvNCVH.Rows.Count; i++)
                    dgvNCVH.Rows[i].HeaderCell.Value = (i + 1).ToString();

                dgvNCVH.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                dgvNCVH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvNCVH.Columns["DeliveredQTY"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

                foreach (DataGridViewColumn col in dgvNCVH.Columns)
                {
                    dgvNCVH.Columns[col.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvNCVH.Rows.Count <= 0) return;

            // �J�����g�Z���̗�Ԓn��ێ�
            int x = dgvNCVH.CurrentCellAddress.X;

            // ������̃Z���͈͂ɑ΂��āA�w�ɍ��v�����P��̂ݏ������s��
            foreach (DataGridViewCell cell in dgvNCVH.SelectedCells)
            {
                if (cell.ColumnIndex == x)
                {
                    int curRow = cell.RowIndex;

                    for (int i = 0; i < dgvNCVH.Columns.Count; i++)
                    {
                        if (dgvNCVH[i, curRow].Value == null) dgvNCVH[i, curRow].Value = string.Empty;
                    }

                    string materialNo = dgvNCVH["MaterialNo", curRow].Value.ToString();
                    string lotNo = dgvNCVH["LotNo", curRow].Value.ToString();
                    string poNo = dgvNCVH["PONo", curRow].Value.ToString();
                    string poLine = dgvNCVH["POLine", curRow].Value.ToString();
                    string qty = dgvNCVH["DeliveredQTY", curRow].Value.ToString();
                    int printPiece = int.Parse(cmbPiecePremac.Text);

                    if (materialNo.Trim().Length == 0 || lotNo.Trim().Length == 0 ||
                            poNo.Trim().Length == 0 || qty.Trim().Length == 0)
                    {
                        MessageBox.Show("Please fill all of the following fields:" + Environment.NewLine
                            + "Material Number, Lot Number, PO Number, Delivered QTY", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // �R���{�{�b�N�X�Ŏw�肵���������v�����g�A�E�g����
                    for (int i = 0; i < printPiece; i++)
                        TfPrint.printBarCodeNCVH(materialNo, lotNo, poNo, poLine, qty);
                }
            }
        }
    }
}