using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.IO;

namespace QA_Management
{
    public partial class Form1 : Form
    {
        DataTable dtKK03;
        DataTable dtLD;
        DataTable dtLS;
        DataTable dtLAA, dtGA1;
        TfSQL update = new TfSQL();
        public static string model;

        /// <summary>
        // application name that is given from caller application for displaying itself with version on login screen
        /// </summary>
        private string applicationName;

        public Form1(string applicationname)
        {
            applicationName = applicationname;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = 100;
            this.Top = 30;

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {

                Version deploy = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;

                StringBuilder version = new StringBuilder();
                version.Append(applicationName + "_");
                //version.Append("VERSION_");
                version.Append(deploy.Major);
                version.Append("_");
                //version.Append(deploy.Minor);
                //version.Append("_");
                //version.Append(deploy.Build);
                //version.Append("_");
                version.Append(deploy.Revision);

                Version_lbl.Text = version.ToString();
            }

            update.getComboBoxData("select distinct config from shipment", ref cmbModel);
            //update.getComboBoxData("select distinct config from shipmentld4", ref cmbModel_LD);
            update.getComboBoxData("select distinct config from shipmentls", ref cmbModel_LS);
            //update.getComboBoxData("select distinct config from shipmentlaa", ref cmbModel_LAA);
        }

        private void defineDatatable(ref DataTable dt)
        {
            dt.Columns.Add("Serial", Type.GetType("System.String"));
            dt.Columns.Add("Config", Type.GetType("System.String"));
            dt.Columns.Add("ShipDate", Type.GetType("System.String"));
            dt.Columns.Add("Status", Type.GetType("System.String"));
        }

        #region Shipment Record
        private void btnShipment_Click(object sender, EventArgs e)
        {
            grbShipment.Visible = true;
            grbDocument.Visible = false;
        }

