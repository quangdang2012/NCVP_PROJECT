using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ReportList
{
    public partial class frmReportList : Com.Nidec.Mes.Framework.FormCommon
    {
        TfSQL tf = new TfSQL();
        DataTable dt = new DataTable();
        DataGridViewButtonColumn openButton;

        public frmReportList()
        {
            InitializeComponent();
            dgvReportList.AutoGenerateColumns = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new frmAddReport().ShowDialog();
            dt.Clear();
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update up = new Update
            {
                ID = int.Parse(dgvReportList.CurrentRow.Cells["id"].Value.ToString()),
                Model = dgvReportList.CurrentRow.Cells["colModel"].Value.ToString(),
                Line = dgvReportList.CurrentRow.Cells["colLine"].Value.ToString(),
                Assy = dgvReportList.CurrentRow.Cells["colAssy"].Value.ToString(),
                Process = dgvReportList.CurrentRow.Cells["colProcess"].Value.ToString(),
                Shift = dgvReportList.CurrentRow.Cells["colShift"].Value.ToString(),
                Status = dgvReportList.CurrentRow.Cells["colStatus"].Value.ToString(),
                Failure = dgvReportList.CurrentRow.Cells["colFail"].Value.ToString(),
                DRI = dgvReportList.CurrentRow.Cells["colDri"].Value.ToString(),
                Deadline = dgvReportList.CurrentRow.Cells["colDeadline"].Value.ToString(),
                Detail = dgvReportList.CurrentRow.Cells["colDetail"].Value.ToString(),
                Cause = dgvReportList.CurrentRow.Cells["colCause"].Value.ToString(),
                Measure = dgvReportList.CurrentRow.Cells["colMeasure"].Value.ToString(),
                AttachPath = dgvReportList.CurrentRow.Cells["colAttach"].Value.ToString()
            };
            new frmUpdateReport(up).ShowDialog();
            dt.Clear();
            LoadData();
        }

        private void frmReportList_Load(object sender, EventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append("VERSION_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                //version.Append(deploy.Build);
                //version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }
            LoadData();
            addButtons(dgvReportList);
        }

        private void LoadData()
        {
            tf.getComboBoxData("SELECT distinct model FROM report_list", ref cmbModel);
            tf.getComboBoxData("SELECT distinct line FROM report_list", ref cmbLine);
            tf.getComboBoxData("SELECT distinct process FROM report_list", ref cmbProcess);
            tf.getComboBoxData("SELECT distinct assy FROM report_list", ref cmbAssy);

            tf.sqlDataAdapterFillDatatable("SELECT * FROM report_list order by id", ref dt);
            dgvReportList.DataSource = dt;
            ShowRownum(dgvReportList);
            MakeColor();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM report_list WHERE 1 = 1");

            if (!String.IsNullOrEmpty(cmbModel.Text))
            {
                sql.Append(" AND model = '" + cmbModel.Text + "'");
            }
            if (!String.IsNullOrEmpty(cmbLine.Text))
            {
                sql.Append(" AND line = '" + cmbLine.Text + "'");
            }
            if (!String.IsNullOrEmpty(cmbAssy.Text))
            {
                sql.Append(" AND assy = '" + cmbAssy.Text + "'");
            }
            if (!String.IsNullOrEmpty(cmbProcess.Text))
            {
                sql.Append(" AND process = '" + cmbProcess.Text + "'");
            }
            if (!String.IsNullOrEmpty(cmbShift.Text))
            {
                sql.Append(" AND shift = '" + cmbShift.Text + "'");
            }
            if (!String.IsNullOrEmpty(cmbStatus.Text))
            {
                sql.Append(" AND status = '" + cmbStatus.Text + "'");
            }
            if (rdRecordDate.Checked)
            {
                sql.Append(" AND date_record = '" + dtpRecordDate.Value.ToShortDateString() + "'");
            }
            if (rdDeadline.Checked)
            {
                sql.Append(" AND deadline = '" + dtpDeadLine.Value.ToShortDateString() + "'");
            }
            sql.Append(" ORDER BY id asc");

            dt.Clear();
            tf.sqlDataAdapterFillDatatable(sql.ToString(), ref dt);
            dgvReportList.DataSource = dt;
            ShowRownum(dgvReportList);
            MakeColor();
        }

        private void MakeColor()
        {
            for (int i = 0; i < dgvReportList.RowCount; i++)
            {
                if (dgvReportList["colStatus", i].Value.ToString() == "Open")
                {
                    dgvReportList.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    dgvReportList.Rows[i].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                tf.sqlExecuteScalarString("DELETE FROM report_list WHERE id = '" + dgvReportList.CurrentRow.Cells["id"].Value.ToString() + "'");
                dgvReportList.Rows.RemoveAt(dgvReportList.CurrentRow.Index);
                MessageBox.Show("Deleted successfull!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDelete.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Not successfull!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }

        }

        private void dgvReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvReportList.CurrentRow.Cells["colStatus"].Value.ToString() == "Open") { btnDelete.Enabled = true; btnUpdate.Enabled = true; }
            else { btnDelete.Enabled = false; btnUpdate.Enabled = false; }
        }

        // Sub procedure: Add button to datagridview
        private void addButtons(DataGridView dgv)
        {
            // Set OPEN button for every user
            openButton = new DataGridViewButtonColumn();
            openButton.HeaderText = "Open";
            openButton.Text = "Open";
            openButton.UseColumnTextForButtonValue = true;
            openButton.Width = 80;
            dgv.Columns.Add(openButton);
        }
        private void ShowRownum(DataGridView dgv)
        {
            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
        private void dgvReportList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());
            string folder = dgvReportList["colAttach", currentRow].Value.ToString();

            // OPEN button generate frmModule by view mode without delegate event
            if (dgvReportList.Columns[e.ColumnIndex] == openButton && currentRow >= 0)
            {
                Process prc = new Process();
                try
                {
                    prc.StartInfo.FileName = folder;
                    prc.Start();
                }
                catch { }
            }
        }
    }
}