namespace ReportList
{
    partial class CommonHeaderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonHeaderControl));
            this.Icon_pb = new System.Windows.Forms.PictureBox();
            this.Header_lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Icon_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // Icon_pb
            // 
            resources.ApplyResources(this.Icon_pb, "Icon_pb");
            this.Icon_pb.Image = global::ReportList.Properties.Resources.NIDEC_Logo_small;
            this.Icon_pb.Name = "Icon_pb";
            this.Icon_pb.TabStop = false;
            // 
            // Header_lbl
            // 
            this.Header_lbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.Header_lbl, "Header_lbl");
            this.Header_lbl.Name = "Header_lbl";
            // 
            // CommonHeaderControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Header_lbl);
            this.Controls.Add(this.Icon_pb);
            this.Name = "CommonHeaderControl";
            this.Load += new System.EventHandler(this.CommonHeaderControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Icon_pb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Icon_pb;
        private System.Windows.Forms.Label Header_lbl;
    }
}
