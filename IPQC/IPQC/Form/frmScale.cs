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
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks; 

namespace IPQC
{
    public partial class frmScale : Form
    {
        //�e�t�H�[��Form1�փC�x���g������A������A�f���Q�[�g�ϐ�
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // �f�[�^��M�C�x���g�p�A�f���Q�[�g�ϐ�
        private delegate void Delegate_RcvDataToBufferDataTable(string data);

        //�f�[�^�O���b�h�r���[�p�{�^��
        DataGridViewButtonColumn edit1;
        DataGridViewButtonColumn edit2;
        DataGridViewButtonColumn edit3;
        DataGridViewButtonColumn edit4;
        DataGridViewButtonColumn edit5;
        DataGridViewButtonColumn Open;
        DataGridViewButtonColumn Delete;

        //�f�[�^�O���b�h�r���[�{�^���̈ʒu�ƃ}�b�`����f�[�^�e�[�u���̃A�h���X�i�u�c�A�g���j
        int vAdr;
        string hAdr;
        string _ip;

        //���̑��A�񃍁[�J���ϐ�
        const string command1 = "PRT";
        const string command2 = "ST";
        const string command3 = "R";
        double upp;
        double low;
        bool editMode;
        DataTable dtBuffer;
        DataTable dtHistory;
        DataTable dtUpLowIns;
        int clmSet = 5;
        int rowSet = 1;

        // �R���X�g���N�^
        public frmScale()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void frmScale_Load(object sender, EventArgs e)
        {
            // ���t�H�[���̕\���ꏊ���w��
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

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q���P�O���O�̓��t�ɂ���
            dtpSet10daysBefore(dtpLotFrom);

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���؂�グ��
            dtpRoundUpHour(dtpLotTo);

            // �c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���������
            dtpRounddownHour(dtpLotInput);

            // ���m���ꂽ�P�ڂ̃V���A���|�[�g��I�����A�I�[�v������
            initializePort();

            // �e�폈���p�̃e�[�u���𐶐����ăf�[�^��ǂݍ���
            dtBuffer = new DataTable();
            defineBufferAndHistoryTable(ref dtBuffer);
            dtHistory = new DataTable();
            defineBufferAndHistoryTable(ref dtHistory);
            readDtHistory(ref dtHistory);
            dtUpLowIns = new DataTable();
            setLimitSetAndCommand(ref dtUpLowIns);

            //addButtonsToDataGridView(dgvHistory);
            // �O���b�g�r���[�̍X�V
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

            // �O���b�g�r���[�E�[�Ƀ{�^����ǉ��i����̂݁j
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

        // �T�u�v���V�[�W���F�c�a����̂c�s�g�h�r�s�n�q�x�ւ̓ǂݍ���
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
                         "where model = '" + model + "' AND " +
                                "process = '" + process + "' AND " +
                                "inspect = '" + inspect + "' AND " +
                                "lot >= '" + lotFrom.ToString() + "' AND " +
                                "lot <= '" + lotTo.ToString() + "' AND " +
                                "line = '" + line + "' AND " +
                                "qc_user != '1. Upper' AND qc_user != '2. Lower' " +
                         "order by lot, inspectdate";

            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.sqlDataAdapterFillDatatable(sql, ref dt);
        }

        // �T�u�v���V�[�W���F�����e�[�u���̒�`
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

        // �T�u�v���V�[�W���F����E�����A�s�Z�b�g�E��Z�b�g�A�R�}���h�A�̐ݒ�
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
        }

        // �T�u�v���V�[�W���F�f�[�^�O���b�g�r���[�̍X�V
        private void updateDataGripViews(DataTable dt1, DataTable dt2, ref DataGridView dgv1, ref DataGridView dgv2)
        {
            // �f�[�^�O���b�g�r���[�ւc�s�`�`�s�`�a�k�d���i�[
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

            // �X�y�b�N�O�̃Z�����}�[�L���O����
            colorHistoryViewBySpec(dtHistory, ref dgvHistory);

            // ��ԉ��̍s��\������
            if (dgv2.Rows.Count >= 1)
                dgv2.FirstDisplayedScrollingRowIndex = dgv2.Rows.Count - 1;
        }

