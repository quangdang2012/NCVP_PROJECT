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
        // コンストラクタ
        public Login(string applicationname)
        {
            InitializeComponent();

            applicationName = applicationname;
        }

        // ロード時の処理（コンボボックスに、オートコンプリート機能の追加）
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

        // コンボボックス選択時、リーダー名を表示する
        private void cmbLeaderId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "select leader_name FROM t_leader_id where leader_id ='" + cmbLeaderId.Text + "'";
            System.Diagnostics.Debug.Print(sql);
            TfSQL tf = new TfSQL();
            string name = tf.sqlExecuteScalarString(sql);
            txtLeaderName.Text = name;
        }

        // ユーザーログイン
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
                    // 子フォームForm1を表示し、デレゲートイベントを追加： 

                    Main f1 = new Main();
                    f1.RefreshEvent += delegate (object sndr, EventArgs excp)
                    {
                        // 子フォームForm1を閉じる際、当フォームを表示する
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

        // ユーザーログイン
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