using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QA_Management
{
    public partial class frmUpdateDocument : Form
    {
        TfSQL ins = new TfSQL();
        public frmUpdateDocument()
        {
            InitializeComponent();
            ins.getComboBoxData("select * from folder_list", ref cmbDocType);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sql_update = "UPDATE document_mgr SET version = '" + txtVersion.Text + "', update_date = '" + DateTime.Today + "' WHERE doc_name = '" + txtDocName.Text + "'";
            ins.sqlExecuteScalarString(sql_update);
            MessageBox.Show("Update successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddDocument_Load(object sender, EventArgs e)
        {
            cmbDocType.Text = frmDocument.docType_;
            txtDocName.Text = frmDocument.docName_;
            txtDocNo.Text = frmDocument.docNo_;
            txtModel.Text = frmDocument.model_;
            txtVersion.Text = frmDocument.version_;

        }
    }
}