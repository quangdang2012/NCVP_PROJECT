using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Permissions;

namespace Tracy
{
    public partial class Login : Form
    {
        // <summary>
        // application name that is given from caller application for displaying itself with version on login screen
        /// </summary>
        private string applicationName;
        // �R���X�g���N�^
        public Login(string applicationname)
        {
            InitializeComponent();

            applicationName = applicationname;
        }

        // ���[�h���̏����i�R���{�{�b�N�X�ɁA�I�[�g�R���v���[�g�@�\�̒ǉ��j
        private void Login_Load(object sender, EventArgs e)
        {
            string sql = "select leader_id FROM t_leader_id ORDER BY leader_id";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmbLeaderId);

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append("VERSION: ");
                version.Append(applicationName + "_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                //version.Append(deploy.Build);
                // version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }
        }

        // �R���{�{�b�N�X�I�����A���[�_�[����\������
        private void cmbLeaderId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select leader_name FROM t_leader_id where leader_id ='" + cmbLeaderId.Text + "'";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            string name = tf.sqlExecuteScalarString(sql);
            txtLeaderName.Text = name;
        }

        // ���[�U�[���O�C��
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string leaderId = cmbLeaderId.Text;
            string leaderName = txtLeaderName.Text;

            leaderId = cmbLeaderId.Text;

            if (leaderId != null)
            {
                TfSQL tf = new TfSQL();

                string sql1 = "select pass FROM t_leader_id WHERE leader_id='" + leaderId + "'";
                string pass = tf.sqlExecuteScalarString(sql1);

                if (pass == txtPassword.Text)
                {
                    // �q�t�H�[��Form1��\�����A�f���Q�[�g�C�x���g��ǉ��F 

                    Main f1 = new Main();
                    f1.RefreshEvent += delegate (object sndr, EventArgs excp)
                    {
                        // �q�t�H�[��Form1�����ہA���t�H�[����\������
                        txtPassword.Text = string.Empty;
                        this.Visible = true;
                    };

                    string sql2 = "select adminflag FROM t_leader_id WHERE leader_id = '" + leaderId + "'";
                    bool adminUser = tf.sqlExecuteScalarBool(sql2);

                    f1.updateControls(leaderId, leaderName, adminUser);
                    f1.Show();
                    this.Visible = false;
                }
                else if (pass != txtPassword.Text)
                {
                    MessageBox.Show("Password does not match", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.ResetText();
                }
            }
        }

        // ���[�U�[���O�C��
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string pass = txtPassword.Text;
                if (pass != String.Empty)
                {
                    btnLogIn_Click(sender, e);
                }
            }
        }
    }
}