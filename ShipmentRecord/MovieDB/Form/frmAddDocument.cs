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
    public partial class frmAddDocument : Form
    {
        TfSQL ins = new TfSQL();
        public frmAddDocument()
        {
            InitializeComponent();
            ins.getComboBoxData("select * from folder_list", ref cmbDocType);
        }
        public string fileName;
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.ShowDialog();
            txtDocName.Text = Path.GetFileName(o1.FileName);
            linksave_txt.Text = o1.FileName;
            fileName = Path.GetFileNameWithoutExtension(o1.FileName);

            string[] docName = fileName.Split('_');
            txtDocNo.Text = docName[2];
            txtModel.Text = docName[0];
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string mcPath = @"Z:\(01)KK03\QA\(00)Public\DOCUMENT\";
            string parentPath = mcPath + cmbDocType.Text + @"\" + txtDocName.Text;

            if (!Directory.Exists(mcPath + cmbDocType.Text))
            {
                Directory.CreateDirectory(mcPath + cmbDocType.Text);
                File.Move(linksave_txt.Text, parentPath);
            }
            else
            {
                File.Move(linksave_txt.Text, parentPath);
            }
            ins.sqlInsertDocument("document_mgr", txtDocName.Text, txtDocNo.Text, cmbDocType.Text, txtVersion.Text, txtModel.Text);
            cmbDocType.ResetText();
            linksave_txt.ResetText();
            txtDocName.ResetText();
            txtDocNo.ResetText();
            txtModel.ResetText();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}