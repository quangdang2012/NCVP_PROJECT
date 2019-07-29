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
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;

namespace Tracy
{
    public partial class Input : Form
    {
        //�e�t�H�[��Form1�փC�x���g������A���i�f���Q�[�g�j
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // �v�����g�p�e�L�X�g�t�@�C���̕ۑ��p�t�H���_���A��{�ݒ�t�@�C���Őݒ肷��
        string appconfig = System.Environment.CurrentDirectory + "\\info.ini";
        string directory = @"C:\Users\takusuke.fujii\Desktop\Auto Print\\";

        //���̑��A�񃍁[�J���ϐ�
        DataTable dtOperator;
        DataTable dtParts;
        DataTable dtSubMaterial;
        DataTable dtPartsMaster;
        DataTable dtSubPartsMaster;

        bool formAddMode;
        bool formAdmin;
        bool b_headerComplete;
        bool b_operatorComplete;
        bool b_partsComplete;
        bool b_subMatComplete;
        bool matchPartsMaster = false;
        bool sound;
        public string leaderid, leadername;

        public void truyenbien(string _leaderid, string _leadername)
        {
            leaderid = _leaderid;
            leadername = _leadername;
        }
        public void btnDelete()
        {
            TfSQL yn = new TfSQL();
            string batchNo = txtBatchNo.Text;
            string prtn = yn.sqlExecuteScalarString("select print from t_batch_no where batch_no = '" + batchNo + "'");

            if (prtn == "1")
            {
                btnDeleteBatch.Enabled = false;
            }
        }
        // �R���X�g���N�^
        public Input()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void Input_Load(object sender, EventArgs e)
        {

            this.Left = 60;
            this.Top = 35;


            dtOperator = new DataTable();
            dtParts = new DataTable();
            dtSubMaterial = new DataTable();
            dtPartsMaster = new DataTable();
            dtSubPartsMaster = new DataTable();



            defineTables(ref dtOperator, ref dtParts, ref dtSubMaterial, ref dtPartsMaster, ref dtSubPartsMaster);


            if (!formAddMode)
            {
                readOperatorTable(ref dtOperator, ref dgvOperator, true);
                readPartsTable(ref dtParts, ref dgvParts);
                readSubMaterialTable(ref dtSubMaterial, ref dgvSubMaterial);
            }
            else if (formAddMode)
            {
                showDefaultProcess(ref dtOperator, ref dgvOperator, true);
                insertBlankRecord(ref dtParts, ref dgvParts);
                insertBlankRecord(ref dtSubMaterial, ref dgvSubMaterial);
            }


            if (formAddMode)
            {
                txtBatchNo.Text = getNewBatch();
                insertBatchNo();
            }


            directory = txtLink.Text + "\\"; //readIni("TARGET DIRECTORY", "DIR", appconfig);

            #region Permission
            //TfSQL yn = new TfSQL();
            //string permiss = yn.sqlExecuteScalarString("select permission from t_leader_id where leader_id = '" + leaderid + "'");
            //string stt = yn.sqlExecuteScalarString("select check_by from t_batch_no where batch_no = '" + txtBatchNo.Text + "'");
            //string stt1 = yn.sqlExecuteScalarString("select approve_by from t_batch_no where batch_no = '" + txtBatchNo.Text + "'");
            ////string user = yn.sqlExecuteScalarString("select leader_name from t_batch_no where batch_no = '" + txtBatchNo.Text + "'");

            //if (permiss == "user" && txtLeaderName.Text == leadername && stt == "")
            //{
            //    btnDeleteBatch.Enabled = true;
            //    btnUpdateBatch.Enabled = true;
            //}

            //if (permiss == "check" && stt != "")
            //{
            //    btnCheck.Enabled = false;
            //    btnDeleteBatch.Enabled = false;
            //    btnUpdateBatch.Enabled = false;
            //    rdCheck.Enabled = false;
            //    rdApprove.Enabled = false;
            //    grbOP.Enabled = false;
            //    grbPart.Enabled = false;
            //    grbSub.Enabled = false;
            //    dgvOperator.ReadOnly = true;
            //    dgvParts.ReadOnly = true;
            //    dgvSubMaterial.ReadOnly = true;
            //    rdCheck.Text = "Checked by: " + stt;
            //    if (stt1 != "")
            //    {
            //        rdApprove.Text = "Approved by: " + stt1;
            //    }
            //}
            //else if (permiss == "approve" && stt != "")
            //{
            //    btnCheck.Enabled = false;
            //    btnDeleteBatch.Enabled = true;
            //    rdCheck.Enabled = true;
            //    rdApprove.Enabled = true;
            //    rdCheck.Text = "Checked by: " + stt;
            //    if (stt1 != "")
            //    {
            //        btnDeleteBatch.Enabled = false;
            //        btnUpdateBatch.Enabled = false;
            //        rdApprove.Text = "Approved by: " + stt1;
            //        dgvOperator.ReadOnly = true;
            //        dgvParts.ReadOnly = true;
            //        dgvSubMaterial.ReadOnly = true;
            //        grbOP.Enabled = false;
            //        grbPart.Enabled = false;
            //        grbSub.Enabled = false;
            //    }
            //}
            //else if (permiss == "user")
            //{
            //    btnCheck.Enabled = false;
            //    rdCheck.Enabled = false;
            //    rdApprove.Enabled = false;
            //    if (stt != "" && stt1 != "")
            //    {
            //        btnUpdateBatch.Enabled = false;
            //        grbOP.Enabled = false;
            //        grbPart.Enabled = false;
            //        grbSub.Enabled = false;
            //        dgvOperator.ReadOnly = true;
            //        dgvParts.ReadOnly = true;
            //        dgvSubMaterial.ReadOnly = true;
            //        rdApprove.Text = "Approved by: " + stt1;
            //        rdCheck.Text = "Checked by: " + stt;
            //    }
            //    else if (stt != "")
            //    {
            //        btnUpdateBatch.Enabled = false;
            //        grbOP.Enabled = false;
            //        grbPart.Enabled = false;
            //        grbSub.Enabled = false;
            //        dgvOperator.ReadOnly = true;
            //        dgvParts.ReadOnly = true;
            //        dgvSubMaterial.ReadOnly = true;
            //        rdCheck.Text = "Checked by: " + stt;
            //    }
            //}
            //else if (permiss == "approve")
            //{
            //    btnDeleteBatch.Enabled = true;
            //    btnCheck.Enabled = false;
            //    rdCheck.Enabled = false;
            //    rdApprove.Enabled = false;
            //}
            //else if (permiss == "check")
            //{
            //    btnCheck.Enabled = true;
            //    rdCheck.Enabled = false;
            //    rdApprove.Enabled = false;
            //    btnDeleteBatch.Enabled = true;
            //}

            //if (stt != "" && stt1 != "")
            //{
            //    btnPrintBatch.Enabled = true;
            //}
            #endregion

            //btnDelete();
        }

