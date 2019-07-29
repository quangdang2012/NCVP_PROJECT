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

namespace IPQC
{
    public partial class frmItemMaster : Form
    {
        //�e�t�H�[���փC�x���g������A���i�f���Q�[�g�j
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        //���̑��񃍁[�J���ϐ�
        NpgsqlConnection connection;
        NpgsqlCommand command;
        NpgsqlDataAdapter adapter;
        NpgsqlCommandBuilder cmdbuilder;
        DataSet ds;
        DataTable dt;
        string conStringBoxidDb = @"Server=192.168.145.4;Port=5432;User Id=pqm;Password=dbuser;Database=ip_pqmdb; CommandTimeout=100; Timeout=100;";
        string model;

        // �R���X�g���N�^
        public frmItemMaster(string p_model)
        {
            InitializeComponent();
            model = p_model;
        }

        // ���[�h���̏���
        private void Form7_2_Load(object sender, EventArgs e)
        {
            //�t�H�[���̏ꏊ���w��
            this.Left = 30;
            this.Top = 30;
            defineAndReadTable();
        }

        // �T�u�v���V�[�W���F�e�[�u�����`���A�c�a���f�[�^��ǂݍ���
        private void defineAndReadTable()
        {
            // �c�a���f�[�^��ǂݍ��݁A�c�s�`�`�s�`�a�k�d�֊i�[
            string sql = "select no, model, process, inspect, description, upper, lower, instrument, clm_set, row_set " +
                "from tbl_measure_item where model='" + model + "' order by no, process, inspect";
            connection = new NpgsqlConnection(conStringBoxidDb);
            connection.Open();
            command = new NpgsqlCommand(sql, connection);
            adapter = new NpgsqlDataAdapter(command);
            cmdbuilder = new NpgsqlCommandBuilder(adapter);
            adapter.InsertCommand = cmdbuilder.GetInsertCommand();
            adapter.UpdateCommand = cmdbuilder.GetUpdateCommand();
            adapter.DeleteCommand = cmdbuilder.GetDeleteCommand();
            ds = new DataSet();
            adapter.Fill(ds,"buff");
            dt = ds.Tables["buff"];
            
            // �f�[�^�O���b�g�r���[�ւc�s�`�`�s�`�a�k�d���i�[
            dgvTester.DataSource = dt;
            dgvTester.ReadOnly = true;
            btnSave.Enabled = false;
            dgvTester.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTester.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        // �V�K���R�[�h�̒ǉ�
        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvTester.ReadOnly = false;
            dgvTester.AllowUserToAddRows = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
        }

        // �������R�[�h�̍폜
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Do you want to delete this row ?", "Delete", MessageBoxButtons.YesNo);
            if (dlg == DialogResult.No) return;

            try
            {
                dgvTester.Rows.RemoveAt(dgvTester.SelectedRows[0].Index);
                adapter.Update(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // �ۑ�
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                adapter.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally 
            {
                dgvTester.ReadOnly = true;
                dgvTester.AllowUserToAddRows = false;
                btnSave.Enabled = false;
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        // ����{�^����V���[�g�J�b�g�ł̏I���������Ȃ�
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //�e�t�H�[��Form1�̃f�[�^�O���b�g�r���[���X�V���邽�߁A�f���Q�[�g�C�x���g�𔭐�������
            this.RefreshEvent(this, new EventArgs());
            this.Close();
        }
    }
}