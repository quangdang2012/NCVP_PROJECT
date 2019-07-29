using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Permissions;
using Npgsql;

namespace BoxIdDb
{
    public partial class frmBoxid : Form
    {
        // The delegate variable to signal the occurrance of delegate event to the parent form "formLogin"
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // Button variable on datagridview
        DataGridViewButtonColumn openBoxId;
        DataGridViewButtonColumn editShipDate;

        // Constructor
        public frmBoxid()
        {
            InitializeComponent();
        }

        // Load event
        private void frmBoxid_Load(object sender, EventArgs e)
        {
            // Set the position in user screen where this form appears
            this.Left = 20;
            this.Top = 20;
            updateDataGripViews(ref dgvBoxId, true);

            if (txtUser.Text == "User_9")
            {
                btnEditShipping.Enabled = true;
                txtBoxIdTo.Enabled = true;
            }
            else
            {
                btnEditShipping.Enabled = false;
                txtBoxIdTo.Enabled = false;
            }
        }

        // Sub procedure: Get the user information from parent form
        public void updateControls(string user)
        {
            txtUser.Text = user;
        }

        // Sub procedure: Define datatable
        private void defineAndReadDatatable(ref DataTable dt)
        {
            dt.Columns.Add("boxid", Type.GetType("System.String"));
            dt.Columns.Add("fact", Type.GetType("System.String"));
            dt.Columns.Add("suser", Type.GetType("System.String"));
            dt.Columns.Add("printdate", Type.GetType("System.DateTime"));
            dt.Columns.Add("shipdate", Type.GetType("System.DateTime"));
        }


        // Sub procedure: Update datagridview
        public void updateDataGripViews(ref DataGridView dgv, bool load)
        {
            string boxId = txtBoxIdFrom.Text;
            DateTime printDate = dtpPrintDate.Value;
            DateTime shipDate = dtpShipDate.Value;
            string buff = txtProductSerial.Text;
            string serialNo = (buff.IndexOf("+") == -1? buff : VBStrings.Left(buff, buff.IndexOf("+")-1));
            string sql = String.Empty;

            // Store the sql query result into datatable
            ShSQL tf = new ShSQL();
            if (rdbBoxId.Checked)
            {
                sql = "select boxid, fact, suser, printdate, shipdate FROM box_id_fct" +
                    (boxId == String.Empty ? String.Empty : " WHERE boxid='" + boxId + "'");
            }
            else if(rdbPrintDate.Checked)
            {
                sql = "select boxid, fact, suser, printdate, shipdate FROM box_id_fct WHERE printdate " +
                    "BETWEEN '" + printDate.Date + "' AND '" + printDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
            }
            else if (rdbProductSerial.Checked)
            {
                sql = "select boxid FROM product_serial WHERE serialno='" + serialNo + "'";
                boxId = tf.sqlExecuteScalarString(sql);
                txtBoxIdFrom.Text = boxId;
                sql = "select boxid, fact, suser, printdate, shipdate FROM box_id_fct" +
                    (boxId == String.Empty ? String.Empty : " WHERE boxid='" + boxId + "'");
            }
            else if (dtpShipDate.Checked)
            {
                sql = "select boxid, fact, suser, printdate, shipdate FROM box_id_fct WHERE shipdate " +
                    "BETWEEN '" + shipDate.Date + "' AND '" + shipDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
            }
            DataTable dt1 = new DataTable();
            tf.sqlDataAdapterFillDatatable(sql, ref dt1);

            // Bind the datatable data into datagridview
            dgv.DataSource = dt1;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dgv.Columns["boxid"].Visible = false;
            // Add button to the datagridview, only when loading the form (only during first time update)
            if (load) addButtonsToDataGridView(dgv);

            // Set row number in the row header
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            // Adjust the width of the row header
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            // show the botton of the datagridview
            if (dgv.Rows.Count != 0)
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;

            // show barcode graphic in the pannel (not a meaningful function, better delete)
            pnlBarcode.Refresh();
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

            // Set SHIPPING button only for the super user
            if (txtUser.Text == "User_9") 
            {
                editShipDate = new DataGridViewButtonColumn();
                editShipDate.HeaderText = "Ship";
                editShipDate.Text = "Ship";
                editShipDate.UseColumnTextForButtonValue = true;
                editShipDate.Width = 80;
                dgv.Columns.Add(editShipDate);
            }
        }

        // OPEN button generate frmModule by view mode without delegate event.
        // SHIP button edit the shipping date for box id.
        private void dgvBoxId_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());

            // OPEN button generate frmModule by view mode without delegate event
            if (dgvBoxId.Columns[e.ColumnIndex] == openBoxId && currentRow >= 0)
            {
                // In case frmModule is already opened, close it first
                ShGeneral.closeOpenForm("frmModule");

                string boxId = dgvBoxId["boxid", currentRow].Value.ToString();
                DateTime printDate = DateTime.Parse(dgvBoxId["printdate", currentRow].Value.ToString());
                string user = txtUser.Text == "User_9" ? txtUser.Text : dgvBoxId["suser", currentRow].Value.ToString();
                string serialNo = txtProductSerial.Text;
                
                frmModule fM = new frmModule();
                fM.updateControls(boxId, printDate, user, serialNo, false);
                fM.Show();
            }