        // �T�u�v���V�[�W���F�e�t�H�[���ŌĂяo���A�e�t�H�[���̏����A�e�L�X�g�{�b�N�X�֊i�[���Ĉ����p��
        public void updateControls(string batchNo, string modelNo, string modelName, string subAssyNo, string subAssyName,
            DateTime batchDate, string shift, string line, string leader, string leaderName, string input, string output,
            DateTime inputTime, DateTime outputTime, string remark, bool addMode, bool admin)
        {
            txtBatchNo.Text = batchNo;
            cmbModelNo.Text = modelNo;
            txtModelName.Text = modelName;
            cmbSubAssyNo.Text = subAssyNo;
            txtSubAssyName.Text = subAssyName;
            dtpBatchDate.Value = batchDate;
            cmbShift.Text = shift;
            cmbLine.Text = line;
            txtLeaderId.Text = leader;
            txtLeaderName.Text = leaderName;
            txtInputQty.Text = input.ToString();
            txtOutputQty.Text = output.ToString();
            dtpInputTime.Value = inputTime;
            dtpOutputTime.Value = outputTime;
            txtRemark.Text = remark;
            formAddMode = addMode;
            formAdmin = admin;

            // �ҏW���[�h�̏ꍇ�́A�o�^�����m�F�͍s��Ȃ��i�����t���O��\�߃I���j
            if (!formAddMode)
            {
                b_headerComplete = true;
                b_operatorComplete = true;
                b_partsComplete = true;
                b_subMatComplete = true;
            }

            // �Ǘ����[�h�łȂ��ꍇ�i���̃��[�_�[�N�Ẵo�b�`�̏ꍇ�j�A����{�^���ȊO�𖳌��ɂ���
            if (!formAdmin)
            {
                disableControlsExceptTabControlAndCloseButton(this);
            }
        }

        // �T�u�v���V�[�W���F�t�H�[���̖�����
        private void disableControlsExceptTabControlAndCloseButton(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                // ����{�^���ȊO�͖����ɂ���
                if (child.Name != "tabControl1" && child.Name != "btnClose" && child.Name != "btnPrintBatch")
                    child.Enabled = false;
            }

            // ���ꂼ��̃^�u�y�[�W��̃R���g���[���𖳌��ɂ���
            foreach (Control child in this.tabOperator.Controls)
                child.Enabled = false;

            foreach (Control child in this.tabParts.Controls)
                child.Enabled = false;

            foreach (Control child in this.tabSubMaterial.Controls)
                child.Enabled = false;
        }

        // �T�u�v���V�[�W���F�c�s�̒�`
        private void defineTables(ref DataTable dt1, ref DataTable dt2, ref DataTable dt3, ref DataTable dt4, ref DataTable dt5)
        {
            dt1.Columns.Add("process", Type.GetType("System.String"));
            dt1.Columns.Add("operator", Type.GetType("System.String"));
            dt1.Columns.Add("machine", Type.GetType("System.String"));

            dt2.Columns.Add("parts_no", Type.GetType("System.String"));
            dt2.Columns.Add("parts_name", Type.GetType("System.String"));
            dt2.Columns.Add("parts_supplier", Type.GetType("System.String"));
            dt2.Columns.Add("parts_invoice", Type.GetType("System.String"));
            dt2.Columns.Add("qty", Type.GetType("System.Double"));
            dt2.Columns.Add("note", Type.GetType("System.String"));

            dt3.Columns.Add("sub_mat_no", Type.GetType("System.String"));
            dt3.Columns.Add("sub_mat_name", Type.GetType("System.String"));
            dt3.Columns.Add("sub_mat_supplier", Type.GetType("System.String"));
            dt3.Columns.Add("sub_mat_invoice", Type.GetType("System.String"));
            dt3.Columns.Add("validity", Type.GetType("System.DateTime"));

            dt4.Columns.Add("parts_no", Type.GetType("System.String"));
            dt4.Columns.Add("parts_name", Type.GetType("System.String"));
            dt4.Columns.Add("ratio", Type.GetType("System.Double"));
            dt4.Columns.Add("match", Type.GetType("System.String"));

            dt5.Columns.Add("sub_parts_no", Type.GetType("System.String"));
            dt5.Columns.Add("sub_parts_name", Type.GetType("System.String"));
            dt5.Columns.Add("ratio", Type.GetType("System.Double"));
            dt5.Columns.Add("match", Type.GetType("System.String"));
        }

        // �T�u�v���V�[�W���F�󃌃R�[�h���c�s�֒ǉ����\������
        private void insertBlankRecord(ref DataTable dt, ref DataGridView dgv)
        {
            dt.Clear();
            dt.Rows.Add(dt.NewRow());
            updateDataGridViewsub(dt, ref dgv);
        }

        // �@�I�y���[�^�[
        // �T�u�v���V�[�W���F�I�y���[�^�[�����f�[�^�O���b�h�r���[�ɔ��f����
        private void readOperatorTable(ref DataTable dt, ref DataGridView dgv, bool load)
        {
            string batchNo = txtBatchNo.Text;
            string sql = "select process, operator, machine from t_operator " + "where batch_no='" + batchNo + "'";
            System.Diagnostics.Debug.Print(sql);
            dt.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
            updateDataGridViewsub(dt, ref dgv);

            //// �O���b�h�r���[�ɃR���{�{�b�N�X��ǉ�����
            //if(load) insertComboBoxToGridView();
        }

