using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace JigQuick
{
    public partial class frmOmni : Form
    {
        public frmOmni()
        {
            InitializeComponent();
        }
        TfSQL tf = new TfSQL();

        private void frmOmni_Load(object sender, EventArgs e)
        {
            //txt_barcode.SelectNextControl(txt_barcode, true, false, true, true);
            string standByImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\STANDBY.bmp";
            pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
            pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
            pnlThurst.BackgroundImage = System.Drawing.Image.FromFile(standByImagePath);
            pnlNoise.BackgroundImage = System.Drawing.Image.FromFile(standByImagePath);
        }

        int count = 0;
        string line;
        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (txt_barcode.Text == string.Empty) return;

            if (txt_barcode.ReadOnly == true) return;


            //Check Thurst, Noise, TestTime
            bool res1 = checkTestTimes(txt_barcode.Text);
            if (!res1) return;

            bool res = checkThurstNoise(txt_barcode.Text);
            if (!res) return;

            //Output
            string ser = tf.sqlExecuteScalarString("SELECT barcode FROM t_serno WHERE barcode = '" + txt_barcode.Text + "'");

            bool dup = checkDuplicate();

            if (dup)
            {
                MessageBox.Show("Duplicate barcode!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            if (ser != txt_barcode.Text)
            {
                tf.sqlExecuteScalarString("insert into t_serno(barcode, model, regist_date, user_cd, line) values('" + txt_barcode.Text + "','" + lblModel.Text + "', now(), 'GA1FINAL', '" + line + "')");

                count = count + 1;
                lblCounter.Text = count.ToString();
            }
        }

        private bool checkDuplicate()
        {
            string checkI = tf.sqlExecuteScalarString("Select Count(serialno) from t_product_serial Where 1=1 and serialno = '" + txt_barcode.Text + "'");

            if (int.Parse(checkI) > 0)
            {
                txt_barcode.ReadOnly = true;
                txt_barcode.BackColor = Color.Red;
                return true;
            }
            else return false;
        }
        private bool checkThurstNoise(string id)
        {
            string okImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\OK_BEAR.png";
            string noImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\NODATA.png";
            string ngImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\NG_BEAR.png";

            //Show Noise MC
            lblNoiseMC.Text = tf.sqlExecuteScalarString("select eq_id from t_noisecheck_a90 where barcode = '" + txt_barcode.Text + "' order by date_check desc limit 1");

            string scanTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            DataTable dt1 = new DataTable();
            tf.sqlDataAdapterFillDatatableFromTesterDb("select a90_barcode, a90_thurst_status, a90_noise_status, a90_oqc_data, a90_line from t_checkpusha90 where a90_barcode = '" + txt_barcode.Text + "' order by oid desc limit 1", ref dt1);

            bool result = false;

            if (dt1.Rows.Count > 0)
            {
                line = dt1.Rows[0]["a90_line"].ToString();
                //Check Thurst
                switch (dt1.Rows[0]["a90_thurst_status"].ToString())
                {
                    case "OK":
                        pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlThurst.BackgroundImage = Image.FromFile(okImagePath);
                        //checkDuplicate();

                        result = true;

                        txt_barcode.SelectAll();
                        break;
                    case "NG":
                        pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlThurst.BackgroundImage = Image.FromFile(ngImagePath);
                        //checkDuplicate();

                        result = false;//Đẳng sửa true -> false

                        txt_barcode.ReadOnly = true;
                        txt_barcode.BackColor = Color.Red;
                        break;
                    default:
                        pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlThurst.BackgroundImage = Image.FromFile(noImagePath);

                        result = true;

                        txt_barcode.ReadOnly = false;//Đẳng sửa true -> false
                        txt_barcode.BackColor = Color.Red;
                        break;
                }

                if (!result) { return false; } //Đẳng add

                //Check Noise
                switch (dt1.Rows[0]["a90_noise_status"].ToString())
                {
                    case "OK":
                        pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlNoise.BackgroundImage = Image.FromFile(okImagePath);
                        //checkDuplicate();

                        result = true;

                        txt_barcode.SelectAll();
                        break;
                    case "NG":
                        pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlNoise.BackgroundImage = Image.FromFile(ngImagePath);
                        //checkDuplicate();

                        result = false;//Đẳng sửa true -> false

                        txt_barcode.ReadOnly = true;
                        txt_barcode.BackColor = Color.Red;
                        break;
                    default:
                        pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlNoise.BackgroundImage = Image.FromFile(noImagePath);

                        result = false;//Đẳng sửa true -> false

                        txt_barcode.ReadOnly = true;
                        txt_barcode.BackColor = Color.Red;
                        break;
                }
                if (!result) { return false; } //Đẳng add

                //Check OQC_Data Đẳng add
                //switch (dt1.Rows[0]["a90_oqc_data"].ToString())
                //{
                //    case "NG":
                //        pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                //        pnlThurst.BackgroundImage = Image.FromFile(ngImagePath);
                //        //checkDuplicate();

                //        result = false;//Đẳng sửa true -> false

                //        txt_barcode.ReadOnly = true;
                //        txt_barcode.BackColor = Color.Red;
                //        break;
                //    default:
                //        pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                //        pnlThurst.BackgroundImage = Image.FromFile(okImagePath);

                //        result = true;
                //        break;
                //}
                //if (!result) { return false; } //Đẳng add
            }
            else
            {
                pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
                pnlThurst.BackgroundImage = Properties.Resources.NODATA;
                pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
                pnlNoise.BackgroundImage = Properties.Resources.NODATA;
                txt_barcode.ReadOnly = true;
                txt_barcode.BackColor = Color.Red;
                result = false;
            }
            return result;
        }

        private bool checkTestTimes(string serial)
        {
            DataTable dt2 = new DataTable();
            tf.sqlDataAdapterFillDatatableFromTesterDb("select a90_barcode from t_checkpusha90 where a90_barcode = '" + txt_barcode.Text + "'", ref dt2);

            lblTestTime.Text = "Test Times Thurst: " + dt2.Rows.Count;

            bool kq = false;
            if (dt2.Rows.Count > 20)
            {
                lblTestTime.BackColor = Color.Red;
                txt_barcode.BackColor = Color.Red;
                txt_barcode.ReadOnly = true;
                kq = false;
            }
            else
            {
                lblTestTime.BackColor = Color.LightGreen;
                kq = true;
            }
            return kq;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string standByImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\JigQuickDesk\JigQuickApp\images\STANDBY.bmp";
            pnlThurst.BackgroundImageLayout = ImageLayout.Zoom;
            pnlNoise.BackgroundImageLayout = ImageLayout.Zoom;
            pnlThurst.BackgroundImage = Image.FromFile(standByImagePath);
            pnlNoise.BackgroundImage = Image.FromFile(standByImagePath);

            lblNoiseMC.ResetText();
            lblTestTime.ResetText();
            txt_barcode.ResetText();
            txt_barcode.ReadOnly = false;
            txt_barcode.BackColor = Color.White;
            txt_barcode.Focus();
        }
    }
}