            // SHIP button edit the shipping date for box id
            if (dgvBoxId.Columns[e.ColumnIndex] == editShipDate && currentRow >= 0)
            {
                string boxId = dgvBoxId["boxid", currentRow].Value.ToString();
                DateTime shipdate = dtpShipDate.Value;

                DialogResult result1 = MessageBox.Show("Do you want to update the shipping date of as follows:" + System.Environment.NewLine +
                    boxId + ": " + shipdate, "Notice",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result1 == DialogResult.Yes)
                {
                    string sql = "update box_id SET shipdate ='" + shipdate + "' " +
                        "WHERE boxid= '" + boxId + "'";
                    System.Diagnostics.Debug.Print(sql);
                    ShSQL tf = new ShSQL();
                    int res = tf.sqlExecuteNonQueryInt(sql, false);
                    updateDataGripViews(ref dgvBoxId, false);
                }
            }
        }

        // When SEARCH button is clicked, run "update gridview" function, which includes sql command
        private void btnSearchBoxId_Click(object sender, EventArgs e)
        {
            updateDataGripViews(ref dgvBoxId, false);
        }

        // When ADDBOXID button is clicked, open frmModule by edit mode with delegate function
        private void btnAddBoxId_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text;

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
                fM.RefreshEvent += delegate(object sndr, EventArgs excp) 
                {
                    updateDataGripViews(ref dgvBoxId, false);
                    this.Focus(); 
                };

                fM.updateControls(String.Empty, DateTime.Now, user, String.Empty, true);
                fM.Show();            
            }
        }

        // When SHIP button is clicked, register the shipping date to multiple box ids
        private void btnEditShipping_Click(object sender, EventArgs e)
        {
            string idFrom = txtBoxIdFrom.Text;
            string idTo = txtBoxIdTo.Text;
            DateTime shipdate = dtpShipDate.Value;

            if (idFrom == String.Empty || idTo == String.Empty)
            {
                MessageBox.Show("Both box-id-from and box-id-to, plus ship date have to be selected.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                DialogResult result1 = MessageBox.Show("Have you slected box-id-from, box-id-to, and shipdate correctly?", "Notice",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
             
                if (result1 == DialogResult.Yes)
                {
                    DialogResult result2 = MessageBox.Show("Are you really sure to update the ship date?", "Notice",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result2 == DialogResult.Yes)
                    {
                        string sql = "update box_id SET shipdate = '" + shipdate + "' " +
                            "WHERE boxid BETWEEN '" + idFrom + "' AND '" + idTo + "'";
                            ShSQL tf = new ShSQL();
                            int res = tf.sqlExecuteNonQueryInt(sql, false);
                            MessageBox.Show(res + " records were updated", "Notice",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                            rdbShipDate.Checked = true;
                            updateDataGripViews(ref dgvBoxId, false);
                    }
                }
            }
        }

        // Sub procedure: Show barcode graphics to the decoration pannel
        private void pnlBarcode_Paint(object sender, PaintEventArgs e)
        {
            DotNetBarcode barCode = new DotNetBarcode();
            string barcodeNumber;
            Single x1;
            Single y1;
            Single x2;
            Single y2;
            x1 = 0;
            y1 = 0;
            x2 = pnlBarcode.Size.Width;
            y2 = pnlBarcode.Size.Height;
            barcodeNumber = txtBoxIdFrom.Text;
            barCode.Type = DotNetBarcode.Types.Jan13;

            if (barcodeNumber != String.Empty)
                barCode.WriteBar(barcodeNumber, x1, y1, x2, y2, e.Graphics);
        }

        // When closing this frmBoxid form (child), close formLogin (parent)
        private void frmBoxid_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Send signal to the parent form formLogin to induce delegate event
            this.RefreshEvent(this, new EventArgs());
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

        // Even of ratio button change (rdbBoxId) : Clear text box
        private void rdbBoxId_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBoxId.Checked) { txtProductSerial.Text = String.Empty; }
        }
        // Even of ratio button change (rdbPrintDate) : Clear text box
        private void rdbPrintDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPrintDate.Checked)
            {
                txtBoxIdFrom.Text = String.Empty;
                txtBoxIdTo.Text = String.Empty;
                txtProductSerial.Text = String.Empty;
            }
        }
        // Even of ratio button change (rdbProductSerial) : Clear text box
        private void rdbProductSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbProductSerial.Checked)
            {
                txtBoxIdFrom.Text = String.Empty;
                txtBoxIdTo.Text = String.Empty;
            }
        }
        // Even of ratio button change (rdbShipDate) : Clear text box
        private void rdbShipDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbShipDate.Checked)
            {
                txtBoxIdFrom.Text = String.Empty;
                txtBoxIdTo.Text = String.Empty;
                txtProductSerial.Text = String.Empty;
            }
        }
    }
}