using System;
using System.Data;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace BoxIdDb
{
    public partial class frmBoxid : Form
    {
        // The delegate variable to signal the occurrance of delegate event to the parent form "formLogin"
        public delegate void RefreshEventHandler(object sender, EventArgs e);

        // Button variable on datagridview
        DataGridViewButtonColumn openBoxId;
        DataGridViewButtonColumn editShipDate;
        DataGridViewButtonColumn addInvoice;

        CheckBox ckbShipDate;

        // Constructor
        public frmBoxid()
        {
            InitializeComponent();
            dgvBoxId.AutoGenerateColumns = false;
        }

        // Load event
        private void frmBoxid_Load(object sender, EventArgs e)
        {
            dgvBoxId.Columns[1].ReadOnly = true;
            dgvBoxId.Columns[2].ReadOnly = true;
            dgvBoxId.Columns[3].ReadOnly = true;
            dgvBoxId.Columns[4].ReadOnly = true;
            dgvBoxId.Columns[5].ReadOnly = true;

            // Set the position in user screen where this form appears
            this.Left = 20;
            this.Top = 20;
            selectdata();
            addButtonsToDataGridView(dgvBoxId);
            dgvBoxId.AutoGenerateColumns = false;

            ckbShipDate = new CheckBox();
            //Get the column header cell bounds
            Rectangle rect1 = this.dgvBoxId.GetCellDisplayRectangle(0, -1, true);
            ckbShipDate.Size = new Size(14, 14);
            //Change the location of the CheckBox to make it stay on the header
            ckbShipDate.Location = rect1.Location;
            ckbShipDate.CheckedChanged += new EventHandler(ckbShipDate_CheckedChanged);
            //Add the CheckBox into the DataGridView
            this.dgvBoxId.Controls.Add(ckbShipDate);
        }

        private void ckbShipDate_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvBoxId.RowCount; i++)
            {
                if (ckbShipDate.Checked)
                {
                    if (dgvBoxId["shipdate", i].Value.ToString() == "")
                    {
                        dgvBoxId["shipchk", i].Value = true;
                    }
                }
                else
                {
                    dgvBoxId["shipchk", i].Value = false;
                }
            }
        }
        // Sub procedure: Define datatable
        private void defineAndReadDatatable(ref DataTable dt)
        {
            dt.Columns.Add("boxid", Type.GetType("System.String"));
            dt.Columns.Add("child_model", Type.GetType("System.String"));
            dt.Columns.Add("user", Type.GetType("System.String"));
            dt.Columns.Add("printdate", Type.GetType("System.DateTime"));
            dt.Columns.Add("shipdate", Type.GetType("System.DateTime"));
        }

        // Sub procedure: Update datagridview
        public void selectdata()
        {
            string sql = String.Empty;
            DateTime printdate = DateTime.Today;
            DateTime nextdate = printdate.AddDays(1);
            // Store the sql query result into datatable
            ShSQL tf = new ShSQL();
            sql = "select boxid, child_model, printdate, user_cd as user, shipdate  from t_box_id WHERE printdate >= '" + printdate + "' AND printdate < '" + nextdate + "' order by boxid";
            DataTable dt1 = new DataTable();
            tf.sqlDataAdapterFillDatatable(sql, ref dt1);
            dgvBoxId.DataSource = dt1;
        }

        // Sub procedure: Add button to datagridview
        private void addButtonsToDataGridView(DataGridView dgv)
        {
            // Set OPEN button for every user
            openBoxId = new DataGridViewButtonColumn();
            openBoxId.HeaderText = "Open";
            openBoxId.Text = "Open";
            openBoxId.UseColumnTextForButtonValue = true;
            openBoxId.Width = 80;
            dgv.Columns.Add(openBoxId);

            // Set SHIPPING button for every user
            //editShipDate = new DataGridViewButtonColumn();
            //editShipDate.HeaderText = "Ship";
            //editShipDate.Text = "Ship";
            //editShipDate.UseColumnTextForButtonValue = true;
            //editShipDate.Width = 80;
            //dgv.Columns.Add(editShipDate);

            // Set INVOICE button for every user
            addInvoice = new DataGridViewButtonColumn();
            addInvoice.HeaderText = "Invoice";
            addInvoice.Text = "Invoice";
            addInvoice.UseColumnTextForButtonValue = true;
            addInvoice.Width = 100;
            dgv.Columns.Add(addInvoice);
        }

        // OPEN button generate frmModule by view mode without delegate event.
        // SHIP button edit the shipping date for box id.
        private void dgvBoxId_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ShSQL tf = new ShSQL();
            int currentRow = int.Parse(e.RowIndex.ToString());
            string boxId = dgvBoxId["boxid", currentRow].Value.ToString();

            // OPEN button generate frmModule by view mode without delegate event
            if (dgvBoxId.Columns[e.ColumnIndex] == openBoxId && currentRow >= 0)
            {
                // In case frmModule is already opened, close it first
                ShGeneral.closeOpenForm("frmModule");

                //string boxId = dgvBoxId["boxid", currentRow].Value.ToString();
                DateTime printDate = DateTime.Parse(dgvBoxId["printdate", currentRow].Value.ToString());
                string user = dgvBoxId["user", currentRow].Value.ToString();

                frmModule fM = new frmModule();
                fM.updateControls(boxId, printDate, user, false);
                fM.Show();
            }

            // INVOICE button edit the Invoice no for box id
            if (dgvBoxId.Columns[e.ColumnIndex] == addInvoice && currentRow >= 0)
            {
                // string boxId = dgvBoxId["boxid", currentRow].Value.ToString();
                string invoice = txtInvoice.Text;

                //DialogResult result1 = MessageBox.Show("Do you want to update the invoice number of as follows:" + System.Environment.NewLine +
                //    boxId + ": " + invoice, "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (!String.IsNullOrEmpty(txtInvoice.Text))
                {
                    int count = tf.sqlExecuteScalarInt("select count(boxid) from t_shipment_record where boxid = '" + boxId + "'");
                    if (count > 0)
                    {
                        tf.sqlExecuteScalarString("UPDATE t_shipment_record SET invoice = '" + invoice + "' WHERE boxid = '" + boxId + "'");
                    }
                    btnSearchBoxId.PerformClick();
                }
                else
                {
                    MessageBox.Show("Please input Invoice number and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtInvoice.Focus();
                }
            }
        }

        // When SEARCH button is clicked, run "update gridview" function, which includes sql command
        private void btnSearchBoxId_Click(object sender, EventArgs e)
        {
            DateTime shipdate = dtpShipDate.Value.Date;
            DateTime printdate = DateTime.Parse(dtpPrintDate.Value.ToShortDateString());
            ShSQL tf = new ShSQL();
            DataTable dt2 = new DataTable();
            if (rdbPrintDate.Checked == true)
            {
                tf.sqlDataAdapterFillDatatable("select boxid, printdate, user_cd as user, shipdate, child_model from t_box_id WHERE printdate >= '" + printdate.ToString("yyyy/MM/dd") + " 00:00:00' and printdate < '" + printdate.ToString("yyyy/MM/dd") + " 23:59:59' order by boxid asc", ref dt2);
            }
            if (rdbShipDate.Checked == true)
            {
                tf.sqlDataAdapterFillDatatable("select boxid, printdate, user_cd as user, shipdate, child_model from t_box_id WHERE shipdate = '" + shipdate.ToString("yyyy/MM/dd") + "' order by boxid asc", ref dt2);
            }
            dgvBoxId.DataSource = dt2;
        }

        // When ADDBOXID button is clicked, open frmModule by edit mode with delegate function
        private void btnAddBoxId_Click(object sender, EventArgs e)
        {
            bool bl = ShGeneral.checkOpenFormExists("frmModule");
            if (bl)
            {
                MessageBox.Show("Please close brows-mode form or finish the current edit form.", "BoxId DB",
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            }
            else
            {
                frmModule fM = new frmModule();
                // Catch the child (frmModule) event, then update this form's datagridview
                fM.RefreshEvent += delegate (object sndr, EventArgs excp)
                {
                    selectdata();
                    this.Focus();
                };

                fM.updateControls(String.Empty, DateTime.Now, String.Empty, true);
                fM.Show();
            }
        }

        // Do not allow user to close this form by right top close button or by Alt+F4 shor cut
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }

        // Close this form only when child from frmModule is not open
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string formName = "frmModule";
            bool bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl)
            {
                MessageBox.Show("You need to close Form Product Serial first.", "BoxId DB",
                  MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }
            Close();
        }

        private void btnShipHistory_Click(object sender, EventArgs e)
        {
            new frmHistory().ShowDialog();
        }

        private void btnShip_Click(object sender, EventArgs e)
        {
            // SHIP button edit the shipping date for box id
            ShSQL tf = new ShSQL();
            DateTime shipdate = dtpShipDate.Value;
            string boxId, sql, sql_ship;

            DialogResult result1 = MessageBox.Show("Do you want to update the shipping date of as follows:" + System.Environment.NewLine + shipdate.ToShortDateString(), "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result1 == DialogResult.Yes)
            {
                for (int i = 0; i < dgvBoxId.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvBoxId["shipchk", i].Value))
                    {
                        boxId = dgvBoxId["boxid", i].Value.ToString();
                        sql = "update t_box_id SET shipdate = '" + shipdate.ToShortDateString() + "' WHERE boxid = '" + boxId + "'";
                        int count = tf.sqlExecuteScalarInt("select count(boxid) from t_shipment_record where boxid = '" + boxId + "'");
                        if (count > 0)
                        {
                            tf.sqlExecuteScalarString("UPDATE t_shipment_record SET ship_date = '" + shipdate.ToShortDateString() + "' WHERE boxid = '" + boxId + "'");
                            tf.sqlExecuteScalarString(sql);
                            btnSearchBoxId.PerformClick();
                            continue;
                        }

                        sql_ship = "INSERT INTO t_shipment_record SELECT boxid, serialno, model, '" + shipdate.ToShortDateString() + "', '" + txtShipStatus.Text + "', lot, substring(lot,1,4), '" + txtShipto.Text + "' FROM t_product_serial WHERE boxid = '" + boxId + "'";
                        if (!String.IsNullOrEmpty(txtShipStatus.Text))
                        {
                            int res = tf.sqlExecuteNonQueryInt(sql, false);
                            tf.sqlExecuteScalarString(sql_ship);
                        }
                        else
                        {
                            MessageBox.Show("Please input Shipping Status and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtShipStatus.Focus();
                            continue;
                        }
                    }
                }
                btnSearchBoxId.PerformClick();
            }
        }

        private void btnRTV_Click(object sender, EventArgs e)
        {
            new frmRTV().ShowDialog();
        }
    }
}