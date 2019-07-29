namespace Tracy
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnEditBatch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvBatchNo = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.dtpBatchDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLeaderId = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnAddBatch = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLeaderName = new System.Windows.Forms.TextBox();
            this.cmbModelNo = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.cmbSubAssyNo = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSubAssyName = new System.Windows.Forms.TextBox();
            this.cmbLine = new System.Windows.Forms.ComboBox();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkBatch = new System.Windows.Forms.CheckBox();
            this.chkModel = new System.Windows.Forms.CheckBox();
            this.chkSubAssy = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkLine = new System.Windows.Forms.CheckBox();
            this.chkLeader = new System.Windows.Forms.CheckBox();
            this.chkBatchDate = new System.Windows.Forms.CheckBox();
            this.ckbAll = new System.Windows.Forms.CheckBox();
            this.btnApprove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchNo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEditBatch
            // 
            this.btnEditBatch.Location = new System.Drawing.Point(836, 37);
            this.btnEditBatch.Name = "btnEditBatch";
            this.btnEditBatch.Size = new System.Drawing.Size(89, 25);
            this.btnEditBatch.TabIndex = 6;
            this.btnEditBatch.Text = "Edit Batch";
            this.btnEditBatch.UseVisualStyleBackColor = true;
            this.btnEditBatch.Click += new System.EventHandler(this.btnEditBatch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(727, 37);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(89, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearchBoxId_Click);
            // 
            // dgvBatchNo
            // 
            this.dgvBatchNo.AllowUserToAddRows = false;
            this.dgvBatchNo.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dgvBatchNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchNo.Location = new System.Drawing.Point(12, 170);
            this.dgvBatchNo.Name = "dgvBatchNo";
            this.dgvBatchNo.ReadOnly = true;
            this.dgvBatchNo.RowTemplate.Height = 21;
            this.dgvBatchNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBatchNo.Size = new System.Drawing.Size(1063, 426);
            this.dgvBatchNo.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(375, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Batch Date: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Batch No: ";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(113, 15);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(188, 20);
            this.txtBatchNo.TabIndex = 1;
            // 
            // dtpBatchDate
            // 
            this.dtpBatchDate.CustomFormat = "yyyy/MM/dd";
            this.dtpBatchDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBatchDate.Location = new System.Drawing.Point(462, 15);
            this.dtpBatchDate.Name = "dtpBatchDate";
            this.dtpBatchDate.Size = new System.Drawing.Size(187, 20);
            this.dtpBatchDate.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(376, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Leader Id: ";
            // 
            // txtLeaderId
            // 
            this.txtLeaderId.Enabled = false;
            this.txtLeaderId.Location = new System.Drawing.Point(462, 101);
            this.txtLeaderId.Name = "txtLeaderId";
            this.txtLeaderId.Size = new System.Drawing.Size(188, 20);
            this.txtLeaderId.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(946, 37);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(946, 99);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(89, 25);
            this.btnExport.TabIndex = 42;
            this.btnExport.Text = "Excel Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnAddBatch
            // 
            this.btnAddBatch.Location = new System.Drawing.Point(836, 99);
            this.btnAddBatch.Name = "btnAddBatch";
            this.btnAddBatch.Size = new System.Drawing.Size(89, 25);
            this.btnAddBatch.TabIndex = 6;
            this.btnAddBatch.Text = "Add Batch";
            this.btnAddBatch.UseVisualStyleBackColor = true;
            this.btnAddBatch.Click += new System.EventHandler(this.btnAddBatch_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(375, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Leader Name: ";
            // 
            // txtLeaderName
            // 
            this.txtLeaderName.Enabled = false;
            this.txtLeaderName.Location = new System.Drawing.Point(463, 128);
            this.txtLeaderName.Name = "txtLeaderName";
            this.txtLeaderName.Size = new System.Drawing.Size(187, 20);
            this.txtLeaderName.TabIndex = 1;
            // 
            // cmbModelNo
            // 
            this.cmbModelNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModelNo.FormattingEnabled = true;
            this.cmbModelNo.Location = new System.Drawing.Point(113, 46);
            this.cmbModelNo.Name = "cmbModelNo";
            this.cmbModelNo.Size = new System.Drawing.Size(104, 21);
            this.cmbModelNo.TabIndex = 52;
            this.cmbModelNo.SelectedIndexChanged += new System.EventHandler(this.cmbModelNo_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 50;
            this.label9.Text = "Model Name: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 51;
            this.label7.Text = "Model No: ";
            // 
            // txtModelName
            // 
            this.txtModelName.Enabled = false;
            this.txtModelName.Location = new System.Drawing.Point(113, 73);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(188, 20);
            this.txtModelName.TabIndex = 49;
            // 
            // cmbSubAssyNo
            // 
            this.cmbSubAssyNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubAssyNo.Enabled = false;
            this.cmbSubAssyNo.FormattingEnabled = true;
            this.cmbSubAssyNo.Location = new System.Drawing.Point(113, 100);
            this.cmbSubAssyNo.Name = "cmbSubAssyNo";
            this.cmbSubAssyNo.Size = new System.Drawing.Size(96, 21);
            this.cmbSubAssyNo.TabIndex = 56;
            this.cmbSubAssyNo.SelectedIndexChanged += new System.EventHandler(this.cmbSubAssyNo_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(18, 131);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 13);
            this.label17.TabIndex = 54;
            this.label17.Text = "Sub Assy Name: ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 103);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 13);
            this.label14.TabIndex = 55;
            this.label14.Text = "Sub Assy No: ";
            // 
            // txtSubAssyName
            // 
            this.txtSubAssyName.Enabled = false;
            this.txtSubAssyName.Location = new System.Drawing.Point(113, 128);
            this.txtSubAssyName.Name = "txtSubAssyName";
            this.txtSubAssyName.Size = new System.Drawing.Size(188, 20);
            this.txtSubAssyName.TabIndex = 53;
            // 
            // cmbLine
            // 
            this.cmbLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLine.Enabled = false;
            this.cmbLine.FormattingEnabled = true;
            this.cmbLine.Location = new System.Drawing.Point(461, 73);
            this.cmbLine.Name = "cmbLine";
            this.cmbLine.Size = new System.Drawing.Size(80, 21);
            this.cmbLine.TabIndex = 59;
            // 
            // cmbShift
            // 
            this.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShift.FormattingEnabled = true;
            this.cmbShift.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbShift.Location = new System.Drawing.Point(461, 46);
            this.cmbShift.Name = "cmbShift";
            this.cmbShift.Size = new System.Drawing.Size(80, 21);
            this.cmbShift.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(376, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 57;
            this.label8.Text = "Line: ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(376, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "Shift: ";
            // 
            // chkBatch
            // 
            this.chkBatch.AutoSize = true;
            this.chkBatch.Location = new System.Drawing.Point(329, 18);
            this.chkBatch.Name = "chkBatch";
            this.chkBatch.Size = new System.Drawing.Size(15, 14);
            this.chkBatch.TabIndex = 61;
            this.chkBatch.UseVisualStyleBackColor = true;
            // 
            // chkModel
            // 
            this.chkModel.AutoSize = true;
            this.chkModel.Location = new System.Drawing.Point(329, 49);
            this.chkModel.Name = "chkModel";
            this.chkModel.Size = new System.Drawing.Size(15, 14);
            this.chkModel.TabIndex = 61;
            this.chkModel.UseVisualStyleBackColor = true;
            // 
            // chkSubAssy
            // 
            this.chkSubAssy.AutoSize = true;
            this.chkSubAssy.Location = new System.Drawing.Point(329, 103);
            this.chkSubAssy.Name = "chkSubAssy";
            this.chkSubAssy.Size = new System.Drawing.Size(15, 14);
            this.chkSubAssy.TabIndex = 61;
            this.chkSubAssy.UseVisualStyleBackColor = true;
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Location = new System.Drawing.Point(670, 49);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(15, 14);
            this.chkShift.TabIndex = 61;
            this.chkShift.UseVisualStyleBackColor = true;
            // 
            // chkLine
            // 
            this.chkLine.AutoSize = true;
            this.chkLine.Location = new System.Drawing.Point(670, 76);
            this.chkLine.Name = "chkLine";
            this.chkLine.Size = new System.Drawing.Size(15, 14);
            this.chkLine.TabIndex = 61;
            this.chkLine.UseVisualStyleBackColor = true;
            // 
            // chkLeader
            // 
            this.chkLeader.AutoSize = true;
            this.chkLeader.Location = new System.Drawing.Point(670, 105);
            this.chkLeader.Name = "chkLeader";
            this.chkLeader.Size = new System.Drawing.Size(15, 14);
            this.chkLeader.TabIndex = 61;
            this.chkLeader.UseVisualStyleBackColor = true;
            // 
            // chkBatchDate
            // 
            this.chkBatchDate.AutoSize = true;
            this.chkBatchDate.Checked = true;
            this.chkBatchDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBatchDate.Location = new System.Drawing.Point(670, 18);
            this.chkBatchDate.Name = "chkBatchDate";
            this.chkBatchDate.Size = new System.Drawing.Size(15, 14);
            this.chkBatchDate.TabIndex = 61;
            this.chkBatchDate.UseVisualStyleBackColor = true;
            // 
            // ckbAll
            // 
            this.ckbAll.AutoSize = true;
            this.ckbAll.Location = new System.Drawing.Point(83, 176);
            this.ckbAll.Name = "ckbAll";
            this.ckbAll.Size = new System.Drawing.Size(15, 14);
            this.ckbAll.TabIndex = 62;
            this.ckbAll.UseVisualStyleBackColor = true;
            this.ckbAll.Visible = false;
            this.ckbAll.CheckedChanged += new System.EventHandler(this.ckbAll_CheckedChanged);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(727, 99);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(89, 25);
            this.btnApprove.TabIndex = 2;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Visible = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1085, 609);
            this.Controls.Add(this.ckbAll);
            this.Controls.Add(this.chkLeader);
            this.Controls.Add(this.chkLine);
            this.Controls.Add(this.chkBatchDate);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkSubAssy);
            this.Controls.Add(this.chkModel);
            this.Controls.Add(this.chkBatch);
            this.Controls.Add(this.cmbLine);
            this.Controls.Add(this.cmbShift);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbSubAssyNo);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtSubAssyName);
            this.Controls.Add(this.cmbModelNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dtpBatchDate);
            this.Controls.Add(this.txtBatchNo);
            this.Controls.Add(this.txtLeaderName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLeaderId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddBatch);
            this.Controls.Add(this.btnEditBatch);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvBatchNo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch No";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgvBatchNo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnEditBatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.DateTimePicker dtpBatchDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLeaderId;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnAddBatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLeaderName;
        private System.Windows.Forms.ComboBox cmbModelNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.ComboBox cmbSubAssyNo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSubAssyName;
        private System.Windows.Forms.ComboBox cmbLine;
        private System.Windows.Forms.ComboBox cmbShift;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkBatch;
        private System.Windows.Forms.CheckBox chkModel;
        private System.Windows.Forms.CheckBox chkSubAssy;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.CheckBox chkLine;
        private System.Windows.Forms.CheckBox chkLeader;
        private System.Windows.Forms.CheckBox chkBatchDate;
        private System.Windows.Forms.CheckBox ckbAll;
        private System.Windows.Forms.Button btnApprove;
    }
}

