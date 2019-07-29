namespace BoxIdDb
{
    partial class frmBoxid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBoxid));
            this.btnAddBoxId = new System.Windows.Forms.Button();
            this.btnSearchBoxId = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.dtpPrintDate = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdbPrintDate = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbShipDate = new System.Windows.Forms.RadioButton();
            this.dtpShipDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtShipStatus = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtInvoice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtShipto = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnShipHistory = new System.Windows.Forms.Button();
            this.btnShip = new System.Windows.Forms.Button();
            this.dgvBoxId = new System.Windows.Forms.DataGridView();
            this.shipchk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.boxid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.child_model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRTV = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoxId)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddBoxId
            // 
            this.btnAddBoxId.Location = new System.Drawing.Point(156, 116);
            this.btnAddBoxId.Name = "btnAddBoxId";
            this.btnAddBoxId.Size = new System.Drawing.Size(119, 25);
            this.btnAddBoxId.TabIndex = 6;
            this.btnAddBoxId.Text = "Add Box ID";
            this.btnAddBoxId.UseVisualStyleBackColor = true;
            this.btnAddBoxId.Click += new System.EventHandler(this.btnAddBoxId_Click);
            // 
            // btnSearchBoxId
            // 
            this.btnSearchBoxId.Location = new System.Drawing.Point(67, 116);
            this.btnSearchBoxId.Name = "btnSearchBoxId";
            this.btnSearchBoxId.Size = new System.Drawing.Size(83, 25);
            this.btnSearchBoxId.TabIndex = 2;
            this.btnSearchBoxId.Text = "Search";
            this.btnSearchBoxId.UseVisualStyleBackColor = true;
            this.btnSearchBoxId.Click += new System.EventHandler(this.btnSearchBoxId_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(51, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Print Date: ";
            // 
            // dtpPrintDate
            // 
            this.dtpPrintDate.CustomFormat = "yyyy/MM/dd";
            this.dtpPrintDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPrintDate.Location = new System.Drawing.Point(133, 14);
            this.dtpPrintDate.Name = "dtpPrintDate";
            this.dtpPrintDate.Size = new System.Drawing.Size(166, 20);
            this.dtpPrintDate.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(638, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdbPrintDate
            // 
            this.rdbPrintDate.AutoSize = true;
            this.rdbPrintDate.Checked = true;
            this.rdbPrintDate.Location = new System.Drawing.Point(307, 18);
            this.rdbPrintDate.Name = "rdbPrintDate";
            this.rdbPrintDate.Size = new System.Drawing.Size(14, 13);
            this.rdbPrintDate.TabIndex = 12;
            this.rdbPrintDate.TabStop = true;
            this.rdbPrintDate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ship Date: ";
            // 
            // rdbShipDate
            // 
            this.rdbShipDate.AutoSize = true;
            this.rdbShipDate.Location = new System.Drawing.Point(307, 46);
            this.rdbShipDate.Name = "rdbShipDate";
            this.rdbShipDate.Size = new System.Drawing.Size(14, 13);
            this.rdbShipDate.TabIndex = 12;
            this.rdbShipDate.UseVisualStyleBackColor = true;
            // 
            // dtpShipDate
            // 
            this.dtpShipDate.CustomFormat = "yyyy/MM/dd";
            this.dtpShipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpShipDate.Location = new System.Drawing.Point(133, 46);
            this.dtpShipDate.Name = "dtpShipDate";
            this.dtpShipDate.ShowUpDown = true;
            this.dtpShipDate.Size = new System.Drawing.Size(166, 20);
            this.dtpShipDate.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Ship Status: ";
            // 
            // txtShipStatus
            // 
            this.txtShipStatus.Location = new System.Drawing.Point(83, 5);
            this.txtShipStatus.Name = "txtShipStatus";
            this.txtShipStatus.Size = new System.Drawing.Size(166, 20);
            this.txtShipStatus.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.txtInvoice);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtShipto);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtShipStatus);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(444, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 84);
            this.panel1.TabIndex = 13;
            // 
            // txtInvoice
            // 
            this.txtInvoice.Location = new System.Drawing.Point(83, 57);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(166, 20);
            this.txtInvoice.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Invoice: ";
            // 
            // txtShipto
            // 
            this.txtShipto.Location = new System.Drawing.Point(83, 31);
            this.txtShipto.Name = "txtShipto";
            this.txtShipto.Size = new System.Drawing.Size(166, 20);
            this.txtShipto.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ship To: ";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRTV);
            this.splitContainer1.Panel1.Controls.Add(this.btnShipHistory);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearchBoxId);
            this.splitContainer1.Panel1.Controls.Add(this.rdbShipDate);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddBoxId);
            this.splitContainer1.Panel1.Controls.Add(this.btnShip);
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel1.Controls.Add(this.rdbPrintDate);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.dtpShipDate);
            this.splitContainer1.Panel1.Controls.Add(this.dtpPrintDate);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvBoxId);
            this.splitContainer1.Size = new System.Drawing.Size(787, 668);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 14;
            // 
            // btnShipHistory
            // 
            this.btnShipHistory.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShipHistory.ForeColor = System.Drawing.Color.Blue;
            this.btnShipHistory.Location = new System.Drawing.Point(370, 116);
            this.btnShipHistory.Name = "btnShipHistory";
            this.btnShipHistory.Size = new System.Drawing.Size(128, 25);
            this.btnShipHistory.TabIndex = 14;
            this.btnShipHistory.Text = "Shipment History";
            this.btnShipHistory.UseVisualStyleBackColor = true;
            this.btnShipHistory.Click += new System.EventHandler(this.btnShipHistory_Click);
            // 
            // btnShip
            // 
            this.btnShip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnShip.Location = new System.Drawing.Point(281, 116);
            this.btnShip.Name = "btnShip";
            this.btnShip.Size = new System.Drawing.Size(83, 25);
            this.btnShip.TabIndex = 6;
            this.btnShip.Text = "Ship";
            this.btnShip.UseVisualStyleBackColor = true;
            this.btnShip.Click += new System.EventHandler(this.btnShip_Click);
            // 
            // dgvBoxId
            // 
            this.dgvBoxId.AllowUserToAddRows = false;
            this.dgvBoxId.AllowUserToDeleteRows = false;
            this.dgvBoxId.AllowUserToResizeColumns = false;
            this.dgvBoxId.AllowUserToResizeRows = false;
            this.dgvBoxId.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBoxId.BackgroundColor = System.Drawing.Color.Azure;
            this.dgvBoxId.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBoxId.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shipchk,
            this.boxid,
            this.child_model,
            this.user,
            this.printdate,
            this.shipdate});
            this.dgvBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBoxId.Location = new System.Drawing.Point(0, 0);
            this.dgvBoxId.Name = "dgvBoxId";
            this.dgvBoxId.RowHeadersVisible = false;
            this.dgvBoxId.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvBoxId.RowTemplate.Height = 21;
            this.dgvBoxId.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBoxId.Size = new System.Drawing.Size(787, 504);
            this.dgvBoxId.TabIndex = 11;
            this.dgvBoxId.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBoxId_CellContentClick);
            // 
            // shipchk
            // 
            this.shipchk.HeaderText = "";
            this.shipchk.IndeterminateValue = "";
            this.shipchk.Name = "shipchk";
            this.shipchk.Width = 5;
            // 
            // boxid
            // 
            this.boxid.DataPropertyName = "BoxID";
            this.boxid.HeaderText = "BoxID";
            this.boxid.Name = "boxid";
            this.boxid.Width = 61;
            // 
            // child_model
            // 
            this.child_model.DataPropertyName = "child_model";
            this.child_model.HeaderText = "Child Model";
            this.child_model.Name = "child_model";
            this.child_model.Width = 87;
            // 
            // user
            // 
            this.user.DataPropertyName = "User";
            this.user.HeaderText = "User";
            this.user.Name = "user";
            this.user.Width = 54;
            // 
            // printdate
            // 
            this.printdate.DataPropertyName = "PrintDate";
            this.printdate.HeaderText = "Print Date";
            this.printdate.Name = "printdate";
            this.printdate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.printdate.Width = 79;
            // 
            // shipdate
            // 
            this.shipdate.DataPropertyName = "ShipDate";
            this.shipdate.HeaderText = "Ship Date";
            this.shipdate.Name = "shipdate";
            this.shipdate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.shipdate.Width = 79;
            // 
            // btnRTV
            // 
            this.btnRTV.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRTV.ForeColor = System.Drawing.Color.Red;
            this.btnRTV.Location = new System.Drawing.Point(504, 116);
            this.btnRTV.Name = "btnRTV";
            this.btnRTV.Size = new System.Drawing.Size(128, 25);
            this.btnRTV.TabIndex = 15;
            this.btnRTV.Text = "Control RTV";
            this.btnRTV.UseVisualStyleBackColor = true;
            this.btnRTV.Click += new System.EventHandler(this.btnRTV_Click);
            // 
            // frmBoxid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(787, 668);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBoxid";
            this.Text = "Box ID";
            this.Load += new System.EventHandler(this.frmBoxid_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoxId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSearchBoxId;
        private System.Windows.Forms.Button btnAddBoxId;
        private System.Windows.Forms.DateTimePicker dtpPrintDate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdbPrintDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbShipDate;
        private System.Windows.Forms.DateTimePicker dtpShipDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtShipStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnShipHistory;
        private System.Windows.Forms.TextBox txtInvoice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtShipto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvBoxId;
        private System.Windows.Forms.Button btnShip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn shipchk;
        private System.Windows.Forms.DataGridViewTextBoxColumn boxid;
        private System.Windows.Forms.DataGridViewTextBoxColumn child_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn user;
        private System.Windows.Forms.DataGridViewTextBoxColumn printdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipdate;
        private System.Windows.Forms.Button btnRTV;
    }
}

