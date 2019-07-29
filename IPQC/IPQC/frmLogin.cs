using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.Text;

namespace IPQC
{
    public partial class frmLogin : Form
    {
        string str_md5;
        // コンストラクタ
        /// <summary>
        // application name that is given from caller application for displaying itself with version on login screen
        /// </summary>
        private string applicationName;
        public frmLogin()//string applicationname)
        {
            applicationName = "IPQC Motor";

            InitializeComponent();
        }

        // ロード時の処理（コンボボックスに、オートコンプリート機能の追加）
        private void Form5_Load(object sender, EventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append("VERSION: ");
                //version.Append(applicationName + "_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                version.Append(deploy.Build);
                version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }

            txtPwd.Visible = false;
            btnLogIn.Enabled = false;
            grChangePass.Visible = false;
            string sql = "select DISTINCT qcuser FROM qc_user ORDER BY qcuser";
            TfSQL tf = new TfSQL();
            tf.getComboBoxData(sql, ref cmbUserName);
            tf.getComboBoxData(sql, ref cmbUserC);

            string HostName = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostByName(HostName);
            foreach (IPAddress ipadd in ip.AddressList)
            {
                lblIP.Text = lblIP.Text + ipadd.ToString();
            }
            
        }

        public string GetMD5(string chuoi)
        {
            str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
            
        }
        // ユーザーログイン時、パスワードとログイン状態の確認（２重ログインの防止）
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (btnLogIn.Text == "Log In")
            {
                TfSQL tf = new TfSQL();
                string logt = tf.sqlExecuteScalarString("select login_times from qc_user where qcuser = '" + cmbUserName.Text + "'");
                if (logt == "0")
                {
                    grLogin.Text = "Change Default Password";
                    lblUser.Text = "New Password";
                    txtPwd.Size = new Size(100, 20);
                    txtPwd.Location = new Point(123, 47);
                    lblPass.Text = "Confirm Password";
                    txtPassword.Size = new Size(100, 20);
                    txtPassword.Location = new Point(123, 87);
                    btnLogIn.Text = "Change Password";
                    btnLogIn.Enabled = false;
                    txtPassword.ResetText();
                    txtPwd.ResetText();
                    txtPwd.Visible = true;
                    txtPwd.Focus();
                    cmbUserName.Visible = false;
                    btnChangePass.Visible = false;
                }
                else
                {
                    string sql = null;
                    string user = null;
                    string pass = null;
                    string ip = null;
                    bool login = false;

                    user = cmbUserName.Text;

                    if (user != null)
                    {
                        sql = "select pass FROM qc_user WHERE qcuser='" + user + "'";
                        pass = tf.sqlExecuteScalarString(sql);

                        sql = "select loginstatus FROM qc_user WHERE qcuser='" + user + "'";
                        login = tf.sqlExecuteScalarBool(sql);

                        sql = "select ip_address from qc_user where qcuser = '" + user + "'";
                        ip = tf.sqlExecuteScalarString(sql);

                        
                        GetMD5(txtPassword.Text);

                        if (pass == str_md5)
                        {
                            if (login && ip != "null" && ip != lblIP.Text)
                            {
                                DialogResult reply = MessageBox.Show("This user account is currently used by " + ip + "," + System.Environment.NewLine +
                                    "or the log out last time had a problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                sql = "UPDATE qc_user SET loginstatus=false, ip_address = '" + lblIP.Text + "' WHERE qcuser='" + user + "'";
                                bool res1 = tf.sqlExecuteNonQuery(sql, false);
                                return;
                            }

                            //Check IP Address
                            if (ip == "null") tf.sqlExecuteScalarString("UPDATE qc_user SET ip_address = '" + lblIP.Text + "' where qcuser = '" + user + "'");

                            // ログイン状態をＴＲＵＥへ変更
                            sql = "UPDATE qc_user SET loginstatus=true WHERE qcuser='" + user + "'";
                            bool res = tf.sqlExecuteNonQuery(sql, false);

                            // 子フォームForm1を表示し、デレゲートイベントを追加： 
                            frmItem f1 = new frmItem();
                            f1.RefreshEvent += delegate (object sndr, EventArgs excp)
                            {
                            // Form1を閉じる際、ログイン状態をＦＡＬＳＥへ変更し、当フォームForm5も閉じる
                            sql = "UPDATE qc_user SET loginstatus=false, ip_address = 'null' WHERE qcuser='" + user + "'";
                                res = tf.sqlExecuteNonQuery(sql, false);
                                this.Visible = true;
                                txtPassword.ResetText();
                            };
                            f1.updateControls(user, lblIP.Text);
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
            }
            else
            {
                if (txtPwd.Text != txtPassword.Text)
                {
                    lblStatus.Text = "Password is not avaliable!";
                    lblStatus.ForeColor = Color.Red;
                }
                else
                {
                    string pass = txtPwd.Text;
                    GetMD5(pass);
                    TfSQL up = new TfSQL();
                    up.sqlExecuteScalarString("update qc_user set pass = '" + str_md5 + "' where qcuser = '" + cmbUserName.Text + "'");
                    up.sqlExecuteScalarString("update qc_user set login_times = '1' where qcuser = '" + cmbUserName.Text + "'");
                    btnLogIn.Text = "Log In";
                    lblUser.Text = "Username:";
                    lblPass.Text = "Password:";
                    txtPassword.Size = new Size(122, 20);
                    txtPassword.Location = new Point(87, 87);
                    cmbUserName.Visible = true;
                    txtPwd.Visible = false;
                    DialogResult result = MessageBox.Show("Your password has been changed!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (result == DialogResult.OK) login();
                }
            }
        }
        private void login()
        {
            string sql = null;
            string user = null;
            string pass = null;
            string ip = null;
            bool login = false;

            user = cmbUserName.Text;

            if (user != null)
            {
                TfSQL tf = new TfSQL();
                sql = "select pass FROM qc_user WHERE qcuser='" + user + "'";
                pass = tf.sqlExecuteScalarString(sql);

                sql = "select loginstatus FROM qc_user WHERE qcuser='" + user + "'";
                login = tf.sqlExecuteScalarBool(sql);

                sql = "select ip_address from qc_user where qcuser = '" + user + "'";
                ip = tf.sqlExecuteScalarString(sql);

                //Check IP Address
                if (ip == "null") tf.sqlExecuteScalarString("UPDATE qc_user SET ip_address = '" + lblIP.Text + "'");

                GetMD5(txtPassword.Text);

                if (pass == str_md5)
                {
                    if (login && ip != "null" && ip != lblIP.Text)
                    {
                        DialogResult reply = MessageBox.Show("This user account is currently used by " + ip + "," + System.Environment.NewLine +
                            "or the log out last time had a problem.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        return;
                    }

                    // ログイン状態をＴＲＵＥへ変更
                    sql = "UPDATE qc_user SET loginstatus=true WHERE qcuser='" + user + "'";
                    bool res = tf.sqlExecuteNonQuery(sql, false);

                    // 子フォームForm1を表示し、デレゲートイベントを追加： 
                    frmItem f1 = new frmItem();
                    f1.RefreshEvent += delegate (object sndr, EventArgs excp)
                    {
                        // Form1を閉じる際、ログイン状態をＦＡＬＳＥへ変更し、当フォームForm5も閉じる
                        sql = "UPDATE qc_user SET loginstatus=false, ip_address = 'null' WHERE qcuser='" + user + "'";
                        res = tf.sqlExecuteNonQuery(sql, false);
                        this.Visible = true;
                        txtPassword.ResetText();
                    };
                    f1.updateControls(user, lblIP.Text);
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
        
        // ログインボタンの押下に加え、エンターキーでもログインできる
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string serialNo = txtPassword.Text;
                if (serialNo != String.Empty)
                {
                    btnLogIn_Click(sender, e);
                }
            }
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            lblStatus.ResetText();
        }

        private void txtPwd_Click(object sender, EventArgs e)
        {
            lblStatus.ResetText();
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            btnLogIn.Enabled = true;
            if (txtPwd.Text == "" && txtPassword.Text == "") btnLogIn.Enabled = false;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            btnLogIn.Enabled = true;
            if (txtPwd.Text == "" && txtPassword.Text == "") btnLogIn.Enabled = false;
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string serialNo = txtPassword.Text;
                if (serialNo != String.Empty)
                {
                    btnLogIn_Click(sender, e);
                }
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            grChangePass.Visible = true;
            grLogin.Visible = false;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            GetMD5(txtOldPassC.Text);
            TfSQL tf = new TfSQL();
            string passC = tf.sqlExecuteScalarString("select pass from qc_user where qcuser = '" + cmbUserC.Text + "'");
            if (passC == str_md5)
            {
                lblStatusC.Text = "Available!";
                lblStatusC.ForeColor = Color.Green;
            }
            else
            {
                lblStatusC.Text = "The old password is wrong!";
                lblStatusC.ForeColor = Color.Red;
                return;
            }
            GetMD5(txtPassC.Text);
            tf.sqlExecuteScalarString("update qc_user set pass = '" + str_md5 + "' where qcuser = '" + cmbUserC.Text + "'");
            DialogResult result = MessageBox.Show("Your password has been changed!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                grChangePass.Visible = false;
                grLogin.Visible = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            grChangePass.Visible = false;
        }
    }
}