        #region KK03
        private void btnKK03_Click(object sender, EventArgs e)
        {
            grbKK03.Visible = true;
            grbLD.Visible = false;
            grbLS.Visible = false;
            grbLAA10.Visible = false;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.ShowDialog();
            string path = o1.FileName;
            txtPath.Text = path;

            try
            {
                dgvKK03.DataSource = TfImport.LoadUserListFromExcelFile(txtPath.Text);

                for (int i = 0; i < dgvKK03.Rows.Count; i++)
                    dgvKK03.Rows[i].HeaderCell.Value = (i + 1).ToString();

                dgvKK03.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                dgvKK03.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                foreach (DataGridViewColumn column in dgvKK03.Columns)
                {
                    dgvKK03.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            dtKK03 = new DataTable();
            defineDatatable(ref dtKK03);
            foreach (DataGridViewRow row in dgvKK03.Rows)
            {
                DataRow dtrow = dtKK03.NewRow();
                dtrow[0] = row.Cells[0].Value.ToString();
                dtrow[1] = row.Cells[1].Value.ToString();
                dtrow[2] = row.Cells[2].Value.ToString();
                dtrow[3] = row.Cells[3].Value.ToString();
                dtKK03.Rows.Add(dtrow);
            }
            TfSQL tf = new TfSQL();
            tf.sqlMultipleInsertOverall(dtKK03, "shipment");
        }

        private void btnShipHistory_Click(object sender, EventArgs e)
        {
            frmHistory frmH = new frmHistory();
            frmH.ShowDialog();
        }

        private void rdbReload_CheckedChanged(object sender, EventArgs e)
        {
            dtpShipDate.Enabled = true;
            lblShipDate.Enabled = true;
            lblSubModel.Enabled = true;
            cmbModel.Enabled = true;
            btnSearch.Enabled = true;
            btnReUp.Enabled = true;
            btnDelete.Enabled = true;

            btnUpload.Enabled = false;
            btnShipHistory.Enabled = false;
        }

        private void rdbUpload_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUpload.Checked == true)
            {
                dtpShipDate.Enabled = false;
                lblShipDate.Enabled = false;
                lblSubModel.Enabled = false;
                cmbModel.Enabled = false;
                btnSearch.Enabled = false;
                btnReUp.Enabled = false;
                btnDelete.Enabled = false;

                btnUpload.Enabled = true;
                btnShipHistory.Enabled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtKK03 = new DataTable();
            defineDatatable(ref dtKK03);

            update.sqlDataAdapterFillDatatable("select serno as Serial, config, ship_date as ShipDate, status from shipmentls where ship_date = '" + dtpShipDate.Value.ToShortDateString() + "' and config = '" + cmbModel.Text + "'", ref dtKK03);
            dgvKK03.DataSource = dtKK03;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Vòng lặp chọn những phần tử đang chọn trong gridview
            for (int i = dgvKK03.SelectedRows.Count - 1; i >= 0; i--)
            {
                //Lấy dòng chọn trong gridview
                string serial = dgvKK03.SelectedRows[i].Cells[0].Value.ToString();

                //Thức hiện xóa
                string sqlDelete = "Delete from shipmentls where serno ='" + serial + "'";
                update.sqlExecuteScalarString(sqlDelete);
            }
            btnSearch.PerformClick();
        }
        #endregion

        #region LD4 & LD25
        private void btnLD_Click(object sender, EventArgs e)
        {
            grbKK03.Visible = false;
            grbLD.Visible = true;
            grbLS.Visible = false;
            grbLAA10.Visible = false;
        }

        private void btnBrowser_LD_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.ShowDialog();
            string path = o1.FileName;
            txtPath_LD.Text = path;

            try
            {
                dgvLD.DataSource = TfImport.LoadUserListFromExcelFile(txtPath_LD.Text);

                for (int i = 0; i < dgvLD.Rows.Count; i++)
                    dgvLD.Rows[i].HeaderCell.Value = (i + 1).ToString();

                dgvLD.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                dgvLD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                foreach (DataGridViewColumn column in dgvLD.Columns)
                {
                    dgvLD.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpload_LD_Click(object sender, EventArgs e)
        {
            dtLD = new DataTable();
            defineDatatable(ref dtLD);
            foreach (DataGridViewRow row in dgvLD.Rows)
            {

                DataRow dtrow = dtLD.NewRow();
                dtrow[0] = row.Cells[0].Value.ToString();
                dtrow[1] = row.Cells[1].Value.ToString();
                dtrow[2] = row.Cells[2].Value.ToString();
                dtrow[3] = row.Cells[3].Value.ToString();
                dtLD.Rows.Add(dtrow);
            }
            TfSQL tf = new TfSQL();
            tf.sqlMultipleInsertOverall(dtLD, "shipmentld4");
        }

        private void btnShipHistory_LD_Click(object sender, EventArgs e)
        {
            frmHistory_LD frmH_LD = new frmHistory_LD();
            frmH_LD.ShowDialog();
        }

        private void rdbReload_LD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbReload_LD.Checked == true)
            {
                dtpShipDate_LD.Enabled = true;
                lblShipDate_LD.Enabled = true;
                lblSubModel_LD.Enabled = true;
                cmbModel_LD.Enabled = true;
                btnSearch_LD.Enabled = true;
                btnReUp_LD.Enabled = true;
                btnDelete_LD.Enabled = true;

                btnUpload_LD.Enabled = false;
                btnShipHistory_LD.Enabled = false;
            }
        }

        private void rdbUpload_LD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUpload_LD.Checked == true)
            {
                dtpShipDate_LD.Enabled = false;
                lblShipDate_LD.Enabled = false;
                lblSubModel_LD.Enabled = false;
                cmbModel_LD.Enabled = false;
                btnSearch_LD.Enabled = false;
                btnReUp_LD.Enabled = false;
                btnDelete_LD.Enabled = false;

                btnUpload_LD.Enabled = true;
                btnShipHistory_LD.Enabled = true;
            }
        }

        private void btnSearch_LD_Click(object sender, EventArgs e)
        {
            dtLD = new DataTable();
            defineDatatable(ref dtLS);

            update.sqlDataAdapterFillDatatable("select serno as Serial, config, ship_date as ShipDate, status from shipmentls where ship_date = '" + dtpShipDate_LD.Value.ToShortDateString() + "' and config = '" + cmbModel_LD.Text + "'", ref dtLD);
            dgvLD.DataSource = dtLD;
        }

        private void btnDelete_LD_Click(object sender, EventArgs e)
        {
            //Vòng lặp chọn những phần tử đang chọn trong gridview
            for (int i = dgvLD.SelectedRows.Count - 1; i >= 0; i--)
            {
                //Lấy dòng chọn trong gridview
                string serial = dgvLD.SelectedRows[i].Cells[0].Value.ToString();

                //Thức hiện xóa
                string sqlDelete = "Delete from shipmentls where serno ='" + serial + "'";
                update.sqlExecuteScalarString(sqlDelete);
            }
            btnSearch_LD.PerformClick();
        }
        #endregion

