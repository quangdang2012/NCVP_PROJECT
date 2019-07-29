using System;
using System.Windows.Forms;

namespace ReportList
{
    public partial class frmUpdateReport : Com.Nidec.Mes.Framework.FormCommon
    {
        TfSQL tf = new TfSQL();
        int ID;
        public frmUpdateReport(Update upVo)
        {
            InitializeComponent();
            txtModel.Text = upVo.Model;
            txtLine.Text = upVo.Line;
            txtAssy.Text = upVo.Assy;
            txtProcess.Text = upVo.Process;
            cmbShift.Text = upVo.Shift;
            cmbStatus.Text = upVo.Status;
            txtFailure.Text = upVo.Failure;
            txtDRI.Text = upVo.DRI;
            txtDetail.Text = upVo.Detail;
            dtpDeadLine.Value = DateTime.Parse(upVo.Deadline);
            txtCause.Text = upVo.Cause;
            txtMeasure.Text = upVo.Measure;
            txtAttach.Text = upVo.AttachPath;
            ID = upVo.ID;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog o1 = new FolderBrowserDialog();
            o1.ShowDialog();
            txtAttach.Text = o1.SelectedPath.Replace("\\", "\\\\");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            tf.sqlExecuteScalarString("UPDATE report_list SET shift = '" + cmbShift.Text + "', status = '" + cmbStatus.Text + "', dri = '" + txtDRI.Text + "', measure = '" + txtMeasure.Text + "', deadline = '" + dtpDeadLine.Value.ToShortDateString() + "', attach_file = '" + txtAttach.Text + "' WHERE id = '" + ID + "'");

            MessageBox.Show("Report has been updated successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o2 = new OpenFileDialog();
            o2.ShowDialog();
            txtAttach.Text = o2.FileName.Replace("\\", "\\\\");
        }
    }
}