        // �����{�^���������A�f�[�^��ǂݍ��݁A�c�`�s�`�f�q�h�c�u�h�d�v���X�V����
        private void btnSearch_Click(object sender, EventArgs e)
        {
            readDtHistory(ref dtHistory);
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);
        }

        // �T�u�T�u�v���V�[�W���F�O���b�g�r���[�E�[�Ƀ{�^����ǉ�
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

        // ����l�̐V�K�o�^
        private void btnMeasure_Click(object sender, EventArgs e)
        {
            // �ҏW���[�h�t���O�������A�o�^�E�C���{�^�����u�o�^�v�̕\���ɂ���
            editMode = false;
            btnRegister.Text = "Register";
            dtpLotInput.Enabled = true;

            // �g�h�r�s�n�q�x�f�[�^�O���b�h�r���[�̃}�[�L���O���N���A����
            colorViewReset(ref dgvHistory);
            colorViewReset(ref dgvBuffer);

            // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[������������
            dtBuffer.Clear();

            // ����A�����A���ق̍s�̕\��
            for (int i = 1; i <= 3; i++)
            {
                DataRow dr = dtBuffer.NewRow();
                dr["inspect"] = txtInspect.Text;
                dr["lot"] = dtpLotInput.Value;
                dr["inspectdate"] = DateTime.Now;
                dr["line"] = cmbLine.Text;
                if (i == 1) dr["qc_user"] = "1. Upper";
                else if (i == 2) dr["qc_user"] = "2. Lower";
                else if (i == 3) dr["qc_user"] = txtUser.Text;
                dtBuffer.Rows.Add(dr);
            }

            // �O���b�g�r���[�̍X�V
            updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

            // �V�K�o�^�p�O���b�g�r���[�i�o�b�t�@�e�[�u���j�ցA�{�^����ǉ�����
            if (dgvBuffer.Columns.Count <= 13)
            {
                addButtonsToDgvBuffer(dgvBuffer, edit1, edit2, edit3, edit4, edit5);
            }
        }

        // ��������l�̏C��
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int curRow = int.Parse(e.RowIndex.ToString());

            if (dgvHistory.Columns[e.ColumnIndex] == Open && curRow >= 0)
            {
                // �ҏW���[�h�t���O�𗧂āA�o�^�E�C���{�^�����u�C���v�̕\���ɂ���
                editMode = true;
                btnRegister.Text = "Update";
                dtpLotInput.Enabled = false;

                // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[�����������A�{�^���ɑΉ�����l���i�[����
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

                // �O���b�g�r���[�̍X�V
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �V�K�o�^�p�O���b�g�r���[�i�o�b�t�@�e�[�u���j�ցA�{�^����ǉ�����
                if (dgvBuffer.Columns.Count <= 13)
                {
                    addButtonsToDgvBuffer(dgvBuffer, edit1, edit2, edit3, edit4, edit5);
                }

                // �ύX�^�[�Q�b�g�s��\������
                if (dgvHistory.Rows.Count >= 1)
                    dgvHistory.FirstDisplayedScrollingRowIndex = curRow;

                // �T�u�v���V�[�W���F�ҏW���̍s���}�[�L���O����
                colorViewForEdit(ref dgvHistory, curRow);
                colorViewForEdit(ref dgvBuffer, 0);
            }
        }

        // �T�u�v���V�[�W���F�ҏW���̍s���}�[�L���O����
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

        // �T�u�v���V�[�W���F�}�[�L���O���N���A����
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

        // �T�u�v���V�[�W���F�X�y�b�N�O�̃Z�����}�[�L���O����i�����e�[�u���j
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

        // �T�u�v���V�[�W���F�X�y�b�N�O�̃Z�����}�[�L���O����i�ꎞ�e�[�u���j
        private void colorBufferViewBySpec(double value, ref DataGridView dgv)
        {
            // �Ԓl�̊i�[��f�[�^�e�[�u���A�Z���Ԓn��񃍁[�J���ϐ��֊i�[
            int clm = 0;
            if (hAdr == "m1") clm = 7;
            else if (hAdr == "m2") clm = 9;
            else if (hAdr == "m3") clm = 11;
            else if (hAdr == "m4") clm = 13;
            else if (hAdr == "m5") clm = 15;

            if (value >= double.Parse(txtLsl.Text) && value <= double.Parse(txtUsl.Text)) //|| dgv[clm - 1, 0].Value.ToString() == String.Empty || dgv[clm - 1, 1].Value.ToString() == String.Empty)
            {
                dgv[clm - 1, 2].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
            else
            {
                dgv[clm - 1, 2].Style.BackColor = Color.Red;
            }
        }

        // ����l�̎�荞�݂��I�������A�f�[�^�x�[�X�֓o�^����
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (dtBuffer.Rows.Count <= 0) return;

            string model = txtModel.Text;
            string process = txtProcess.Text;
            string inspect = txtInspect.Text;
            string status = txtStatus.Text;
            DateTime lot = DateTime.Parse(dtBuffer.Rows[0]["lot"].ToString()); ;
            DateTime inspectdate = DateTime.Parse(dtBuffer.Rows[0]["inspectdate"].ToString()); ;
            string line = cmbLine.Text;

            // �u�@�b�t�@�[�e�[�u�����ŁA���ςƃ����W���v�Z����
            calculateAverageAndRangeInDataTable(ref dtBuffer);

            // �h�o�p�b�c�a ���藚���e�[�u���ɓo�^����
            TfSQL tf = new TfSQL();
            bool res = tf.sqlMultipleInsert(model, process, inspect, lot, inspectdate, line, status, dtBuffer);

            if (res)
            {
                // �o�b�N�O���E���h�ło�p�l�e�[�u���ɓo�^����
                DataTable dtTemp = new DataTable();
                dtTemp = dtBuffer.Copy();
                //registerMeasurementToPqmTable(dtTemp);

                // �o�^�ς̏�Ԃ��A���t�H�[���ɕ\������
                dtBuffer.Clear();
                readDtHistory(ref dtHistory);
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �ҏW���[�h�t���O�𗧂āA�o�^�E�C���{�^�����u�o�^�v�̕\���ɖ߂�
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // �T�u�v���V�[�W��: �f�[�^�e�[�u�������ŁA���ςƃ����W�i�ő�|�ŏ��j�����߁A�i�[����
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

        // �T�u�v���V�[�W��: �o�p�l�e�[�u���ւ̓o�^�i�o�b�N�O���E���h�����j
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

        //        TfSqlPqm Tfc = new TfSqlPqm();
        //        Tfc.sqlMultipleInsertMeasurementToPqmTable(model, process, inspect, lot, inspectdate, line, dt, upp, low);
        //    });
        //}

        // �폜�{�^���������̏���
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
                // �f�[�^�̍폜
                string sql = "delete from tbl_measure_history where " +
                    "model='" + txtModel.Text + "' and " +
                    "inspect='" + txtInspect.Text + "' and " +
                    "lot ='" + dtBuffer.Rows[0]["lot"] + "' and " +
                    "inspectdate ='" + dtBuffer.Rows[0]["inspectdate"] + "' and " +
                    "line ='" + cmbLine.Text + "'";

                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                int res = tf.sqlExecuteNonQueryInt(sql, false);

                // �o�b�N�O���E���h�ło�p�l�e�[�u�����̍폜
                DataTable dtTemp = new DataTable();
                dtTemp = dtBuffer.Copy();
               // deleteFromPqmTable(dtTemp);

                // �V�K�o�^�p�o�b�t�@�[�e�[�u���A�o�b�t�@�[�O���b�g�r���[������������
                dtBuffer.Clear();

                // �폜��e�[�u���̍ēǂݍ���
                readDtHistory(ref dtHistory);

                // �g�h�r�s�n�q�x�f�[�^�O���b�h�r���[�̃}�[�L���O���N���A����
                colorViewReset(ref dgvHistory);

                // �O���b�g�r���[�̍X�V
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �ҏW���[�h�t���O�������A�o�^�E�C���{�^�����u�o�^�v�̕\���ɂ���
                editMode = false;
                btnRegister.Text = "Register";
                dtpLotInput.Enabled = true;
            }
        }

        // �T�u�v���V�[�W��: �o�p�l�e�[�u���ł̍폜�i�o�b�N�O���E���h�����j
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

        // �L�����Z���{�^���������̏���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // �O���b�g�r���[��̃{�^���N���b�N���A�d�q�V���Ƃ̒ʐM���J�n����B���������̓f���Q�[�g�B
        private void dgvBuffer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int curRow = int.Parse(e.RowIndex.ToString());
            int curClm = int.Parse(e.ColumnIndex.ToString());

            if ((curClm == 7 || curClm == 9 || curClm == 11 || curClm == 13 || curClm == 15) && curRow >= 0)
            {
                // �Ԓl�̊i�[��f�[�^�e�[�u���A�Z���Ԓn��񃍁[�J���ϐ��֊i�[
                vAdr = curRow;

                if (curClm == 7) hAdr = "m1";
                else if (curClm == 9) hAdr = "m2";
                else if (curClm == 11) hAdr = "m3";
                else if (curClm == 13) hAdr = "m4";
                else if (curClm == 15) hAdr = "m5";

                // ����l�̑��M�v�����s��
                sendCmdPeakReuirement();
            }
        }

        // �T�u�v���V�[�W���F����l�̑��M�v�����s��
        private void sendCmdPeakReuirement()
        {
            // �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
            if (serialPort1.IsOpen == false) return;

            try
            {
                string cmd = command1 + System.Environment.NewLine;
                serialPort1.Write(cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �f�[�^��M�����������Ƃ��̃C�x���g�����i�f���Q�[�g���j
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //! �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
            if (serialPort1.IsOpen == false) return;

            try
            {
                //�@�X���[�v�����邱�ƂŁA�P�Z�b�g�̐M���̑��M���I���܂ő҂�
                Thread.Sleep(100);

                //! ��M�R�}���h��ǂݍ��݁A�f���Q�[�g����
                string cmd = serialPort1.ReadExisting();
                Invoke(new Delegate_RcvDataToBufferDataTable(RcvDataToBufferDataTable), new Object[] { cmd });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �f�[�^��M�����������Ƃ��̃C�x���g�����i�f���Q�[�g��j
        private void RcvDataToBufferDataTable(string cmd)
        {
            // ����l�̑��M�v���ɑ΂���ԐM�̏ꍇ�̂ݏ���
            //if (cmd.Length != 17) return;

            // �����̌���F�������ڂɉ����āA�Q�̑���l�̍����A�����Ŋ����ĕ\���E�o�^����i�f�t�H���g�����͂P�j
            string inspect = txtInspect.Text;
            double factor = 1;
            List<string> list10 = new List<string> { "CAFBVP2", "CAFBVP4", "CAFBVP2", "CAFBVP4", "CAFBVP2", "CAFBVP4" };
            List<string> list5 = new List<string> { "RBCAVL", "RBCAVL", "RBCAVL" };
            if (list10.Contains(inspect)) factor = 10;
            if (list5.Contains(inspect)) factor = 5;

            //�@�����������񂩂�n�܂��M������̂ݏ�������
            if (cmd.Substring(0, 2) == command2)
            {
                // ����l�̃e�L�X�g���A�c�n�t�a�k�d�ɕϊ����Ăa�t�e�e�d�q�s�`�a�k�d�֊i�[����
                double value = 0;
                string sql = "select dbplace from tbl_model_dbplace where model='" + txtModel.Text + "'";
                System.Diagnostics.Debug.Print(sql);
                TfSQL tf = new TfSQL();
                string dbplace = tf.sqlExecuteScalarString(sql);

                // �g�`�x�v�`�q�c�Q�͂P�O���̐��x�A�b�`�q�͂W���̐��x
                if (dbplace == "A")
                    double.TryParse(cmd.Substring(4, 8), out value);
                else if (dbplace == "CAR")
                    double.TryParse(cmd.Substring(4, 8), out value);

                dtBuffer.Rows[vAdr][hAdr] = value;

                // ��ʂƉ��ʂ́A�Q�̑���l�̍����A�R�s�ڂɕ\������
                if (dtBuffer.Rows[0][hAdr].ToString() != String.Empty && dtBuffer.Rows[1][hAdr].ToString() != String.Empty)
                {
                    dtBuffer.Rows[2][hAdr] =
                        Math.Round((double.Parse(dtBuffer.Rows[0][hAdr].ToString()) - double.Parse(dtBuffer.Rows[1][hAdr].ToString())) / factor, 4);
                    value = double.Parse(dtBuffer.Rows[2][hAdr].ToString());
                }

                // �O���b�g�r���[�̍X�V
                updateDataGripViews(dtBuffer, dtHistory, ref dgvBuffer, ref dgvHistory);

                // �X�y�b�N�O�̃Z�����}�[�L���O����i�ꎞ�e�[�u���j
                colorBufferViewBySpec(value, ref dgvBuffer);
            }
        }

        // �s�[�N�l���N���A����R�}���h�𑗐M
        private void btnZero_Click(object sender, EventArgs e)
        {
            // �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
            if (serialPort1.IsOpen == false) return;

            try
            {
                string cmd = command3 + System.Environment.NewLine;
                serialPort1.Write(cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �T�u�v���V�[�W��: �V�K�o�^�p�O���b�g�r���[�i�c�f�u�o�b�t�@�[�j�ցA�{�^����ǉ�����
        private void addButtonsToDgvBuffer(DataGridView dgv, DataGridViewButtonColumn b1, DataGridViewButtonColumn b2,
            DataGridViewButtonColumn b3, DataGridViewButtonColumn b4, DataGridViewButtonColumn b5)
        {
            b1 = new DataGridViewButtonColumn();
            b1.Text = "Edit";
            b1.UseColumnTextForButtonValue = true;
            b1.Width = 80;
            dgv.Columns.Insert(7, b1);

            b2 = new DataGridViewButtonColumn();
            b2.Text = "Edit";
            b2.UseColumnTextForButtonValue = true;
            b2.Width = 80;
            dgv.Columns.Insert(9, b2);

            b3 = new DataGridViewButtonColumn();
            b3.Text = "Edit";
            b3.UseColumnTextForButtonValue = true;
            b3.Width = 80;
            dgv.Columns.Insert(11, b3);

            b4 = new DataGridViewButtonColumn();
            b4.Text = "Edit";
            b4.UseColumnTextForButtonValue = true;
            b4.Width = 80;
            dgv.Columns.Insert(13, b4);

            b5 = new DataGridViewButtonColumn();
            b5.Text = "Edit";
            b5.UseColumnTextForButtonValue = true;
            b5.Width = 80;
            dgv.Columns.Insert(15, b5);

            // �R�s�ڂ̃{�^��������
            dgv[7, 2] = new DataGridViewTextBoxCell();
            dgv[9, 2] = new DataGridViewTextBoxCell();
            dgv[11, 2] = new DataGridViewTextBoxCell();
            dgv[13, 2] = new DataGridViewTextBoxCell();
            dgv[15, 2] = new DataGridViewTextBoxCell();
        }

        // �T�u�v���V�[�W��: ���m���ꂽ�P�ڂ̃V���A���|�[�g��I�����A�I�[�v������
        private void initializePort()
        {
            // ���p�\�ȃV���A���|�[�g���̔z����擾����
            string[] PortList = SerialPort.GetPortNames();

            // �V���A���|�[�g�����R���{�{�b�N�X�ɃZ�b�g����
            cmbPortName.Items.Clear();

            foreach (string PortName in PortList)
                cmbPortName.Items.Add(PortName);

            // ���m���ꂽ�|�[�g�̂P�ڂ�I�����A�R���{�{�b�N�X�ύX�C�x���g�ɂ��A�V���A���|�[�g���I�[�v������
            if (cmbPortName.Items.Count > 0)
                cmbPortName.SelectedIndex = 0;
        }

        // �|�[�g�I��p�R���{�{�b�N�X�ύX���A�V���A���|�[�g���I�[�v������
        private void cmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //! ���ɃI�[�v�����Ă���|�[�g������ꍇ�́A�������s��Ȃ�
            if (serialPort1.IsOpen) return;

            //! �I�[�v������V���A���|�[�g���R���{�{�b�N�X������o��
            serialPort1.PortName = cmbPortName.SelectedItem.ToString();
            serialPort1.BaudRate = 2400;
            serialPort1.DataBits = 7;
            serialPort1.Parity = Parity.Even;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Encoding = Encoding.ASCII;

            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // �_�C�A���O�̏I�������F�V���A���|�[�g���I�[�v�����Ă���ꍇ�A�N���[�Y����
        private void frmScale_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q���P�O���O�̓��t�ɂ���
        private void dtpSet10daysBefore(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value.Date.AddDays(-10);
            dtp.Value = dt;
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���؂�グ��
        private void dtpRoundUpHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddHours(1).AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // �T�u�T�u�v���V�[�W���F�c�`�s�d�s�h�l�d�o�h�b�j�d�q�̕��ȉ���������
        private void dtpRounddownHour(DateTimePicker dtp)
        {
            DateTime dt = dtp.Value;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            int millisecond = dt.Millisecond;
            dtp.Value = dt.AddMinutes(-minute).AddSeconds(-second).AddMilliseconds(-millisecond);
        }

        // �w�q�O���t�쐬�{�^���������̏���     
        private void btnExport_Click(object sender, EventArgs e)
        {
            TfSQL sampl = new TfSQL();
            string sample = sampl.sqlExecuteScalarString("select clm_set from tbl_measure_item where inspect = '" + txtInspect.Text + "'");
            string descrip = sampl.sqlExecuteScalarString("select description from tbl_measure_item where inspect = '" + txtInspect.Text + "'");
            ExcelClassnew xl = new ExcelClassnew();

            string dtpFrom = dtpLotFrom.Value.ToString("yyyy/MM/dd");
            string dtpTo = dtpLotTo.Value.ToString("yyyy/MM/dd");

            xl.exportExcel(txtModel.Text, cmbLine.Text, txtUser.Text, txtUsl.Text, txtLsl.Text, txtProcess.Text, txtInspect.Text, sample, descrip, dgvHistory, dtpFrom, dtpTo);
        }

        // �T�u�T�u�v���V�[�W���F�w�q�Ǘ��}�p�f�[�^�e�[�u���̐���      
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