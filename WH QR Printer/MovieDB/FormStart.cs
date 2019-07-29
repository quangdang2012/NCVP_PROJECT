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

namespace WhQrPrinter
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        private void btnNCVP_Click(object sender, EventArgs e)
        {
            Form1 ncvp = new Form1();
            this.Hide();
            ncvp.ShowDialog();
            this.Show();
        }

        private void btnNCVH_Click(object sender, EventArgs e)
        {
            NCVH ncvh = new NCVH();
            this.Hide();
            ncvh.ShowDialog();
            this.Show();
        }

        private void FormStart_Load(object sender, EventArgs e)
        {

        }
    }
}