using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QA_Management
{
    public partial class frmDocument : Form
    {
        TfSQL sq = new TfSQL();
        DataTable doc_dt, doc_dt_s, dt_m;
        DataGridViewButtonColumn Open;
        public string sql;

        public frmDocument()
        {
            InitializeComponent();
            initForm();
            this.AcceptButton = btnSearch;
        }

        private void initForm()
        {
            dt_m = new DataTable();
            if (cmbModel.DataSource != null)
            {
                cmbModel.DataSource = null;
            }
            switch (Form1.model)
            {
                case "KK03":
                    sql = "select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where model like '%KK03%' order by doc_id";
                    cmbModel.Text = "KK03";
                    cmbModel.Items.Add("KK03");
                    break;
                case "LD4 - LD25":
                    sql = "select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where model like '%LD%' order by doc_id";
                    sq.sqlDataAdapterFillDatatable("select distinct model from t_document where model like '%LD%'", ref dt_m);
                    cmbModel.DataSource = dt_m;
                    cmbModel.DisplayMember = "model";
                    break;
                case "LS12":
                    sql = "select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where model like '%LS12%' order by doc_id";
                    sq.sqlDataAdapterFillDatatable("select distinct model from document_mgr where model like '%LS12%'", ref dt_m);
                    cmbModel.DataSource = dt_m;
                    cmbModel.DisplayMember = "model";
                    break;
                case "LAA10":
                    sql = "select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where model like '%LAA10%' order by doc_id";
                    sq.sqlDataAdapterFillDatatable("select distinct model from document_mgr where model like '%LAA10%'", ref dt_m);
                    cmbModel.DataSource = dt_m;
                    cmbModel.DisplayMember = "model";
                    break;
                default:
                    sql = "select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where model not like '%LAA10%' and model not like '%KK03%' and model not like '%LD%' and model not like '%LS12%' order by doc_id";
                    sq.sqlDataAdapterFillDatatable("select distinct model from document_mgr where model not like '%LAA10%' and model not like '%KK03%' and model not like '%LD%' and model not like '%LS12%'", ref dt_m);
                    cmbModel.DataSource = dt_m;
                    cmbModel.DisplayMember = "model";
                    break;
            }
           
            doc_dt = new DataTable();

            sq.sqlDataAdapterFillDatatable(sql, ref doc_dt);
            dgvDocument.DataSource = doc_dt;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDocument_Load(object sender, EventArgs e)
        {
            sq.getComboBoxData("select * from folder_list", ref cmbDocType);
            addButtonTodgv(dgvDocument);
        }

        public void addButtonTodgv(DataGridView dgv)
        {
            Open = new DataGridViewButtonColumn();
            Open.Text = "Open";
            Open.UseColumnTextForButtonValue = true;
            Open.Width = 45;
            Open.Visible = true;
            dgv.Columns.Add(Open);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddDocument frmAdd = new frmAddDocument();
            frmAdd.ShowDialog();
            initForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string docName = dgvDocument.CurrentRow.Cells["doc_name"].Value.ToString();
            string docType = dgvDocument.CurrentRow.Cells["doc_type"].Value.ToString();
            string parentPath = @"Z:\(01)KK03\QA\(00)Public\DOCUMENT\" + docType + @"\";
            string id = dgvDocument.CurrentRow.Cells["doc_id"].Value.ToString();

            DialogResult result = MessageBox.Show("Do you really want to delete this document ? This action will be delete the document file also!",
                "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                sq.sqlExecuteScalarString("delete from document_mgr where doc_id = '" + id + "'");
                File.Delete(parentPath + docName);
                MessageBox.Show("This document is deleted!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                initForm();
            }
        }

        public static string docName_, docNo_, docType_, version_, model_;
        public static int docID_;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            docID_ = int.Parse(dgvDocument.CurrentRow.Cells["doc_id"].Value.ToString());
            docName_ = dgvDocument.CurrentRow.Cells["doc_name"].Value.ToString();
            docNo_ = dgvDocument.CurrentRow.Cells["doc_no"].Value.ToString();
            docType_ = dgvDocument.CurrentRow.Cells["doc_type"].Value.ToString();
            version_ = dgvDocument.CurrentRow.Cells["col_version"].Value.ToString();
            model_ = dgvDocument.CurrentRow.Cells["model"].Value.ToString();

            frmUpdateDocument frmUp = new frmUpdateDocument();
            frmUp.ShowDialog();
            initForm();
        }

        private void dgvDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string docName = dgvDocument.CurrentRow.Cells["doc_name"].Value.ToString();
            string docType = dgvDocument.CurrentRow.Cells["doc_type"].Value.ToString();
            string parentPath = @"Z:\(01)KK03\QA\(00)Public\DOCUMENT\" + docType + @"\";

            if (dgvDocument.Columns[e.ColumnIndex] == Open)
            {
                Process prc = new Process();
                try
                {
                    prc.StartInfo.FileName = parentPath + docName;
                    prc.Start();
                }
                catch { }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            doc_dt_s = new DataTable();
            StringBuilder sql_ = new StringBuilder();
            sql_.Append("select doc_id, doc_name, doc_no, version, update_date, doc_type, model from document_mgr where 1 = 1");
            switch (Form1.model)
            {
                case "KK03":
                    sql_.Append(" and model like '%KK03%'");
                    if (!String.IsNullOrEmpty(txtDocName.Text))
                    {
                        sql_.Append(" and doc_name like '%" + txtDocName.Text + "%'");
                    }

                    if (!String.IsNullOrEmpty(cmbDocType.Text))
                    {
                        sql_.Append(" and doc_type = '" + cmbDocType.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(txtDocNo.Text))
                    {
                        sql_.Append(" and doc_no = '" + txtDocNo.Text + "'");
                    }

                    //if (!String.IsNullOrEmpty(cmbModel.Text))
                    //{
                    //    sql_.Append(" and model like '%" + cmbModel.Text + "%'");
                    //}
                    break;
                case "LD4 - LD25":
                    sql_.Append(" and model like '%LD%'");
                    if (!String.IsNullOrEmpty(txtDocName.Text))
                    {
                        sql_.Append(" and doc_name like '%" + txtDocName.Text + "%'");
                    }

                    if (!String.IsNullOrEmpty(cmbDocType.Text))
                    {
                        sql_.Append(" and doc_type = '" + cmbDocType.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(txtDocNo.Text))
                    {
                        sql_.Append(" and doc_no = '" + txtDocNo.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(cmbModel.Text))
                    {
                        sql_.Append(" and model = '" + cmbModel.Text + "'");
                    }
                    break;
                case "LS12":
                    sql_.Append(" and model like '%LS12%'");
                    if (!String.IsNullOrEmpty(txtDocName.Text))
                    {
                        sql_.Append(" and doc_name like '%" + txtDocName.Text + "%'");
                    }

                    if (!String.IsNullOrEmpty(cmbDocType.Text))
                    {
                        sql_.Append(" and doc_type = '" + cmbDocType.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(txtDocNo.Text))
                    {
                        sql_.Append(" and doc_no = '" + txtDocNo.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(cmbModel.Text))
                    {
                        sql_.Append(" and model = '" + cmbModel.Text + "'");
                    }
                    break;
                case "LAA10":
                    sql_.Append(" and model like '%LAA10%'");
                    if (!String.IsNullOrEmpty(txtDocName.Text))
                    {
                        sql_.Append(" and doc_name like '%" + txtDocName.Text + "%'");
                    }

                    if (!String.IsNullOrEmpty(cmbDocType.Text))
                    {
                        sql_.Append(" and doc_type = '" + cmbDocType.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(txtDocNo.Text))
                    {
                        sql_.Append(" and doc_no = '" + txtDocNo.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(cmbModel.Text))
                    {
                        sql_.Append(" and model = '" + cmbModel.Text + "'");
                    }
                    break;
                default:
                    sql_.Append(" and model not like '%LD%' and model not like '%LAA10%' and model not like '%KK03%' and model not like '%LS12%'");
                    if (!String.IsNullOrEmpty(txtDocName.Text))
                    {
                        sql_.Append(" and doc_name like '%" + txtDocName.Text + "%'");
                    }

                    if (!String.IsNullOrEmpty(cmbDocType.Text))
                    {
                        sql_.Append(" and doc_type = '" + cmbDocType.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(txtDocNo.Text))
                    {
                        sql_.Append(" and doc_no = '" + txtDocNo.Text + "'");
                    }

                    if (!String.IsNullOrEmpty(cmbModel.Text))
                    {
                        sql_.Append(" and model = '" + cmbModel.Text + "'");
                    }
                    break;
            }
            

            sql_.Append(" order by doc_id");

            sq.sqlDataAdapterFillDatatable(sql_.ToString(), ref doc_dt_s);
            dgvDocument.DataSource = doc_dt_s;
        }
    }
}
