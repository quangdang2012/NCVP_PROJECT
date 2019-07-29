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
        //�e�t�H�[��frmBoxid�փC�x���g������A���i�f���Q�[�g�j
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;
        DataTable dataTable;
        
        // �R���X�g���N�^
        public frmModuleReplace()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void frmCapacity_Load(object sender, EventArgs e)
        {
            //�t�H�[���̏ꏊ���w��
            this.Left = 450;
            this.Top = 100;
            dataTable = new DataTable();
            defineDatatable(ref dataTable);
        }

        // �T�u�v���V�[�W��: �c�s�̒�`
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

        // �T�u�v���V�[�W���F�e�t�H�[���ŌĂяo���A�e�t�H�[���̏����A�e�L�X�g�{�b�N�X�֊i�[���Ĉ����p��
        public void updateControls(string serno, int row)
        {
            txtBefore.Text = serno;
            txtRow.Text = row.ToString();
        }

        // �o�^�ς̃V���A������т��̕t�я����A�t�o�c�`�s�d���Œu��������
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
                //�e�t�H�[��frmBoxid�̃f�[�^�O���b�g�r���[���X�V���邽�߁A�f���Q�[�g�C�x���g�𔭐�������
                this.RefreshEvent(this, new EventArgs());
                Close();
            }
        }

        // �V���A���t�я����擾����
        private void setSerialInfoAndTesterResult(string serLong)
        {
            if (serLong != String.Empty)
            {
                // �e�X�^�[�f�[�^�̓����e�[�u������A�e�X�g���ʓ����擾����
                string sql = "SELECT serno, lot, tjudge, inspectdate" +
                    " FROM " + "tester_data" +
                    " WHERE serno = '" + serLong + "'" +
                    " ORDER BY tjudge DESC,inspectdate ASC";
                DataTable dt1 = new DataTable();
                ShSQL tf = new ShSQL();
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                // �e�X�^�[�f�[�^�̑O���e�[�u������A�e�X�g���ʓ����擾���A�����c�`�s�`�s�`�a�k�d�փ��R�[�h��ǉ�����
                sql = "SELECT serno, lot, tjudge, inspectdate" +
                    " FROM " + "tester_data" +
                    " WHERE serno = '" + serLong + "'" +
                    " ORDER BY tjudge DESC,inspectdate ASC";
                tf.sqlDataAdapterFillDatatableFromTesterDb(sql, ref dt1);

                DataView dv = new DataView(dt1);
                dv.Sort = "tjudge desc, inspectdate";
                DataTable dt2 = dv.ToTable();

                // �ꎞ�e�[�u���ւ̓o�^����
                string lot = VBStrings.Mid(serLong, 4, 5);
                string line = VBStrings.Mid(serLong, 8, 1);
                string config1 = VBStrings.Mid(serLong, 12, 4);
                string config2 = VBStrings.Mid(serLong, 16, 1);

                // �e�X�^�[�f�[�^�ɊY�����Ȃ��ꍇ�ł��A���[�U�[�ɔF�������邽�߂ɕ\������
                // �܂��A�{�t�H�[���̂c�`�s�`�s�`�a�k�d�́A��ɂP���݂̂̃f�[�^�ێ��ł悢
                dataTable.Clear();
                DataRow newrow = dataTable.NewRow();
                newrow["serialno"] = serLong;
                newrow["lot"] = lot;
                newrow["line"] = line;
                newrow["config1"] = config1;
                newrow["config2"] = config2;

                // �e�X�^�[�f�[�^�ɊY��������ꍇ�A���R�\������
                if (dt2.Rows.Count != 0)
                {
                    string stationid = dt2.Rows[0][1].ToString();
                    string linepass = dt2.Rows[0][2].ToString();
                    DateTime testtime = (DateTime)dt2.Rows[0][3];

                    newrow["stationid"] = stationid;
                    newrow["linepass"] = linepass;
                    newrow["testtime"] = testtime;
                }

                // ��������̃e�[�u���Ƀ��R�[�h��ǉ�
                dataTable.Rows.Add(newrow);
            }
        }

        // ���̃t�H�[���Ƃ̐���������邽�߁A�L�����Z���{�^����݂���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}