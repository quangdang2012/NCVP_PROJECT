namespace IPQC
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.grLogin = new System.Windows.Forms.GroupBox();
            this.Version_lbl = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.btnChangePass = new System.Windows.Forms.Button();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.grChangePass = new System.Windows.Forms.GroupBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblStatusC = new System.Windows.Forms.Label();
            this.cmbUserC = new System.Windows.Forms.ComboBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtOldPassC = new System.Windows.Forms.TextBox();
            this.txtPassC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grLogin.SuspendLayout();
            this.grChangePass.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(87, 74);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(122, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(15, 37);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(66, 13);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "User Name: ";
            // 
            // grLogin
            // 
            this.grLogin.Controls.Add(this.Version_lbl);
            this.grLogin.Controls.Add(this.lblIP);
            this.grLogin.Controls.Add(this.lblStatus);
            this.grLogin.Controls.Add(this.cmbUserName);
            this.grLogin.Controls.Add(this.btnChangePass);
            this.grLogin.Controls.Add(this.btnLogIn);
            this.grLogin.Controls.Add(this.txtPassword);
            this.grLogin.Controls.Add(this.lblPass);
            this.grLogin.Controls.Add(this.lblUser);
            this.grLogin.Controls.Add(this.txtPwd);
            this.grLogin.Location = new System.Drawing.Point(12, 12);
            this.grLogin.Name = "grLogin";
            this.grLogin.Size = new System.Drawing.Size(255, 228);
            this.grLogin.TabIndex = 6;
            this.grLogin.TabStop = false;
            this.grLogin.Text = "Log in and register measurement";
            // 
            // Version_lbl
            // 
            this.Version_lbl.AutoSize = true;
            this.Version_lbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Version_lbl.Location = new System.Drawing.Point(154, 210);
            this.Version_lbl.Name = "Version_lbl";
            this.Version_lbl.Size = new System.Drawing.Size(95, 15);
            this.Version_lbl.TabIndex = 71;
            this.Version_lbl.Tag = "";
            this.Version_lbl.Text = "VERSION : 1_00";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Enabled = false;
            this.lblIP.Location = new System.Drawing.Point(6, 186);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(0, 13);
            this.lblIP.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(71, 99);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 6;
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(87, 34);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(78, 21);
            this.cmbUserName.TabIndex = 1;
            // 
            // btnChangePass
            // 
            this.btnChangePass.Location = new System.Drawing.Point(74, 146);
            this.btnChangePass.Name = "btnChangePass";
            this.btnChangePass.Size = new System.Drawing.Size(104, 25);
            this.btnChangePass.TabIndex = 3;
            this.btnChangePass.Text = "Change Password";
            this.btnChangePass.UseVisualStyleBackColor = true;
            this.btnChangePass.Click += new System.EventHandler(this.btnChangePass_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(74, 115);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(104, 25);
            this.btnLogIn.TabIndex = 3;
            this.btnLogIn.Text = "Log In";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(15, 77);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(59, 13);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Password: ";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(87, 34);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(122, 20);
            this.txtPwd.TabIndex = 5;
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.Click += new System.EventHandler(this.txtPwd_Click);
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwd_KeyDown);
            // 
            // grChangePass
            // 
            this.grChangePass.Controls.Add(this.btnBack);
            this.grChangePass.Controls.Add(this.lblStatusC);
            this.grChangePass.Controls.Add(this.cmbUserC);
            this.grChangePass.Controls.Add(this.btnChange);
            this.grChangePass.Controls.Add(this.txtOldPassC);
            this.grChangePass.Controls.Add(this.txtPassC);
            this.grChangePass.Controls.Add(this.label1);
            this.grChangePass.Controls.Add(this.label3);
            this.grChangePass.Controls.Add(this.label4);
            this.grChangePass.Location = new System.Drawing.Point(12, 14);
            this.grChangePass.Name = "grChangePass";
            this.grChangePass.Size = new System.Drawing.Size(255, 205);
            this.grChangePass.TabIndex = 8;
            this.grChangePass.TabStop = false;
            this.grChangePass.Text = "Change Password";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(157, 152);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 25);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblStatusC
            // 
            this.lblStatusC.AutoSize = true;
            this.lblStatusC.Location = new System.Drawing.Point(99, 129);
            this.lblStatusC.Name = "lblStatusC";
            this.lblStatusC.Size = new System.Drawing.Size(0, 13);
            this.lblStatusC.TabIndex = 6;
            // 
            // cmbUserC
            // 
            this.cmbUserC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserC.FormattingEnabled = true;
            this.cmbUserC.Location = new System.Drawing.Point(100, 36);
            this.cmbUserC.Name = "cmbUserC";
            this.cmbUserC.Size = new System.Drawing.Size(78, 21);
            this.cmbUserC.TabIndex = 1;
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(36, 152);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(104, 25);
            this.btnChange.TabIndex = 3;
            this.btnChange.Text = "Change Password";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // txtOldPassC
            // 
            this.txtOldPassC.Location = new System.Drawing.Point(100, 70);
            this.txtOldPassC.Name = "txtOldPassC";
            this.txtOldPassC.Size = new System.Drawing.Size(122, 20);
            this.txtOldPassC.TabIndex = 2;
            this.txtOldPassC.UseSystemPasswordChar = true;
            // 
            // txtPassC
            // 
            this.txtPassC.Location = new System.Drawing.Point(100, 102);
            this.txtPassC.Name = "txtPassC";
            this.txtPassC.Size = new System.Drawing.Size(122, 20);
            this.txtPassC.TabIndex = 2;
            this.txtPassC.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Old Password: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Password: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "User Name: ";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 250);
            this.Controls.Add(this.grLogin);
            this.Controls.Add(this.grChangePass);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IP-PQM DB";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.grLogin.ResumeLayout(false);
            this.grLogin.PerformLayout();
            this.grChangePass.ResumeLayout(false);
            this.grChangePass.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox grLogin;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.GroupBox grChangePass;
        private System.Windows.Forms.Label lblStatusC;
        private System.Windows.Forms.ComboBox cmbUserC;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.TextBox txtPassC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChangePass;
        private System.Windows.Forms.TextBox txtOldPassC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label Version_lbl;
    }
}