        // �T�u�v���V�[�W���F�V�K�o�^���[�h���A�H�����X�g��\������
        private void showDefaultProcess(ref DataTable dt, ref DataGridView dgv, bool load)
        {
            string subAssyNo = cmbSubAssyNo.Text;
            string modelNo = cmbModelNo.Text;
            // string line = "mc_line_" + cmbLine.Text; �}�V���ԍ��擾�@�\�́A�g�p��~�B�����I�ɍēx�g�p����\���͂���B
            // string sql = "select process, " + line + " as machine from t_process " + "where sub_assy_no='" + subAssyNo + "'";

            string sql = "select process from t_process where sub_assy_no='" + subAssyNo + "' and model_no = '" + modelNo + "' order by process";
            System.Diagnostics.Debug.Print(sql);
            dt.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
            updateDataGridViewsub(dt, ref dgv);

            //// �O���b�h�r���[�ɃR���{�{�b�N�X��ǉ�����
            //if (load) insertComboBoxToGridView();
        }

        // �T�u�v���V�[�W���F�O���b�h�r���[�ɃR���{�{�b�N�X��ǉ�����
        private void insertComboBoxToGridView()
        {
            ////dgvOperator.Columns["process"].Visible = false;
            //DataGridViewComboBoxColumn cmbCol= new DataGridViewComboBoxColumn();
            //cmbCol.HeaderText = "select_process";
            //cmbCol.Name = "cmbProcess";

            //string sql = "select process from t_process where " + "sub_assy_no = '" + cmbSubAssyNo.Text + "'";
            //System.Diagnostics.Debug.Print(sql);
            //DataTable dtProcessList = new DataTable();
            //TfSQL tf = new TfSQL();
            //tf.sqlDataAdapterFillDatatable(sql, ref dtProcessList);
            //foreach (DataRow row in dtProcessList.Rows)
            //    cmbCol.Items.Add(row[0].ToString());

            //dgvOperator.Columns.Add(cmbCol);
            //dgvOperator.Columns["cmbProcess"].DisplayIndex = 0;
        }

