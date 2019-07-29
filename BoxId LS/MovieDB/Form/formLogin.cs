using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb; 
using System.Security.Permissions;

namespace BoxIdDb
{
    public partial class formLogin : Form
    {
        /// <summary>
        // application name that is given from caller application for displaying itself with version on login screen
        /// </summary>
        private string applicationName;

        // Constructor
        public formLogin(string applicationname)
        {
            applicationName = applicationname;

            InitializeComponent();
        }

        // Load event: add user list from database to the combobox
        private void formLogin_Load(object sender, EventArgs e)
        {
            string sql = "select DISTINCT suser FROM s_user ORDER BY suser";
            ShSQL tf = new ShSQL();
            tf.getComboBoxData(sql, ref cmbUserName);

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append("VERSION:   ");
                version.Append(applicationName + "_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                version.Append(deploy.Build);
                version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }

            txtPassword.Select();
        }

        // Login button click event: match account and pass
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string sql = null;
            string user = null;
            string pass = null;
            bool login = false;

            user = cmbUserName.Text;

            if (user != null) 
            {
                ShSQL tf = new ShSQL();

                sql = "select pass FROM s_user WHERE suser='" + user + "'";
                pass = tf.sqlExecuteScalarString(sql);

                sql = "select loginstatus FROM s_user WHERE suser='" + user + "'";
                login = tf.sqlExecuteScalarBool(sql); 

                if (pass == txtPassword.Text)
                {
                    if (login)
                    {
                        DialogResult reply = MessageBox.Show("This user account is currently used by other user," + System.Environment.NewLine +
                            "or the log out last time had a problem." + System.Environment.NewLine + "Do you log in with this account ?",
                            "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (reply == DialogResult.No)
                        {
                            return;
                        }
                    }

                    // turn the login status into TRUE
                    sql = "UPDATE s_user SET loginstatus=true WHERE suser='" + user + "'";
                    bool res = tf.sqlExecuteNonQuery(sql, false);

                    // Generate child form "frmBoxid" and add delegate event: 
                    // when the child form is closed, this parent login form is also to be closed
                    frmBoxid f1 = new frmBoxid();
                    f1.RefreshEvent += delegate(object sndr, EventArgs excp)
                    {
                        // when frmBoxid(child) is closed, change the login status into FALSE, then close the this form(parent)
                        sql = "UPDATE s_user SET loginstatus=false WHERE suser='" + user + "'";
                        res = tf.sqlExecuteNonQuery(sql, false);
                        //this.Hide();
                    };
                    f1.updateControls(user);
                    this.Hide();
                    f1.ShowDialog();
                    this.Show();
                }
                else if(pass != txtPassword.Text)
                {
                    MessageBox.Show("Password does not match", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // In addition to the Log-in button, entry key on text box also allow user to log in
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

        private void btnOpenSerchMenu_Click(object sender, EventArgs e)
        {
            frmSearch f6 = new frmSearch();
            f6.Show();
        }

    }
}



