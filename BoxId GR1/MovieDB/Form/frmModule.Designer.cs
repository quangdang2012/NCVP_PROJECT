namespace BoxIdDb
{
    partial class frmModule
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModule));
            this.btnCancel = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpPrintDate = new System.Windows.Forms.DateTimePicker();
            this.txtProductSerial = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.dgvLine = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRegisterBoxId = new System.Windows.Forms.Button();
            this.txtOkCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDeleteSelection = new System.Windows.Forms.Button();
            this.btnChangeLimit = new System.Windows.Forms.Button();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.dgvDateCode = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvSI = new System.Windows.Forms.DataGridView();
            this.btnDeleteBoxId = new System.Windows.Forms.Button();
            this.txtBoxIdPrint = new System.Windows.Forms.TextBox();
            this.dgvDateCode2 = new System.Windows.Forms.DataGridView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnReplaceSerial = new System.Windows.Forms.Button();
            this.ckbDeleteBox = new System.Windows.Forms.CheckBox();
            this.dgvProductSerial = new System.Windows.Forms.DataGridView();
            this.serialno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thurst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thurst_mc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noise = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noise_mc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblLotNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDateCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDateCode2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSerial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(709, 162);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 24);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(403, 97);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "User: ";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(468, 94);
            this.txtUser.MaxLength = 9999;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(227, 20);
            this.txtUser.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Print Date: ";
            // 
            // txtBoxId
            // 
            this.txtBoxId.Enabled = false;
            this.txtBoxId.Location = new System.Drawing.Point(114, 94);
            this.txtBoxId.Name = "txtBoxId";
            this.txtBoxId.Size = new System.Drawing.Size(254, 20);
            this.txtBoxId.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Box ID: ";
            // 
            // dtpPrintDate
            // 
            this.dtpPrintDate.Enabled = false;
            this.dtpPrintDate.Location = new System.Drawing.Point(114, 119);
            this.dtpPrintDate.Name = "dtpPrintDate";
            this.dtpPrintDate.Size = new System.Drawing.Size(254, 20);
            this.dtpPrintDate.TabIndex = 12;
            // 
            // txtProductSerial
            // 
            this.txtProductSerial.Location = new System.Drawing.Point(114, 143);
            this.txtProductSerial.Name = "txtProductSerial";
            this.txtProductSerial.Size = new System.Drawing.Size(254, 20);
            this.txtProductSerial.TabIndex = 5;
            this.txtProductSerial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductSerial_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Product Serial: ";
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Location = new System.Drawing.Point(600, 162);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(100, 24);
            this.btnDeleteAll.TabIndex = 11;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // dgvLine
            // 
            this.dgvLine.AllowUserToAddRows = false;
            this.dgvLine.AllowUserToDeleteRows = false;
            this.dgvLine.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLine.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvLine.Location = new System.Drawing.Point(83, 20);
            this.dgvLine.Name = "dgvLine";
            this.dgvLine.ReadOnly = true;
            this.dgvLine.RowTemplate.Height = 21;
            this.dgvLine.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvLine.Size = new System.Drawing.Size(314, 66);
            this.dgvLine.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "By Line: ";
            // 
            // btnRegisterBoxId
            // 
            this.btnRegisterBoxId.Location = new System.Drawing.Point(494, 162);
            this.btnRegisterBoxId.Name = "btnRegisterBoxId";
            this.btnRegisterBoxId.Size = new System.Drawing.Size(100, 24);
            this.btnRegisterBoxId.TabIndex = 11;
            this.btnRegisterBoxId.Text = "Register Box ID";
            this.btnRegisterBoxId.UseVisualStyleBackColor = true;
            this.btnRegisterBoxId.Click += new System.EventHandler(this.btnRegisterBoxId_Click);
            // 
            // txtOkCount
            // 
            this.txtOkCount.Enabled = false;
            this.txtOkCount.Location = new System.Drawing.Point(468, 118);
            this.txtOkCount.Name = "txtOkCount";
            this.txtOkCount.Size = new System.Drawing.Size(227, 20);
            this.txtOkCount.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(403, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "OK Count: ";
            // 
            // btnDeleteSelection
            // 
            this.btnDeleteSelection.Location = new System.Drawing.Point(600, 192);
            this.btnDeleteSelection.Name = "btnDeleteSelection";
            this.btnDeleteSelection.Size = new System.Drawing.Size(100, 24);
            this.btnDeleteSelection.TabIndex = 11;
            this.btnDeleteSelection.Text = "Delete Selection";
            this.btnDeleteSelection.UseVisualStyleBackColor = true;
            this.btnDeleteSelection.Click += new System.EventHandler(this.btnDeleteSelection_Click);
            // 
            // btnChangeLimit
            // 
            this.btnChangeLimit.Location = new System.Drawing.Point(706, 118);
            this.btnChangeLimit.Name = "btnChangeLimit";
            this.btnChangeLimit.Size = new System.Drawing.Size(100, 24);
            this.btnChangeLimit.TabIndex = 11;
            this.btnChangeLimit.Text = "Change Limit";
            this.btnChangeLimit.UseVisualStyleBackColor = true;
            this.btnChangeLimit.Click += new System.EventHandler(this.btnChangeLimit_Click);
            // 
            // txtLimit
            // 
            this.txtLimit.Enabled = false;
            this.txtLimit.Location = new System.Drawing.Point(706, 94);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(100, 20);
            this.txtLimit.TabIndex = 5;
            // 
            // dgvDateCode
            // 
            this.dgvDateCode.AllowUserToAddRows = false;
            this.dgvDateCode.AllowUserToDeleteRows = false;
            this.dgvDateCode.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvDateCode.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDateCode.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvDateCode.Location = new System.Drawing.Point(452, 20);
            this.dgvDateCode.Name = "dgvDateCode";
            this.dgvDateCode.ReadOnly = true;
            this.dgvDateCode.RowTemplate.Height = 21;
            this.dgvDateCode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDateCode.Size = new System.Drawing.Size(381, 66);
            this.dgvDateCode.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(403, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "By Lot: ";
            // 
            // dgvSI
            // 
            this.dgvSI.AllowUserToAddRows = false;
            this.dgvSI.AllowUserToDeleteRows = false;
            this.dgvSI.BackgroundColor = System.Drawing.Color.Khaki;
            this.dgvSI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSI.Location = new System.Drawing.Point(2, 3);
            this.dgvSI.Name = "dgvSI";
            this.dgvSI.ReadOnly = true;
            this.dgvSI.RowTemplate.Height = 21;
            this.dgvSI.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSI.Size = new System.Drawing.Size(897, 424);
            this.dgvSI.TabIndex = 10;
            // 
            // btnDeleteBoxId
            // 
            this.btnDeleteBoxId.Location = new System.Drawing.Point(600, 162);
            this.btnDeleteBoxId.Name = "btnDeleteBoxId";
            this.btnDeleteBoxId.Size = new System.Drawing.Size(100, 24);
            this.btnDeleteBoxId.TabIndex = 16;
            this.btnDeleteBoxId.Text = "Delete BoxID";
            this.btnDeleteBoxId.UseVisualStyleBackColor = true;
            this.btnDeleteBoxId.Visible = false;
            this.btnDeleteBoxId.Click += new System.EventHandler(this.btnDeleteBoxId_Click);
            // 
            // txtBoxIdPrint
            // 
            this.txtBoxIdPrint.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBoxIdPrint.Location = new System.Drawing.Point(153, 8);
            this.txtBoxIdPrint.Multiline = true;
            this.txtBoxIdPrint.Name = "txtBoxIdPrint";
            this.txtBoxIdPrint.Size = new System.Drawing.Size(190, 24);
            this.txtBoxIdPrint.TabIndex = 5;
            this.txtBoxIdPrint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxIdPrint.Visible = false;
            // 
            // dgvDateCode2
            // 
            this.dgvDateCode2.AllowUserToAddRows = false;
            this.dgvDateCode2.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvDateCode2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDateCode2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDateCode2.ColumnHeadersHeight = 18;
            this.dgvDateCode2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDateCode2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDateCode2.EnableHeadersVisualStyles = false;
            this.dgvDateCode2.GridColor = System.Drawing.Color.White;
            this.dgvDateCode2.Location = new System.Drawing.Point(465, 8);
            this.dgvDateCode2.Name = "dgvDateCode2";
            this.dgvDateCode2.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDateCode2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDateCode2.RowHeadersVisible = false;
            this.dgvDateCode2.RowHeadersWidth = 40;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvDateCode2.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDateCode2.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dgvDateCode2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvDateCode2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvDateCode2.RowTemplate.Height = 18;
            this.dgvDateCode2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDateCode2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDateCode2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDateCode2.Size = new System.Drawing.Size(344, 43);
            this.dgvDateCode2.TabIndex = 20;
            this.dgvDateCode2.Visible = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(494, 192);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 24);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "Re-print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnReplaceSerial
            // 
            this.btnReplaceSerial.Location = new System.Drawing.Point(405, 162);
            this.btnReplaceSerial.Name = "btnReplaceSerial";
            this.btnReplaceSerial.Size = new System.Drawing.Size(83, 54);
            this.btnReplaceSerial.TabIndex = 21;
            this.btnReplaceSerial.Text = "Replace Serial";
            this.btnReplaceSerial.UseVisualStyleBackColor = true;
            this.btnReplaceSerial.Click += new System.EventHandler(this.btnReplaceSerial_Click);
            // 
            // ckbDeleteBox
            // 
            this.ckbDeleteBox.AutoSize = true;
            this.ckbDeleteBox.Location = new System.Drawing.Point(602, 141);
            this.ckbDeleteBox.Name = "ckbDeleteBox";
            this.ckbDeleteBox.Size = new System.Drawing.Size(89, 17);
            this.ckbDeleteBox.TabIndex = 22;
            this.ckbDeleteBox.Text = "Delete BoxID";
            this.ckbDeleteBox.UseVisualStyleBackColor = true;
            this.ckbDeleteBox.CheckedChanged += new System.EventHandler(this.ckbDeleteBox_CheckedChanged);
            // 
            // dgvProductSerial
            // 
            this.dgvProductSerial.AllowUserToAddRows = false;
            this.dgvProductSerial.AllowUserToDeleteRows = false;
            this.dgvProductSerial.AllowUserToResizeColumns = false;
            this.dgvProductSerial.AllowUserToResizeRows = false;
            this.dgvProductSerial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProductSerial.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvProductSerial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSerial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.serialno,
            this.model,
            this.Lot,
            this.Line,
            this.thurst,
            this.thurst_mc,
            this.noise,
            this.noise_mc});
            this.dgvProductSerial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductSerial.Location = new System.Drawing.Point(0, 0);
            this.dgvProductSerial.Name = "dgvProductSerial";
            this.dgvProductSerial.ReadOnly = true;
            this.dgvProductSerial.RowTemplate.Height = 21;
            this.dgvProductSerial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvProductSerial.Size = new System.Drawing.Size(845, 450);
            this.dgvProductSerial.TabIndex = 23;
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
            // thurst
            // 
            this.thurst.DataPropertyName = "thurst";
            this.thurst.HeaderText = "Thurst";
            this.thurst.Name = "thurst";
            this.thurst.ReadOnly = true;
            this.thurst.Width = 62;
            // 
            // thurst_mc
            // 
            this.thurst_mc.DataPropertyName = "thurst_mc";
            this.thurst_mc.HeaderText = "Thurst MC";
            this.thurst_mc.Name = "thurst_mc";
            this.thurst_mc.ReadOnly = true;
            this.thurst_mc.Width = 81;
            // 
            // noise
            // 
            this.noise.DataPropertyName = "noise";
            this.noise.HeaderText = "Noise";
            this.noise.Name = "noise";
            this.noise.ReadOnly = true;
            this.noise.Width = 59;
            // 
            // noise_mc
            // 
            this.noise_mc.DataPropertyName = "noise_mc";
            this.noise_mc.HeaderText = "Noise MC";
            this.noise_mc.Name = "noise_mc";
            this.noise_mc.ReadOnly = true;
            this.noise_mc.Width = 78;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblLotNum);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.ckbDeleteBox);
            this.splitContainer1.Panel1.Controls.Add(this.dgvLine);
            this.splitContainer1.Panel1.Controls.Add(this.btnReplaceSerial);
            this.splitContainer1.Panel1.Controls.Add(this.txtBoxId);
            this.splitContainer1.Panel1.Controls.Add(this.dgvDateCode2);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductSerial);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteBoxId);
            this.splitContainer1.Panel1.Controls.Add(this.txtBoxIdPrint);
            this.splitContainer1.Panel1.Controls.Add(this.dgvDateCode);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.dtpPrintDate);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtLimit);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.txtOkCount);
            this.splitContainer1.Panel1.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel1.Controls.Add(this.txtUser);
            this.splitContainer1.Panel1.Controls.Add(this.btnChangeLimit);
            this.splitContainer1.Panel1.Controls.Add(this.btnPrint);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteAll);
            this.splitContainer1.Panel1.Controls.Add(this.btnRegisterBoxId);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeleteSelection);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvProductSerial);
            this.splitContainer1.Size = new System.Drawing.Size(845, 678);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 24;
            // 
            // lblLotNum
            // 
            this.lblLotNum.AutoSize = true;
            this.lblLotNum.BackColor = System.Drawing.Color.LightGreen;
            this.lblLotNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLotNum.Location = new System.Drawing.Point(20, 183);
            this.lblLotNum.Name = "lblLotNum";
            this.lblLotNum.Size = new System.Drawing.Size(140, 25);
            this.lblLotNum.TabIndex = 23;
            this.lblLotNum.Text = "Lot Number:";
            // 
            // frmModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(845, 678);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmModule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Serial";
            this.Load += new System.EventHandler(this.frmModule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDateCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDateCode2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSerial)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpPrintDate;
        private System.Windows.Forms.TextBox txtProductSerial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.DataGridView dgvLine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRegisterBoxId;
        private System.Windows.Forms.TextBox txtOkCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDeleteSelection;
        private System.Windows.Forms.Button btnChangeLimit;
        private System.Windows.Forms.TextBox txtLimit;
        private System.Windows.Forms.DataGridView dgvDateCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvSI;
        private System.Windows.Forms.Button btnDeleteBoxId;
        private System.Windows.Forms.TextBox txtBoxIdPrint;
        private System.Windows.Forms.DataGridView dgvDateCode2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnReplaceSerial;
        private System.Windows.Forms.CheckBox ckbDeleteBox;
        private System.Windows.Forms.DataGridView dgvProductSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn serialno;
        private System.Windows.Forms.DataGridViewTextBoxColumn model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn thurst;
        private System.Windows.Forms.DataGridViewTextBoxColumn thurst_mc;
        private System.Windows.Forms.DataGridViewTextBoxColumn noise;
        private System.Windows.Forms.DataGridViewTextBoxColumn noise_mc;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblLotNum;
    }
}

