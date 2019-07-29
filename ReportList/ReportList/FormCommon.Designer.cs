namespace Com.Nidec.Mes.Framework
{
    partial class FormCommon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCommon));
            this.Title_pnl = new System.Windows.Forms.Panel();
            this.Title_lbl = new Com.Nidec.Mes.Framework.LabelCommon();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.commonHeaderControl1 = new ReportList.CommonHeaderControl();
            this.Title_pnl.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title_pnl
            // 
            this.Title_pnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Title_pnl.Controls.Add(this.Title_lbl);
            resources.ApplyResources(this.Title_pnl, "Title_pnl");
            this.Title_pnl.Name = "Title_pnl";
            // 
            // Title_lbl
            // 
            this.Title_lbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Title_lbl.ControlId = null;
            resources.ApplyResources(this.Title_lbl, "Title_lbl");
            this.Title_lbl.Name = "Title_lbl";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.Title_pnl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.commonHeaderControl1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // commonHeaderControl1
            // 
            resources.ApplyResources(this.commonHeaderControl1, "commonHeaderControl1");
            this.commonHeaderControl1.Name = "commonHeaderControl1";
            // 
            // FormCommon
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(236)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormCommon";
            this.Load += new System.EventHandler(this.FormCommon_Load);
            this.Title_pnl.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Title_pnl;
        private LabelCommon Title_lbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ReportList.CommonHeaderControl commonHeaderControl1;
    }
}