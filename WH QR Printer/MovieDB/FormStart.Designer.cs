namespace WhQrPrinter
{
    partial class FormStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStart));
            this.btnNCVP = new System.Windows.Forms.Button();
            this.btnNCVH = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNCVP
            // 
            this.btnNCVP.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNCVP.Location = new System.Drawing.Point(63, 48);
            this.btnNCVP.Name = "btnNCVP";
            this.btnNCVP.Size = new System.Drawing.Size(184, 68);
            this.btnNCVP.TabIndex = 0;
            this.btnNCVP.Text = "NCVP";
            this.btnNCVP.UseVisualStyleBackColor = true;
            this.btnNCVP.Click += new System.EventHandler(this.btnNCVP_Click);
            // 
            // btnNCVH
            // 
            this.btnNCVH.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNCVH.Location = new System.Drawing.Point(63, 153);
            this.btnNCVH.Name = "btnNCVH";
            this.btnNCVH.Size = new System.Drawing.Size(184, 68);
            this.btnNCVH.TabIndex = 1;
            this.btnNCVH.Text = "NCVH";
            this.btnNCVH.UseVisualStyleBackColor = true;
            this.btnNCVH.Click += new System.EventHandler(this.btnNCVH_Click);
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(317, 269);
            this.Controls.Add(this.btnNCVH);
            this.Controls.Add(this.btnNCVP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WhQrPrinter";
            this.Load += new System.EventHandler(this.FormStart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNCVP;
        private System.Windows.Forms.Button btnNCVH;
    }
}

