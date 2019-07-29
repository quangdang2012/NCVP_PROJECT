namespace WhQrPrinter
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnPrintPremac = new System.Windows.Forms.Button();
            this.dgvPremac = new System.Windows.Forms.DataGridView();
            this.cmbPiecePremac = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCopy = new System.Windows.Forms.DataGridView();
            this.btnPrintCopy = new System.Windows.Forms.Button();
            this.cmbPieceCopy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtScan = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCopy)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrintPremac
            // 
            this.btnPrintPremac.Location = new System.Drawing.Point(849, 170);
            this.btnPrintPremac.Name = "btnPrintPremac";
            this.btnPrintPremac.Size = new System.Drawing.Size(146, 25);
            this.btnPrintPremac.TabIndex = 7;
            this.btnPrintPremac.Text = "Print Premac";
            this.btnPrintPremac.UseVisualStyleBackColor = true;
            this.btnPrintPremac.Click += new System.EventHandler(this.btnPrintPremac_Click);
            // 
            // dgvPremac
            // 
            this.dgvPremac.AllowDrop = true;
            this.dgvPremac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPremac.Location = new System.Drawing.Point(12, 202);
            this.dgvPremac.Name = "dgvPremac";
            this.dgvPremac.RowTemplate.Height = 21;
            this.dgvPremac.Size = new System.Drawing.Size(1083, 517);
            this.dgvPremac.TabIndex = 6;
            // 
            // cmbPiecePremac
            // 
            this.cmbPiecePremac.FormattingEnabled = true;
            this.cmbPiecePremac.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbPiecePremac.Location = new System.Drawing.Point(736, 172);
            this.cmbPiecePremac.Name = "cmbPiecePremac";
            this.cmbPiecePremac.Size = new System.Drawing.Size(67, 21);
            this.cmbPiecePremac.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(647, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Print Piece:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(22, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Print from Premac text";
            // 
            // dgvCopy
            // 
            this.dgvCopy.AllowDrop = true;
            this.dgvCopy.AllowUserToAddRows = false;
            this.dgvCopy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCopy.Location = new System.Drawing.Point(12, 70);
            this.dgvCopy.Name = "dgvCopy";
            this.dgvCopy.RowTemplate.Height = 21;
            this.dgvCopy.Size = new System.Drawing.Size(1083, 62);
            this.dgvCopy.TabIndex = 3;
            // 
            // btnPrintCopy
            // 
            this.btnPrintCopy.Location = new System.Drawing.Point(849, 39);
            this.btnPrintCopy.Name = "btnPrintCopy";
            this.btnPrintCopy.Size = new System.Drawing.Size(146, 25);
            this.btnPrintCopy.TabIndex = 4;
            this.btnPrintCopy.Text = "Print Copy";
            this.btnPrintCopy.UseVisualStyleBackColor = true;
            this.btnPrintCopy.Click += new System.EventHandler(this.btnPrintCopy_Click);
            // 
            // cmbPieceCopy
            // 
            this.cmbPieceCopy.FormattingEnabled = true;
            this.cmbPieceCopy.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbPieceCopy.Location = new System.Drawing.Point(736, 41);
            this.cmbPieceCopy.Name = "cmbPieceCopy";
            this.cmbPieceCopy.Size = new System.Drawing.Size(67, 21);
            this.cmbPieceCopy.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(647, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Print Piece:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(22, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Print copy from scanned data";
            // 
            // txtScan
            // 
            this.txtScan.Location = new System.Drawing.Point(312, 41);
            this.txtScan.Name = "txtScan";
            this.txtScan.Size = new System.Drawing.Size(265, 20);
            this.txtScan.TabIndex = 1;
            this.txtScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScan_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Scan:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1107, 731);
            this.Controls.Add(this.txtScan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbPieceCopy);
            this.Controls.Add(this.cmbPiecePremac);
            this.Controls.Add(this.dgvCopy);
            this.Controls.Add(this.dgvPremac);
            this.Controls.Add(this.btnPrintCopy);
            this.Controls.Add(this.btnPrintPremac);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NCVP";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCopy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrintPremac;
        private System.Windows.Forms.DataGridView dgvPremac;
        private System.Windows.Forms.ComboBox cmbPiecePremac;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCopy;
        private System.Windows.Forms.Button btnPrintCopy;
        private System.Windows.Forms.ComboBox cmbPieceCopy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtScan;
        private System.Windows.Forms.Label label5;

    }
}

