namespace JigQuick
{
    partial class frmOmni
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOmni));
            this.lblModel = new System.Windows.Forms.Label();
            this.lbl_model = new System.Windows.Forms.Label();
            this.lbl_barcode = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_barcode = new System.Windows.Forms.TextBox();
            this.lblTestTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlThurst = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlNoise = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNoiseMC = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModel.Location = new System.Drawing.Point(82, 14);
            this.lblModel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(89, 20);
            this.lblModel.TabIndex = 64;
            this.lblModel.Text = "LDP_5SG";
            // 
            // lbl_model
            // 
            this.lbl_model.AutoSize = true;
            this.lbl_model.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_model.Location = new System.Drawing.Point(12, 14);
            this.lbl_model.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lbl_model.Name = "lbl_model";
            this.lbl_model.Size = new System.Drawing.Size(56, 20);
            this.lbl_model.TabIndex = 3;
            this.lbl_model.Text = "Model:";
            // 
            // lbl_barcode
            // 
            this.lbl_barcode.AutoSize = true;
            this.lbl_barcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_barcode.Location = new System.Drawing.Point(210, 14);
            this.lbl_barcode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lbl_barcode.Name = "lbl_barcode";
            this.lbl_barcode.Size = new System.Drawing.Size(73, 20);
            this.lbl_barcode.TabIndex = 9;
            this.lbl_barcode.Text = "Barcode:";
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(298, 50);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(161, 53);
            this.btnReset.TabIndex = 60;
            this.btnReset.Text = "Reset Judge";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblCounter.ForeColor = System.Drawing.Color.Blue;
            this.lblCounter.Location = new System.Drawing.Point(81, 59);
            this.lblCounter.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(27, 29);
            this.lblCounter.TabIndex = 62;
            this.lblCounter.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 61;
            this.label3.Text = "Count";
            // 
            // txt_barcode
            // 
            this.txt_barcode.Font = new System.Drawing.Font("Arial", 14.25F);
            this.txt_barcode.Location = new System.Drawing.Point(298, 9);
            this.txt_barcode.Name = "txt_barcode";
            this.txt_barcode.Size = new System.Drawing.Size(161, 29);
            this.txt_barcode.TabIndex = 63;
            this.txt_barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_barcode_KeyDown);
            // 
            // lblTestTime
            // 
            this.lblTestTime.AutoSize = true;
            this.lblTestTime.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblTestTime.Location = new System.Drawing.Point(496, 7);
            this.lblTestTime.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblTestTime.Name = "lblTestTime";
            this.lblTestTime.Size = new System.Drawing.Size(225, 29);
            this.lblTestTime.TabIndex = 25;
            this.lblTestTime.Text = "Test Times Thurst:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(157, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 57;
            this.label1.Text = "THURST CHECK";
            // 
            // pnlThurst
            // 
            this.pnlThurst.BackColor = System.Drawing.Color.White;
            this.pnlThurst.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlThurst.Location = new System.Drawing.Point(88, 166);
            this.pnlThurst.Name = "pnlThurst";
            this.pnlThurst.Size = new System.Drawing.Size(285, 280);
            this.pnlThurst.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(629, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 55;
            this.label2.Text = "NOISE CHECK";
            // 
            // pnlNoise
            // 
            this.pnlNoise.BackColor = System.Drawing.Color.White;
            this.pnlNoise.Location = new System.Drawing.Point(547, 166);
            this.pnlNoise.Name = "pnlNoise";
            this.pnlNoise.Size = new System.Drawing.Size(285, 280);
            this.pnlNoise.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(412, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.TabIndex = 65;
            this.label4.Text = "NOISE M/C";
            // 
            // lblNoiseMC
            // 
            this.lblNoiseMC.AutoSize = true;
            this.lblNoiseMC.BackColor = System.Drawing.Color.Blue;
            this.lblNoiseMC.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoiseMC.ForeColor = System.Drawing.Color.White;
            this.lblNoiseMC.Location = new System.Drawing.Point(446, 266);
            this.lblNoiseMC.Name = "lblNoiseMC";
            this.lblNoiseMC.Size = new System.Drawing.Size(0, 29);
            this.lblNoiseMC.TabIndex = 66;
            // 
            // frmOmni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(925, 469);
            this.Controls.Add(this.lblNoiseMC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTestTime);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.pnlNoise);
            this.Controls.Add(this.lbl_model);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlThurst);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.txt_barcode);
            this.Controls.Add(this.lbl_barcode);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmOmni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Final Check";
            this.Load += new System.EventHandler(this.frmOmni_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lbl_model;
        private System.Windows.Forms.Label lbl_barcode;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_barcode;
        private System.Windows.Forms.Label lblTestTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlThurst;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlNoise;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNoiseMC;
    }
}

