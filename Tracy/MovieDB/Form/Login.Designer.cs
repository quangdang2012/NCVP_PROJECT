namespace Tracy
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Version_lbl = new System.Windows.Forms.Label();
            this.cmbLeaderId = new System.Windows.Forms.ComboBox();
            this.btnOqcLogIn = new System.Windows.Forms.Button();
            this.txtLeaderName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(110, 128);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(133, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Leader Id: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Version_lbl);
            this.groupBox1.Controls.Add(this.cmbLeaderId);
            this.groupBox1.Controls.Add(this.btnOqcLogIn);
            this.groupBox1.Controls.Add(this.txtLeaderName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 239);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log in and start assembly batch";
            // 
            // Version_lbl
            // 
            this.Version_lbl.AutoSize = true;
            this.Version_lbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.Version_lbl.Location = new System.Drawing.Point(126, 219);
            this.Version_lbl.Name = "Version_lbl";
            this.Version_lbl.Size = new System.Drawing.Size(95, 15);
            this.Version_lbl.TabIndex = 70;
            this.Version_lbl.Tag = "";
            this.Version_lbl.Text = "VERSION : 1_00";
            // 
            // cmbLeaderId
            // 
            this.cmbLeaderId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeaderId.FormattingEnabled = true;
            this.cmbLeaderId.Location = new System.Drawing.Point(110, 45);
            this.cmbLeaderId.Name = "cmbLeaderId";
            this.cmbLeaderId.Size = new System.Drawing.Size(72, 21);
            this.cmbLeaderId.TabIndex = 1;
            this.cmbLeaderId.SelectedIndexChanged += new System.EventHandler(this.cmbLeaderId_SelectedIndexChanged);
            // 
            // btnOqcLogIn
            // 
            this.btnOqcLogIn.Location = new System.Drawing.Point(56, 182);
            this.btnOqcLogIn.Name = "btnOqcLogIn";
            this.btnOqcLogIn.Size = new System.Drawing.Size(149, 25);
            this.btnOqcLogIn.TabIndex = 3;
            this.btnOqcLogIn.Text = "Log In";
            this.btnOqcLogIn.UseVisualStyleBackColor = true;
            this.btnOqcLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // txtLeaderName
            // 
            this.txtLeaderName.Enabled = false;
            this.txtLeaderName.Location = new System.Drawing.Point(110, 73);
            this.txtLeaderName.Name = "txtLeaderName";
            this.txtLeaderName.Size = new System.Drawing.Size(133, 20);
            this.txtLeaderName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Leader Name: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Password: ";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 253);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tracy (trace sheet system)";
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOqcLogIn;
        private System.Windows.Forms.ComboBox cmbLeaderId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLeaderName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Version_lbl;
    }
}

