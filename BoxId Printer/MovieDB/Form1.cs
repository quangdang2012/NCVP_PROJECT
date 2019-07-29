using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace BoxIdPrinter
{
    public partial class Form1 : Form
    {
        // ��{�ݒ�̕ύX�𓖃t�@�C���ōs��
        string config = System.Environment.CurrentDirectory + "\\info.ini";

        // ini�t�@�C���̃^�[�Q�b�g�t�H���_�ݒ��ێ�����ϐ�
        string directory = @"C:\Users\takusuke.fujii\Desktop\Auto Print\"; //��

        // �����p�̃o�[�R�[�h�\���ϐ�
        string barcodeNumber = String.Empty;

        // �R���X�g���N�^
        public Form1()
        {
            InitializeComponent();
        }

        // ���[�h��
        private void Form1_Load(object sender, EventArgs e)
        {
            directory = readIni("TARGET DIRECTORY", "DIR", config);
        }

        // ���C���v���V�[�W���F�^�C�}�[�C�x���g���g�p���A�P�b���Ƃɏ������s��
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // �w��̃t�H���_�����݂��Ȃ��ꍇ�́A���������Ȃ�
            if (!System.IO.Directory.Exists(directory)) return;

            // �w��̃t�H���_���̃t�@�C�������ׂĎ擾
            string[] files = System.IO.Directory.GetFiles(directory, "*", System.IO.SearchOption.AllDirectories);
            // �v�����g�A�E�g���A�t�@�C�����폜
            for (int i = 0; i < files.Length; i++)
            {
                string fname = System.IO.Path.GetFileName(files[i]);
                if (VBStrings.Right(fname.ToLower(), 4) == ".txt")
                {
                    string boxid = VBStrings.Left(fname, fname.Length - 4);
                    // �Q�����
                    TfPrint.printBarCode(boxid);
                    //TfPrint.printBarCode(boxid);
                    if (boxid != String.Empty) barcodeNumber = boxid;
                    pnlBarcode.Refresh();
                    System.IO.File.Delete(files[i]);
                    lblTime.Text = DateTime.Now.ToString();
                }
                else if (VBStrings.Right(fname.ToLower(), 4) == ".bmp")
                {
                    string datecdFile = files[i];
                    // �Q�����
                    TfPrint.printBitmap(datecdFile);
                    //TfPrint.printBitmap(datecdFile);
                    //TfPrint.printBitmap(datecdFile);
                    System.IO.File.Delete(files[i]);
                }
            }
        }
        
        // �T�u�v���V�[�W���F�����p�̃o�[�R�[�h�\���p�l���̍X�V�A���ۂ̏o�͂Ƃ͊֌W�̂Ȃ����C�u�������g�p���Ă���
        private void pnlBarcode_Paint(object sender, PaintEventArgs e)
        {
            DotNetBarcode barCode = new DotNetBarcode();
            Single x1;
            Single y1;
            Single x2;
            Single y2;
            x1 = 0;
            y1 = 0;
            x2 = pnlBarcode.Size.Width;
            y2 = pnlBarcode.Size.Height;
            barCode.Type = DotNetBarcode.Types.Code39;

            if (barcodeNumber != String.Empty)
                barCode.WriteBar(barcodeNumber, x1, y1, x2, y2, e.Graphics);
        }

        // �ݒ�e�L�X�g�t�@�C���̓ǂݍ���
        private string readIni(string s, string k, string cfs)
        {
            StringBuilder retVal = new StringBuilder(255);
            string section = s;
            string key = k;
            string def = String.Empty;
            int size = 255;
            //get the value from the key in section
            int strref = GetPrivateProfileString(section, key, def, retVal, size, cfs);
            return retVal.ToString();
        }

        // Windows API ���C���|�[�g
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def,
                                                  StringBuilder retVal, int size, string filepath);
        
        // ����{�^����V���[�g�J�b�g�ł̏I���������Ȃ�
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;
            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE) { return; }
            base.WndProc(ref m);
        }
    }
}