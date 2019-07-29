namespace BoxIdDb
{
    partial class frmReplace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReplace));
            this.label2 = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvProductSerial = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serialno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linepass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblBoxID = new System.Windows.Forms.Label();
            this.txtBeforeSerial = new System.Windows.Forms.TextBox();
            this.txtAfterSerial = new System.Windows.Forms.TextBox();
            this.txtRep = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSerial)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Barcode NG:";
            // 
            // btnReplace
            // 
            this.btnReplace.Enabled = false;
            this.btnReplace.Location = new System.Drawing.Point(248, 12);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(83, 46);
            this.btnReplace.TabIndex = 11;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Barcode OK:";
            // 
            // dgvProductSerial
            // 
            this.dgvProductSerial.AllowUserToAddRows = false;
            this.dgvProductSerial.AllowUserToDeleteRows = false;
            this.dgvProductSerial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProductSerial.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvProductSerial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSerial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.serialno,
            this.model,
            this.Lot,
            this.Line,
            this.colConfig,
            this.process,
            this.linepass,
            this.testtime});
            this.dgvProductSerial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvProductSerial.Location = new System.Drawing.Point(0, 74);
            this.dgvProductSerial.Name = "dgvProductSerial";
            this.dgvProductSerial.ReadOnly = true;
            this.dgvProductSerial.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvProductSerial.RowTemplate.Height = 21;
            this.dgvProductSerial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvProductSerial.Size = new System.Drawing.Size(616, 119);
            this.dgvProductSerial.TabIndex = 12;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 43;
            // 
            // serialno
            // 
            this.serialno.DataPropertyName = "serialno";
            this.serialno.HeaderText = "Serial No";
            this.serialno.Name = "serialno";
            this.serialno.ReadOnly = true;
            this.serialno.Width = 75;
            // 
            // model
            // 
            this.model.DataPropertyName = "model";
            this.model.HeaderText = "Model";
            this.model.Name = "model";
            this.model.ReadOnly = true;
            this.model.Width = 61;
            // 
            // Lot
            // 
            this.Lot.DataPropertyName = "lot";
            this.Lot.HeaderText = "Lot";
            this.Lot.Name = "Lot";
            this.Lot.ReadOnly = true;
            this.Lot.Width = 47;
            // 
            // Line
            // 
            this.Line.DataPropertyName = "line";
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            this.Line.Width = 52;
            // 
            // colConfig
            // 
            this.colConfig.DataPropertyName = "config";
            this.colConfig.HeaderText = "Config";
            this.colConfig.Name = "colConfig";
            this.colConfig.ReadOnly = true;
            this.colConfig.Width = 62;
            // 
            // process
            // 
            this.process.DataPropertyName = "process";
            this.process.HeaderText = "Process";
            this.process.Name = "process";
            this.process.ReadOnly = true;
            this.process.Width = 70;
            // 
            // linepass
            // 
            this.linepass.DataPropertyName = "linepass";
            this.linepass.HeaderText = "Judge";
            this.linepass.Name = "linepass";
            this.linepass.ReadOnly = true;
            this.linepass.Width = 61;
            // 
            // testtime
            // 
            this.testtime.DataPropertyName = "testtime";
            this.testtime.HeaderText = "Test Time";
            this.testtime.Name = "testtime";
            this.testtime.ReadOnly = true;
            this.testtime.Width = 79;
            // 
            // lblBoxID
            // 
            this.lblBoxID.AutoSize = true;
            this.lblBoxID.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBoxID.Location = new System.Drawing.Point(375, 29);
            this.lblBoxID.Name = "lblBoxID";
            this.lblBoxID.Size = new System.Drawing.Size(62, 19);
            this.lblBoxID.TabIndex = 6;
            this.lblBoxID.Text = "BoxID:";
            // 
            // txtBeforeSerial
            // 
            this.txtBeforeSerial.Location = new System.Drawing.Point(75, 12);
            this.txtBeforeSerial.Name = "txtBeforeSerial";
            this.txtBeforeSerial.Size = new System.Drawing.Size(148, 20);
            this.txtBeforeSerial.TabIndex = 13;
            this.txtBeforeSerial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBeforeSerial_KeyDown);
            // 
            // txtAfterSerial
            // 
            this.txtAfterSerial.Location = new System.Drawing.Point(75, 38);
            this.txtAfterSerial.Name = "txtAfterSerial";
            this.txtAfterSerial.Size = new System.Drawing.Size(148, 20);
            this.txtAfterSerial.TabIndex = 14;
            this.txtAfterSerial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAfterSerial_KeyDown);
            // 
            // txtRep
            // 
            this.txtRep.Location = new System.Drawing.Point(248, 12);
            this.txtRep.Name = "txtRep";
            this.txtRep.Size = new System.Drawing.Size(83, 20);
            this.txtRep.TabIndex = 15;
            this.txtRep.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRep_KeyDown);
            // 
            // frmReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(616, 193);
            this.Controls.Add(this.txtAfterSerial);
            this.Controls.Add(this.txtBeforeSerial);
            this.Controls.Add(this.dgvProductSerial);
            this.Controls.Add(this.lblBoxID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.txtRep);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmReplace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replace Serial";
            this.Load += new System.EventHandler(this.frmReplace_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSerial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvProductSerial;
        private System.Windows.Forms.Label lblBoxID;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialno;
        private System.Windows.Forms.DataGridViewTextBoxColumn model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn process;
        private System.Windows.Forms.DataGridViewTextBoxColumn linepass;
        private System.Windows.Forms.DataGridViewTextBoxColumn testtime;
        private System.Windows.Forms.TextBox txtBeforeSerial;
        private System.Windows.Forms.TextBox txtAfterSerial;
        private System.Windows.Forms.TextBox txtRep;
    }
}