        #region LS12
        private void btnLS_Click(object sender, EventArgs e)
        {
            grbKK03.Visible = false;
            grbLD.Visible = false;
            grbLAA10.Visible = false;
            grbLS.Visible = true;
        }

        private void btnBrowser_LS_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.ShowDialog();
            string path = o1.FileName;
            txtPath_LS.Text = path;

            try
            {
                dgvLS.DataSource = TfImport.LoadUserListFromExcelFile(txtPath_LS.Text);

                for (int i = 0; i < dgvLS.Rows.Count; i++)
                    dgvLS.Rows[i].HeaderCell.Value = (i + 1).ToString();

                dgvLS.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                dgvLS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                foreach (DataGridViewColumn column in dgvLS.Columns)
                {
                    dgvLS.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpload_LS_Click(object sender, EventArgs e)
        {
            dtLS = new DataTable();
            defineDatatable(ref dtLS);
            foreach (DataGridViewRow row in dgvLS.Rows)
            {
                DataRow dtrow = dtLS.NewRow();
                dtrow[0] = row.Cells[0].Value.ToString();
                dtrow[1] = row.Cells[1].Value.ToString();
                dtrow[2] = DateTime.Parse(row.Cells[2].Value.ToString());
                dtrow[3] = row.Cells[3].Value.ToString();
                dtLS.Rows.Add(dtrow);
            }
            TfSQL tf = new TfSQL();
            tf.sqlMultipleInsertLS(dtLS, "shipmentls");
        }

        private void btnShipHistory_LS_Click(object sender, EventArgs e)
        {
            frmHistory_LS frmH_LS = new frmHistory_LS();
            frmH_LS.ShowDialog();
        }

        private void rdbReload_LS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbReload_LS.Checked == true)
            {
                dtpShipDate_LS.Enabled = true;
                lblShipDate_LS.Enabled = true;
                lblSubModel_LS.Enabled = true;
                cmbModel_LS.Enabled = true;
                btnSearch_LS.Enabled = true;
                btnReUp_LS.Enabled = true;
                btnDelete_LS.Enabled = true;

                btnUpload_LS.Enabled = false;
                btnShipHistory_LS.Enabled = false;
            }
        }