        // �O���b�h�r���[�R���{�{�b�N�X�̒l���ύX���ꂽ�ۂ̏����i�C�x���g�����j
        private void dgvOperator_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if (dgvOperator.CurrentCell.ColumnIndex == 3 && e.Control is ComboBox)
            //{
            //    ComboBox comboBox = e.Control as ComboBox;
            //    comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            //}
        }

        // �T�u�v���V�[�W���F�O���b�h�r���[�R���{�{�b�N�X�̒l���ύX���ꂽ�ۂ̏����i�C�x���g���e�j
        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    var currentcell = dgvOperator.CurrentCellAddress;
            //    var sendingCB = sender as DataGridViewComboBoxEditingControl;
            //    DataGridViewTextBoxCell nextcell = (DataGridViewTextBoxCell)dgvOperator.Rows[currentcell.Y].Cells["process"];
            //    nextcell.Value = sendingCB.EditingControlFormattedValue.ToString();
            //    nextcell.Selected = true;
            //    dgvOperator.Columns["cmbProcess"].Width = 50;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        // �A���i
        // �T�u�v���V�[�W���F���i�����f�[�^�O���b�h�r���[�ɔ��f����
        private void readPartsTable(ref DataTable dt, ref DataGridView dgv)
        {
            string batchNo = txtBatchNo.Text;
            string sql = "select parts_no, parts_name, parts_supplier, parts_invoice, qty, note from t_parts_invoice where batch_no='" + batchNo + "'";
            System.Diagnostics.Debug.Print(sql);
            dt.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
            updateDataGridViewsub(dt, ref dgv);

            // �X�L�����o�^���ł���悤�A�J�����g�Z����ݒ肵�A�c�h�q�s�x�ɂ���
            int r = dt.Rows.Count;
            dt.Rows.Add(dt.NewRow());
            dgv.CurrentCell = dgv[0, r];
            dgv.NotifyCurrentCellDirty(true);
            dgv.NotifyCurrentCellDirty(false);

            // ���i�}�X�^�����擾����
            readPartsMasterTable(ref dtPartsMaster);
            readSubPartsMasterTable(ref dtSubPartsMaster);
        }

        // �T�u�v���V�[�W���F���i�}�X�^�����擾����
        private void readPartsMasterTable(ref DataTable dt)
        {
            string subAssyNo = cmbSubAssyNo.Text;
            string modelNo = cmbModelNo.Text;
            string sql = "select parts_no, parts_name, ratio from t_parts where sub_assy_no ='" + subAssyNo + "' and model_no = '" + modelNo + "'";
            System.Diagnostics.Debug.Print(sql);
            dt.Rows.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }

        private void readSubPartsMasterTable(ref DataTable dt)
        {
            string subAssyNo = cmbSubAssyNo.Text;
            string modelNo = cmbModelNo.Text;
            string sql = "select sub_parts_no, sub_parts_name, ratio from t_sub_parts where sub_assy_no ='" + subAssyNo + "' and model_no = '" + modelNo + "'";
            System.Diagnostics.Debug.Print(sql);
            dt.Rows.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }

        // �B������
        // �T�u�v���V�[�W���F�����ޏ����f�[�^�O���b�h�r���[�ɔ��f����
        private void readSubMaterialTable(ref DataTable dt, ref DataGridView dgv)
        {
            string batchNo = txtBatchNo.Text;
            string sql = "select sub_mat_no, sub_mat_name, sub_mat_supplier, sub_mat_invoice, validity from t_sub_mat_invoice where batch_no='" + batchNo + "'";
            System.Diagnostics.Debug.Print(sql);
            dt.Clear();
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
            updateDataGridViewsub(dt, ref dgv);

            // �X�L�����o�^���ł���悤�A�J�����g�Z����ݒ肵�A�c�h�q�s�x�ɂ���
            int r = dt.Rows.Count;
            dt.Rows.Add(dt.NewRow());
            dgv.CurrentCell = dgv[0, r];
            dgv.NotifyCurrentCellDirty(true);
            dgv.NotifyCurrentCellDirty(false);
        }

        // �T�u�T�u�v���V�[�W���F�f�[�^�O���b�g�r���[�փf�[�^�e�[�u�����i�[
        private void updateDataGridViewsub(DataTable dt, ref DataGridView dgv)
        {
            //�f�[�^�O���b�g�r���[�փf�[�^�e�[�u�����i�[
            dgv.DataSource = dt;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // �o�b�`�w�b�_�[���X�V�{�^���������̏���
        private void btnUpdateBatch_Click(object sender, EventArgs e)
        {
            string batchNo = txtBatchNo.Text;
            double inQty;
            double.TryParse(txtInputQty.Text, out inQty);
            double outQty;
            double.TryParse(txtOutputQty.Text, out outQty);
            DateTime inTime = dtpInputTime.Value.Date;
            DateTime outTime = dtpOutputTime.Value.Date;
            string remark = txtRemark.Text;

            bool[] cr = { txtInputQty.Text == string.Empty  ? false : true,
                          txtOutputQty.Text == string.Empty ? false : true,
                                                                      true,
                                                                      true,
                          remark  == string.Empty           ? false : true  };

            string sql1 = "update t_batch_no set " +
                (cr[0] ? "in_qty=" + inQty + "," : "in_qty = null,") +
                (cr[1] ? "out_qty=" + outQty + "," : "out_qty = null,") +
                (cr[2] ? "in_time='" + inTime + "'," : string.Empty) +
                (cr[3] ? "out_time='" + outTime + "'," : string.Empty) +
                (cr[4] ? "remark='" + remark + "'," : "remark = null,");

            string sql2 = " where batch_no='" + batchNo + "'";

            string sql3 = VBStrings.Left(sql1, sql1.Length - 1) + sql2;
            System.Diagnostics.Debug.Print(sql3);
            TfSQL tf = new TfSQL();
            b_headerComplete = tf.sqlExecuteNonQuery(sql3, false);

            if (b_headerComplete)
                MessageBox.Show("Step 1: Batch general info register completed", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //�e�t�H�[��Form1�̃f�[�^�O���b�g�r���[���X�V���邽�߁A�f���Q�[�g�C�x���g�𔭐�������
            this.RefreshEvent(this, new EventArgs());
        }

        // ����{�^���������̏���
        private void btnPrintBatch_Click(object sender, EventArgs e)
        {
            string check = rdCheck.Text;
            string approve = rdApprove.Text;
            string batchNo = txtBatchNo.Text;
            string modelNo = cmbModelNo.Text;
            string modelName = txtModelName.Text;
            string subAssyNo = cmbSubAssyNo.Text;
            string subAssyName = txtSubAssyName.Text;
            DateTime batchDate = dtpBatchDate.Value;
            string shift = cmbShift.Text;
            string line = cmbLine.Text;
            string leader = txtLeaderId.Text;
            string leaderName = txtLeaderName.Text;
            string input = txtInputQty.Text;
            string output = txtOutputQty.Text;
            DateTime inputTime = dtpInputTime.Value;
            DateTime outputTime = dtpOutputTime.Value;
            string remark = txtRemark.Text;

            TfSQL prt = new TfSQL();
            string print = prt.sqlExecuteScalarString("update t_batch_no set print = '1' where batch_no = '" + batchNo + "'");
            btnDeleteBatch.Enabled = false;

            ExcelClass xl = new ExcelClass();

            // ���L�v�����^�[�p�b�r�u�o��
            xl.ExportBatchToCsvForPrint(directory + batchNo + ".csv", "SUB ASSY", subAssyNo, subAssyName, outputTime, output, batchNo, line);

            // �G�N�Z���֏o��
            xl.ExportBatchDetailToExcel(check, approve, batchNo, modelNo, modelName, subAssyNo, subAssyName, batchDate, shift, line,
               leader, leaderName, input, output, inputTime, outputTime, remark, dtOperator, dtParts, dtSubMaterial);
        }

        // �o�b�`�폜�{�^���������̏����i�w�b�_�[�Ɩ��בS�Ă̍폜�j
        private void btnDeleteBatch_Click(object sender, EventArgs e)
        {
            string model = cmbModelNo.Text;
            // �O�̂��߁A�{���ɍ폜����̂��A�Q�x���[�U�[�ɖ₤
            DialogResult result1 = MessageBox.Show("Do you really want to delete the batch data?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result1 == DialogResult.No) return;

            DialogResult result2 = MessageBox.Show("Again, is it OK to delete the batch?",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result2 == DialogResult.No) return;

            string batchNo = txtBatchNo.Text;
            string[] sql = {"delete from t_batch_no where batch_no='" + batchNo + "'",
                            "delete from t_operator where batch_no='" + batchNo + "'",
                            "delete from t_parts_invoice where batch_no='" + batchNo + "'",
                            "delete from t_sub_mat_invoice where batch_no='" + batchNo + "'" };

            TfSQL tf = new TfSQL();

            for (int i = 0; i < sql.Length; i++)
            {
                System.Diagnostics.Debug.Print(sql[i]);
                tf.sqlExecuteNonQuery(sql[i], false);
            }

            //�e�t�H�[��Form1�̃f�[�^�O���b�g�r���[���X�V���邽�߁A�f���Q�[�g�C�x���g�𔭐�������
            this.RefreshEvent(this, new EventArgs());

            // �R���g���[���̖�����
            disableControlsExceptCloseButton(this);

            // �o�^�����t���O�I���i����ĕ��邱�Ƃ�h�~����@�\�A�I�t�j
            b_headerComplete = true;
            b_operatorComplete = true;
            b_partsComplete = true;
            b_subMatComplete = true;

            // �s�a�h�e�[�u���̃p�[�c�����폜���邽�߁A�Q�Ƃ��ׂ��s�a�h�e�[�u��������肷��
            //string model = cmbModelNo.Text;
            //DateTime month = dtpBatchDate.Value;
            //string tbiTable = decideReferenceTable(model, month);

            //// �s�a�h�e�[�u���̓��o�b�`�������R�[�h�폜
            //string sql2 = "delete from " + tbiTable + " where lot = '" + batchNo + "'";
            //System.Diagnostics.Debug.Print(sql2);
            //tf.sqlExecuteNonQueryToPqmDb(sql2, false);
        }

        // �t�H�[���N���[�Y�{�^���������̏���
        private void btnClose_Click(object sender, EventArgs e)
        {
            // �X�V���[�h�̏ꍇ�́A�����Ȃ��B
            if (!formAddMode) this.Close();

            // �w�b�_�[�E�I�y���[�^�[�E���i�E�����ށA�S�X�e�b�v���������Ă��邩�ۂ��A�m�F
            if (!(b_headerComplete && b_operatorComplete && b_partsComplete && b_subMatComplete))
            {
                string imcompleteList =
                    (b_headerComplete ? string.Empty : "header, ") +
                    (b_operatorComplete ? string.Empty : "operator, ") +
                    (b_partsComplete ? string.Empty : "parts, ") +
                    (b_subMatComplete ? string.Empty : "sub material, ");

                string message = "Is it ok to close this form?" + System.Environment.NewLine + System.Environment.NewLine +
                    "Please check the followings:" + System.Environment.NewLine +
                    VBStrings.Left(imcompleteList, imcompleteList.Length - 2) + System.Environment.NewLine + System.Environment.NewLine +
                    "Click Yes to close." + System.Environment.NewLine +
                    "Click No to check.";

                DialogResult reply = MessageBox.Show(message, "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (reply == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        // �T�u�v���V�[�W��: �R���g���[���̖�����
        private void disableControlsExceptCloseButton(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                // ����{�^���ȊO�͖����ɂ���
                if (child.Name != "btnClose")
                    child.Enabled = false;

                // �e�L�X�g�{�b�N�X�ƃR���{�{�b�N�X�̓N���A
                if (child is TextBox || child is ComboBox)
                    child.Text = string.Empty;
            }

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q�͓�����������
            dtpBatchDate.Value = DateTime.Today;
            dtpInputTime.Value = DateTime.Today;
            dtpOutputTime.Value = DateTime.Today;

            // �f�[�^�O���b�h�r���[�́A�f�[�^�\�[�X������
            dgvOperator.DataSource = "";
            dgvParts.DataSource = "";
            dgvSubMaterial.DataSource = "";
        }


        // �@�I�y���[�^�[��񂪃X�L�������ꂽ���̏���
        private void txtOperator_KeyDown(object sender, KeyEventArgs e)
        {
            //�G���^�[�L�[�ȊO�A���͕�����Ȃ��A�O���b�h�r���[�I���s�Ȃ��A�͏������Ȃ�
            if (e.KeyCode != Keys.Enter) return;

            string opId = txtOperator.Text;
            if (opId == string.Empty) return;

            int curRow = dgvOperator.CurrentCell.RowIndex;
            int curClm = dgvOperator.CurrentCell.ColumnIndex;
            if (curRow < 0) return;
            if (curClm != 1) return;

            DataGridViewTextBoxCell targetCell = (DataGridViewTextBoxCell)dgvOperator.Rows[curRow].Cells["operator"];
            string oldValue = targetCell.Value.ToString();
            string newValue = oldValue + ", " + opId;
            targetCell.Value = newValue;
            targetCell.Selected = true;

            // �A�����ăX�L�����ł���悤�A�e�L�X�g����ɂ���
            txtOperator.Text = string.Empty;
        }

        // �@�I�y���[�^�[�ҏW�L�����Z���{�^�����������ꂽ���̏���
        private void btnOperatorCancel_Click(object sender, EventArgs e)
        {
            readOperatorTable(ref dtOperator, ref dgvOperator, false);
        }

        // �@�I�y���[�^�[�ҏW�o�^�{�^�����������ꂽ���̏���
        private void btnOperatorRegister_Click(object sender, EventArgs e)
        {
            string batchNo = txtBatchNo.Text;
            TfSqlTracy Tfc = new TfSqlTracy();
            bool res = Tfc.sqlInsertOperatorInfo(batchNo, dtOperator);

            // �s�e�r�p�k�s�q�`�b�x����������������A�c�a���I�y���[�^�[�����f�[�^�O���b�h�r���[�ɕ\������
            if (res)
            {
                readOperatorTable(ref dtOperator, ref dgvOperator, false);
                b_operatorComplete = true;
                MessageBox.Show("Operator info register completed", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // �@�I�y���[�^�[��񃊃Z�b�g�{�^�����������ꂽ���̏���
        private void btnResetOperator_Click(object sender, EventArgs e)
        {
            showDefaultProcess(ref dtOperator, ref dgvOperator, false);
        }

        // �A�p�[�c��񂪃X�L�������ꂽ���̏���
        private void txtParts_KeyDown(object sender, KeyEventArgs e)
        {
            //�G���^�[�L�[�ȊO�A���͕�����Ȃ��A�O���b�h�r���[�I���s�Ȃ��A�͏������Ȃ�
            if (e.KeyCode != Keys.Enter) return;

            string partsInfo = txtParts.Text;
            if (partsInfo == string.Empty) return;

            int curRow = dgvParts.CurrentCell.RowIndex;
            int curClm = dgvParts.CurrentCell.ColumnIndex;
            if (curRow < 0) return;

            // �Z�~�R�����łp�q�ǂݎ����e�𕪊����A�O���b�g�r���[�ɕ\������
            try
            {
                string[] split = partsInfo.Split(';');
                int a = Check(split[0]);
                if (a == 0)
                {
                    dtParts.Rows[curRow]["parts_no"] = split[0];
                    dtParts.Rows[curRow]["parts_name"] = split[1];
                    dtParts.Rows[curRow]["parts_supplier"] = split[2];
                    dtParts.Rows[curRow]["parts_invoice"] = split[3];
                    double d;
                    double.TryParse(txtInputQty.Text, out d);
                    // ���i�ԍ������i�}�X�^�̏��ɊY�����邩�m�F���A�t����Ԃ�
                    double r = matchPartsNoAndGetRatio(split[0], dtPartsMaster);
                    dtParts.Rows[curRow]["qty"] = d * r; //split[5]; �o�b�`�S�̂h�m�o�t�s �p�s�x * �t��
                    dtParts.Rows[curRow]["note"] = "new";

                    // �A�����ăX�L�����ł���悤�A����̃Z����I������
                    dtParts.Rows.Add(dtParts.NewRow());
                    dgvParts.CurrentCell = dgvParts[curClm, curRow + 1];
                    dgvParts.NotifyCurrentCellDirty(true);
                    dgvParts.NotifyCurrentCellDirty(false);
                    // �A�����ăX�L�����ł���悤�A�e�L�X�g����ɂ���
                    txtParts.Text = string.Empty;
                }
                else
                {
                    txtParts.Text = string.Empty;
                    MessageBox.Show(split[0] + " is not in parts master.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            // �����ł��Ȃ�������̏ꍇ�A�G���[�̕\��
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public int Check(string parts)
        {
            TfSQL tf = new TfSQL();
            string strTemp = "Select count(*) from t_parts where model_no = N'" + cmbModelNo.Text + "'";
            int count = int.Parse(tf.sqlExecuteScalarString(strTemp).ToString());
            if (count.Equals(0))
            {
                return 1;
            }
            else
            {
                string strTemp1 = "Select count(*) from t_parts where model_no = N'" + cmbModelNo.Text + "' and sub_assy_no = N'" + cmbSubAssyNo.Text + "' and parts_no = N'" + parts + "'";

                int count1 = int.Parse(tf.sqlExecuteScalarString(strTemp1).ToString());
                if (count1.Equals(0))
                {
                    return 1;
                }
            }
            return 0;
        }
        public int Count()
        {
            TfSQL tf = new TfSQL();
            string str = "select count(parts_no) from t_parts where model_no = '" + cmbModelNo.Text + "' and sub_assy_no = '" + cmbSubAssyNo.Text + "'";
            int count2 = int.Parse(tf.sqlExecuteScalarString(str).ToString());
            return count2;
        }
        private double matchPartsNoAndGetRatio(string partsNo, DataTable dt)
        {
            if (partsNo == string.Empty) return 1;
            if (dt.Rows.Count == 0) return 1;

            matchPartsMaster = false;
            double ratio = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (partsNo.IndexOf(dt.Rows[i]["parts_no"].ToString()) >= 0)
                {
                    double.TryParse(dt.Rows[i]["ratio"].ToString(), out ratio);
                    dt.Rows[i]["match"] = "1";
                    matchPartsMaster = true;
                }
            }

            return ratio;
        }
        private void btnPartCancel_Click(object sender, EventArgs e)
        {
            readPartsTable(ref dtParts, ref dgvParts);
        }
        private void btnPartRegister_Click(object sender, EventArgs e)
        {
            readPartsMasterTable(ref dtPartsMaster);
            string list = showIncompleteScanPartsList(dtPartsMaster, dtParts);
            if (list != string.Empty)
            {
                // �X�L�����������ł��o�^�𑱍s���邩�A���[�U�[�ɖ₤
                MessageBox.Show("The following parts on the master has not been scanned:" +
                    System.Environment.NewLine + list,
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //insertBlankRecord(ref dtParts, ref dgvParts);
                return;
            }

            string batchNo = txtBatchNo.Text;
            TfSqlTracy Tfc = new TfSqlTracy();
            bool res = Tfc.sqlDeleteInsertPartsInfo(batchNo, dtParts);

            // �s�e�r�p�k�s�q�`�b�x���������������ꍇ�̏���
            if (res)
            {
                // �c�a���p�[�c�����f�[�^�O���b�h�r���[�ɕ\������
                readPartsTable(ref dtParts, ref dgvParts);

                MessageBox.Show("Parts info register completed", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // �ʃX���b�h�ŁA�s�a�h�e�[�u���ɓo�^����
                //registerPartsInfoToTbiTable();
            }
        }
        private string showIncompleteScanPartsList(DataTable dtm, DataTable dtp)
        {
            if (dtm.Rows.Count == 0) return string.Empty;
            if (dtp.Rows.Count == 0) return string.Empty;

            // ���i�}�X�^�[�c�`�s�`�s�`�a�k�d�̂l�`�s�b�g�t���O�ɂ��āA�S�X�L�����ϕ��i�ƍď���
            for (int i = 0; i < dtp.Rows.Count; i++)
            {
                string partsNo = dtp.Rows[i]["parts_no"].ToString();
                for (int j = 0; j < dtm.Rows.Count; j++)
                {
                    if (partsNo.IndexOf(dtm.Rows[j]["parts_no"].ToString()) >= 0)
                        dtm.Rows[j]["match"] = "1";
                }
            }

            // ���i�}�X�^�[�c�`�s�`�s�`�a�k�d�̂l�`�s�b�g�t���O�Ŗ��X�L�������i�𒊏o
            DataRow[] dr = dtm.Select("match is null");
            if (dr.Length == 0) return string.Empty;

            string incompleteList = string.Empty;
            for (int i = 0; i < dr.Length; i++)
            {
                incompleteList += (dr[i]["parts_no"].ToString() + " : " + dr[i]["parts_name"].ToString() + System.Environment.NewLine);
            }
            return incompleteList;
        }
        //private void registerPartsInfoToTbiTable()
        //{
        //    string batch = txtBatchNo.Text;
        //    string model =cmbModelNo.Text;
        //    DateTime batchDate = dtpBatchDate.Value;
        //    string tbiTable = decideReferenceTable(model, batchDate);

        //    TfSqlTracy Tfc = new TfSqlTracy();
        //    b_partsComplete = Tfc.sqlMultipleInsertPartsInfoToTbiTable
        //        (tbiTable, batch, model, batchDate, dtParts);

        //    if (b_partsComplete)
        //        MessageBox.Show("Step 3: Parts info register completed", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        private string decideReferenceTable(string model, DateTime month)
        {
            string tablekey = String.Empty;
            switch (model)
            {
                case "LA14G-371VC":
                    tablekey = "la14g_371vc";
                    break;
                case "LA14G-371VH":
                    tablekey = "la14g_371vh";
                    break;
                case "LA14G-371VJ":
                    tablekey = "la14g_371vj";
                    break;
                case "LA16-498A":
                    tablekey = "la16_498a";
                    break;
                case "LA16-498C":
                    tablekey = "la16_498c";
                    break;
                case "LA16-498D":
                    tablekey = "la16_498d";
                    break;
                default:
                    tablekey = "ls16";
                    break;
            }
            string tbiTable = tablekey + month.ToString("yyyyMM") + "tbi";
            return tbiTable;
        }
        public int Check_Sub(string sub_parts)
        {
            TfSQL tf = new TfSQL();
            string strTemp = "Select count(*) from t_sub_parts where model_no = N'" + cmbModelNo.Text + "'";
            int count3 = int.Parse(tf.sqlExecuteScalarString(strTemp).ToString());
            if (count3.Equals(0))
            {
                return 1;
            }
            else
            {
                string strTemp1 = "Select count(*) from t_sub_parts where model_no = N'" + cmbModelNo.Text + "' and sub_assy_no = N'" + cmbSubAssyNo.Text + "' and sub_parts_no = N'" + sub_parts + "'";

                int count4 = int.Parse(tf.sqlExecuteScalarString(strTemp1).ToString());
                if (count4.Equals(0))
                {
                    return 1;
                }
            }
            return 0;
        }
        public int Count_Sub()
        {
            TfSQL tf = new TfSQL();
            string str = "select count(sub_parts_no) from t_sub_parts where model_no = '" + cmbModelNo.Text + "' and sub_assy_no = '" + cmbSubAssyNo.Text + "'";
            int count5 = int.Parse(tf.sqlExecuteScalarString(str).ToString());
            return count5;
        }
        // �B�����ޏ�񂪃X�L�������ꂽ���̏���
        private void txtSubMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            //�G���^�[�L�[�ȊO�A���͕�����Ȃ��A�O���b�h�r���[�I���s�Ȃ��A�͏������Ȃ�
            if (e.KeyCode != Keys.Enter) return;

            string submatInfo = txtSubMaterial.Text;
            if (submatInfo == string.Empty) return;

            int curRow = dgvSubMaterial.CurrentCell.RowIndex;
            int curClm = dgvParts.CurrentCell.ColumnIndex;

            if (curRow < 0) return;

            // �Z�~�R�����łp�q�ǂݎ����e�𕪊����A�O���b�g�r���[�ɕ\������
            try
            {
                string[] split = submatInfo.Split(';');
                int c = Check_Sub(split[0]);
                if (c == 0)
                {
                    dtSubMaterial.Rows[curRow]["sub_mat_no"] = split[0];
                    dtSubMaterial.Rows[curRow]["sub_mat_name"] = split[1];
                    dtSubMaterial.Rows[curRow]["sub_mat_supplier"] = split[2];
                    dtSubMaterial.Rows[curRow]["sub_mat_invoice"] = split[3];
                    DateTime date;
                    if (DateTime.TryParse(split[6], out date))
                        dtSubMaterial.Rows[curRow]["validity"] = DateTime.ParseExact(split[6], "yyyy/MM/dd", CultureInfo.InvariantCulture);

                    dtSubMaterial.Rows.Add(dtSubMaterial.NewRow());
                    dgvSubMaterial.CurrentCell = dgvSubMaterial[curClm, curRow + 1];
                    dgvSubMaterial.NotifyCurrentCellDirty(true);
                    dgvSubMaterial.NotifyCurrentCellDirty(false);

                    txtSubMaterial.Text = string.Empty;
                }
                else
                {
                    txtSubMaterial.Text = string.Empty;
                    MessageBox.Show(split[0] + " is not in sub material master.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSubMaterialCancel_Click(object sender, EventArgs e)
        {
            readSubMaterialTable(ref dtSubMaterial, ref dgvSubMaterial);
        }
        private string showIncompleteScanSubPartsList(DataTable dtm, DataTable dtp)
        {
            if (dtm.Rows.Count == 0) return string.Empty;
            if (dtp.Rows.Count == 0) return string.Empty;

            // ���i�}�X�^�[�c�`�s�`�s�`�a�k�d�̂l�`�s�b�g�t���O�ɂ��āA�S�X�L�����ϕ��i�ƍď���
            for (int i = 0; i < dtp.Rows.Count; i++)
            {
                string partsNo = dtp.Rows[i]["sub_mat_no"].ToString();
                for (int j = 0; j < dtm.Rows.Count; j++)
                {
                    if (partsNo.IndexOf(dtm.Rows[j]["sub_parts_no"].ToString()) >= 0)
                        dtm.Rows[j]["match"] = "1";
                }
            }

            // ���i�}�X�^�[�c�`�s�`�s�`�a�k�d�̂l�`�s�b�g�t���O�Ŗ��X�L�������i�𒊏o
            DataRow[] dr = dtm.Select("match is null");
            if (dr.Length == 0) return string.Empty;

            string incompleteList = string.Empty;
            for (int i = 0; i < dr.Length; i++)
            {
                incompleteList += (dr[i]["sub_parts_no"].ToString() + " : " + dr[i]["sub_parts_name"].ToString() + System.Environment.NewLine);
            }
            return incompleteList;
        }

        private void btnSubMaterialRegister_Click(object sender, EventArgs e)
        {
            readSubPartsMasterTable(ref dtSubPartsMaster);
            string list1 = showIncompleteScanSubPartsList(dtSubPartsMaster, dtSubMaterial);
            if (list1 != string.Empty)
            {
                MessageBox.Show("The following sub parts on the master has not been scanned:" +
                    System.Environment.NewLine + list1,
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //insertBlankRecord(ref dtSubMaterial, ref dgvSubMaterial);
                return;
            }

            string batchNo = txtBatchNo.Text;
            TfSqlTracy Tfc = new TfSqlTracy();
            bool res = Tfc.sqlDeleteInsertSubMaterialInfo(batchNo, dtSubMaterial);

            if (res)
            {
                readSubMaterialTable(ref dtSubMaterial, ref dgvSubMaterial);

                MessageBox.Show("Sub material info register completed!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //registerSubMaterialInfoToTbiTable();           
            }
        }

        //private void registerSubMaterialInfoToTbiTable()
        //{
        //    string batch = txtBatchNo.Text;
        //    string model = cmbModelNo.Text;
        //    DateTime batchDate = dtpBatchDate.Value;
        //    string tbiTable = decideReferenceTable(model, batchDate);

        //    TfSqlTracy Tfc = new TfSqlTracy();
        //    b_subMatComplete = Tfc.sqlMultipleInsertSubMaterialInfoToTbiTable(tbiTable, batch, model, batchDate, dtSubMaterial);

        //    if (b_subMatComplete)
        //        MessageBox.Show("Step 4: Sub Material info register completed", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        private string getNewBatch()
        {
            string subAssyNo = cmbSubAssyNo.Text;
            string modelNo = cmbModelNo.Text;
            string sql;
            TfSQL tf = new TfSQL();

            sql = "select batch_prefix from t_model_sub_assy where sub_assy_no = '" + subAssyNo + "' and model_no = '" + modelNo + "'";
            string batchPrefix = tf.sqlExecuteScalarString(sql);

            sql = "select max(batch_no) from t_batch_no where batch_no like '" + batchPrefix + "%'";
            string batchOld = tf.sqlExecuteScalarString(sql);

            DateTime dateOld = new DateTime(0);
            long numberOld = 0;
            string batchNew;

            if (batchOld != string.Empty)
            {
                dateOld = DateTime.ParseExact(VBStrings.Mid(batchOld, 12, 6), "yyMMdd", CultureInfo.InvariantCulture);
                numberOld = long.Parse(VBStrings.Right(batchOld, 4));
            }

            if (dateOld != DateTime.Today)
            {
                batchNew = batchPrefix + "#" + DateTime.Today.ToString("yyMMdd") + "#" + "0001";
            }
            else
            {
                batchNew = batchPrefix + "#" + DateTime.Today.ToString("yyMMdd") + "#" + (numberOld + 1).ToString("0000");
            }

            return batchNew;
        }

        private void insertBatchNo()
        {
            string batchNo = txtBatchNo.Text;
            string modelNo = cmbModelNo.Text;
            string modelName = txtModelName.Text;
            string subAssyNo = cmbSubAssyNo.Text;
            string subAssyName = txtSubAssyName.Text;
            DateTime batchDate = dtpBatchDate.Value;
            string shift = cmbShift.Text;
            string line = cmbLine.Text;
            string leader = txtLeaderId.Text;
            string leaderName = txtLeaderName.Text;
            double inQty;
            double.TryParse(txtInputQty.Text, out inQty);
            double outQty;
            double.TryParse(txtOutputQty.Text, out outQty);
            DateTime inTime = dtpInputTime.Value;
            DateTime outTime = dtpOutputTime.Value;
            string remark = txtRemark.Text;

            bool[] cr = { batchNo           == string.Empty ? false : true,
                          modelNo           == string.Empty ? false : true,
                          modelName         == string.Empty ? false : true,
                          subAssyNo         == string.Empty ? false : true,
                          subAssyName       == string.Empty ? false : true,
                                                                      true,
                          shift             == string.Empty ? false : true,
                          line              == string.Empty ? false : true,
                          leader            == string.Empty ? false : true,
                          leaderName        == string.Empty ? false : true,
                          txtInputQty.Text  == string.Empty ? false : true,
                          txtOutputQty.Text == string.Empty ? false : true,
                                                                      true,
                                                                      true,
                          remark            == string.Empty ? false : true, };

            string sql1 = "insert into t_batch_no(" +
                (cr[0] ? "batch_no," : string.Empty) +
                (cr[1] ? "model_no," : string.Empty) +
                (cr[2] ? "model_name," : string.Empty) +
                (cr[3] ? "sub_assy_no," : string.Empty) +
                (cr[4] ? "sub_assy_name," : string.Empty) +
                (cr[5] ? "batch_date," : string.Empty) +
                (cr[6] ? "shift," : string.Empty) +
                (cr[7] ? "line," : string.Empty) +
                (cr[8] ? "leader_id," : string.Empty) +
                (cr[9] ? "leader_name," : string.Empty) +
                (cr[10] ? "in_qty," : string.Empty) +
                (cr[11] ? "out_qty," : string.Empty) +
                (cr[12] ? "in_time," : string.Empty) +
                (cr[13] ? "out_time," : string.Empty) +
                (cr[14] ? "remark," : string.Empty);

            string sql2 = ") VALUES(" +
                (cr[0] ? "'" + batchNo + "'," : string.Empty) +
                (cr[1] ? "'" + modelNo + "'," : string.Empty) +
                (cr[2] ? "'" + modelName + "'," : string.Empty) +
                (cr[3] ? "'" + subAssyNo + "'," : string.Empty) +
                (cr[4] ? "'" + subAssyName + "'," : string.Empty) +
                (cr[5] ? "'" + batchDate + "'," : string.Empty) +
                (cr[6] ? "'" + shift + "'," : string.Empty) +
                (cr[7] ? "'" + line + "'," : string.Empty) +
                (cr[8] ? "'" + leader + "'," : string.Empty) +
                (cr[9] ? "'" + leaderName + "'," : string.Empty) +
                (cr[10] ? " " + inQty + " ," : string.Empty) +
                (cr[11] ? " " + outQty + " ," : string.Empty) +
                (cr[12] ? "'" + inTime + "'," : string.Empty) +
                (cr[13] ? "'" + outTime + "'," : string.Empty) +
                (cr[14] ? "'" + remark + "'," : string.Empty);

            string sql3 = VBStrings.Left(sql1, sql1.Length - 1) + VBStrings.Left(sql2, sql2.Length - 1) + ")";
            System.Diagnostics.Debug.Print(sql3);
            TfSQL tf = new TfSQL();
            tf.sqlExecuteNonQuery(sql3, false);

            this.RefreshEvent(this, new EventArgs());
        }

        private string aliasName = "MediaFile";
        private void soundAlarm()
        {
            string currentDir = System.Environment.CurrentDirectory;
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
        // Windows API ���C���|�[�g
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        // �ݒ�e�L�X�g�t�@�C���̓ǂݍ���
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

        // Windows API ���C���|�[�g
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        // ����{�^����V���[�g�J�b�g�ł̏I���������Ȃ�
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }

        private void txtInputQty_KeyDown(object sender, KeyEventArgs e)
        {
            double t;
            double.TryParse(txtInputQty.Text, out t);

            if (e.KeyCode == Keys.Enter)
            {
                TfSQL ts = new TfSQL();
                for (int i = 0; i < dgvParts.Rows.Count - 1; i++)
                {
                    string partno = dgvParts.Rows[i].Cells[0].Value.ToString();
                    string ratio_sql = "select ratio from t_parts where parts_no = '" + partno + "'";
                    double o = ts.sqlExecuteScalarDouble(ratio_sql);
                    dgvParts.Rows[i].Cells[4].Value = t * o;
                }
                txtOutputQty.Focus();
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            rdCheck.Checked = true;
            TfSQL yn = new TfSQL();
            yn.sqlExecuteScalarString("update t_batch_no set check_by = '" + leadername + "' where batch_no = '" + txtBatchNo.Text + "'");
            string checker = yn.sqlExecuteScalarString("select check_by from t_batch_no where batch_no = '" + txtBatchNo.Text + "'");
            rdCheck.Text = "Checked by: " + checker;
            btnDeleteBatch.Enabled = false;
            btnUpdateBatch.Enabled = false;
        }
    }
}