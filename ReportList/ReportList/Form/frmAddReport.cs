using System;
using System.Windows.Forms;

namespace ReportList
{
    public partial class frmAddReport : Com.Nidec.Mes.Framework.FormCommon
    {
        TfSQL tf = new TfSQL();
        public frmAddReport()
        {
            InitializeComponent();
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

        private void frmAddReport_Load(object sender, EventArgs e)
        {
            tf.getComboBoxData("SELECT distinct model FROM report_list", ref cmbModel);
            tf.getComboBoxData("SELECT distinct line FROM report_list", ref cmbLine);
            tf.getComboBoxData("SELECT distinct process FROM report_list", ref cmbProcess);
            tf.getComboBoxData("SELECT distinct assy FROM report_list", ref cmbAssy);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cmbModel.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbModel.Focus();
                return;
            }
            if (String.IsNullOrEmpty(cmbLine.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLine.Focus();
                return;
            }
            if (String.IsNullOrEmpty(cmbAssy.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbAssy.Focus();
                return;
            }
            if (String.IsNullOrEmpty(cmbProcess.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProcess.Focus();
                return;
            }
            if (String.IsNullOrEmpty(cmbShift.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbShift.Focus();
                return;
            }
            if (String.IsNullOrEmpty(cmbStatus.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtDetail.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDetail.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCause.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCause.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtMeasure.Text))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMeasure.Focus();
                return;
            }
            tf.sqlExecuteScalarString("INSERT INTO report_list(model, line, assy, process, failure, shift, date_record, detail, status, dri, cause, measure, deadline, attach_file) VALUES ('" + cmbModel.Text + "','" + cmbLine.Text + "','" + cmbAssy.Text + "','" + cmbProcess.Text + "','" + txtFailure.Text + "','" + cmbShift.Text + "','" + DateTime.Today + "','" + txtDetail.Text + "','" + cmbStatus.Text + "','" + txtDRI.Text + "','" + txtCause.Text + "','" + txtMeasure.Text + "','" + dtpDeadLine.Value.ToShortDateString() + "','" + txtAttach.Text + "')");

            MessageBox.Show("Report has been added successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetControlValues.ResetControlValue(tableLayoutPanel2);
            ResetControlValues.ResetControlValue(panel3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o2 = new OpenFileDialog();
            o2.ShowDialog();
            txtAttach.Text = o2.FileName.Replace("\\", "\\\\");
        }
    }
}