        private void rdbUpload_LS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUpload_LS.Checked == true)
            {
                dtpShipDate_LS.Enabled = false;
                lblShipDate_LS.Enabled = false;
                lblSubModel_LS.Enabled = false;
                cmbModel_LS.Enabled = false;
                btnSearch_LS.Enabled = false;
                btnReUp_LS.Enabled = false;
                btnDelete_LS.Enabled = false;

                btnUpload_LS.Enabled = true;
                btnShipHistory_LS.Enabled = true;
            }
        }

        private void btnSearch_LS_Click(object sender, EventArgs e)
        {
            dtLS = new DataTable();
            defineDatatable(ref dtLS);

            update.sqlDataAdapterFillDatatable("select serno as Serial, config, ship_date as ShipDate, status from shipmentls where ship_date = '" + dtpShipDate_LS.Value.ToShortDateString() + "' and config = '" + cmbModel_LS.Text + "'", ref dtLS);
            dgvLS.DataSource = dtLS;
        }

        private void btnDelete_LS_Click(object sender, EventArgs e)
        {
            //Vòng lặp chọn những phần tử đang chọn trong gridview
            for (int i = dgvLS.SelectedRows.Count - 1; i >= 0; i--)
            {
                //Lấy dòng chọn trong gridview
                string serial = dgvLS.SelectedRows[i].Cells[0].Value.ToString();

                //Thức hiện xóa
                string sqlDelete = "Delete from shipmentls where serno ='" + serial + "'";
                update.sqlExecuteScalarString(sqlDelete);
            }
            btnSearch_LS.PerformClick();
        }

        #endregion

        #region LAA10
        private void btnLAA10_Click(object sender, EventArgs e)
        {
            grbKK03.Visible = false;
            grbLD.Visible = false;
            grbLAA10.Visible = true;
            grbLS.Visible = false;
        }
        private void btnBrowser_LAA_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.ShowDialog();
            string path = o1.FileName;
            txtPath_LAA.Text = path;

            try
            {
                dgvLAA.DataSource = TfImport.LoadUserListFromExcelFile(txtPath_LAA.Text);

                for (int i = 0; i < dgvLAA.Rows.Count; i++)
                    dgvLAA.Rows[i].HeaderCell.Value = (i + 1).ToString();

                dgvLAA.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                dgvLAA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                foreach (DataGridViewColumn column in dgvLAA.Columns)
                {
                    dgvLAA.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpload_LAA_Click(object sender, EventArgs e)
        {
            dtLAA = new DataTable();
            defineDatatable(ref dtLAA);
            foreach (DataGridViewRow row in dgvLAA.Rows)
            {
                DataRow dtrow = dtLAA.NewRow();
                dtrow[0] = row.Cells[0].Value.ToString();
                dtrow[1] = row.Cells[1].Value.ToString();
                dtrow[2] = row.Cells[2].Value.ToString();
                dtrow[3] = row.Cells[3].Value.ToString();
                dtLAA.Rows.Add(dtrow);
            }
            TfSQL tf = new TfSQL();
            tf.sqlMultipleInsertOverall(dtLAA, "shipmentlaa");
        }

        private void btnShipHistory_LAA_Click(object sender, EventArgs e)
        {
            frmHistory_LAA frmH_LAA = new frmHistory_LAA();
            frmH_LAA.ShowDialog();
        }

        private void rdbReload_LAA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbReload_LAA.Checked == true)
            {
                dtpShipDate_LAA.Enabled = true;
                lblShipDate_LAA.Enabled = true;
                lblSubModel_LAA.Enabled = true;
                cmbModel_LAA.Enabled = true;
                btnSearch_LAA.Enabled = true;
                btnReUp_LAA.Enabled = true;
                btnDelete_LAA.Enabled = true;

                btnUpload_LAA.Enabled = false;
                btnShipHistory_LAA.Enabled = false;
            }
        }

        private void rdbUpload_LAA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUpload_LAA.Checked == true)
            {
                dtpShipDate_LAA.Enabled = false;
                lblShipDate_LAA.Enabled = false;
                lblSubModel_LAA.Enabled = false;
                cmbModel_LAA.Enabled = false;
                btnSearch_LAA.Enabled = false;
                btnReUp_LAA.Enabled = false;
                btnDelete_LAA.Enabled = false;

                btnUpload_LAA.Enabled = true;
                btnShipHistory_LAA.Enabled = true;
            }
        }

        private void btnSearch_LAA_Click(object sender, EventArgs e)
        {
            dtLAA = new DataTable();
            defineDatatable(ref dtLAA);

            update.sqlDataAdapterFillDatatable("select serno as Serial, config, ship_date as ShipDate, status from shipmentls where ship_date = '" + dtpShipDate_LAA.Value.ToShortDateString() + "' and config = '" + cmbModel_LAA.Text + "'", ref dtLAA);
            dgvLS.DataSource = dtLAA;
        }

        private void btnDelete_LAA_Click(object sender, EventArgs e)
        {
            //Vòng lặp chọn những phần tử đang chọn trong gridview
            for (int i = dgvLS.SelectedRows.Count - 1; i >= 0; i--)
            {
                //Lấy dòng chọn trong gridview
                string serial = dgvLS.SelectedRows[i].Cells[0].Value.ToString();

                //Thức hiện xóa
                string sqlDelete = "Delete from shipmentls where serno ='" + serial + "'";
                update.sqlExecuteScalarString(sqlDelete);
            }
            btnSearch_LAA.PerformClick();
        }
        #endregion

        #endregion

        #region Document Management
        private void btnDocument_Click(object sender, EventArgs e)
        {
            grbShipment.Visible = false;
            grbDocument.Visible = true;
        }

        #region KK03
        private void btnDocKK03_Click(object sender, EventArgs e)
        {
            model = btnDocKK03.Text;
            frmDocument frmDoc = new frmDocument();
            frmDoc.ShowDialog();
        }
        #endregion

        #region LD4 & LD25
        private void btnDocLD_Click(object sender, EventArgs e)
        {
            model = btnDocLD.Text;
            frmDocument frmDoc = new frmDocument();
            frmDoc.ShowDialog();
        }
        #endregion

        #region LS12
        private void btnDocLS12_Click(object sender, EventArgs e)
        {
            model = btnDocLS12.Text;
            frmDocument frmDoc = new frmDocument();
            frmDoc.ShowDialog();
        }
        #endregion

        #region LAA10
        private void btnDocLAA10_Click(object sender, EventArgs e)
        {
            model = btnDocLAA10.Text;
            frmDocument frmDoc = new frmDocument();
            frmDoc.ShowDialog();
        }
        #endregion

        #region MOTOR
        private void btnMotor_Click(object sender, EventArgs e)
        {
            model = btnMotor.Text;
            frmDocument frmDoc = new frmDocument();
            frmDoc.ShowDialog();
        }

        #endregion

        #endregion
    }
}