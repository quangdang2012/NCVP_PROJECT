using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace BoxIdPrinter
{
    public partial class Form1 : Form
    {
        // 基本設定の変更を当ファイルで行う
        string config = System.Environment.CurrentDirectory + "\\info.ini";

        // iniファイルのターゲットフォルダ設定を保持する変数
        string directory = @"C:\Users\takusuke.fujii\Desktop\Auto Print\"; //例

        // 装飾用のバーコード表示変数
        string barcodeNumber = String.Empty;

        // コンストラクタ
        public Form1()
        {
            InitializeComponent();
        }

        // ロード時
        private void Form1_Load(object sender, EventArgs e)
        {
            directory = readIni("TARGET DIRECTORY", "DIR", config);
        }

        // メインプロシージャ：タイマーイベントを使用し、１秒ごとに処理を行う
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // 指定のフォルダが存在しない場合は、処理をしない
            if (!System.IO.Directory.Exists(directory)) return;

            // 指定のフォルダ内のファイルをすべて取得
            string[] files = System.IO.Directory.GetFiles(directory, "*", System.IO.SearchOption.AllDirectories);
            // プリントアウトし、ファイルを削除
            for (int i = 0; i < files.Length; i++)
            {
                string fname = System.IO.Path.GetFileName(files[i]);
                if (VBStrings.Right(fname.ToLower(), 4) == ".txt")
                {
                    string boxid = VBStrings.Left(fname, fname.Length - 4);
                    // ２枚印刷
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
                    // ２枚印刷
                    TfPrint.printBitmap(datecdFile);
                    //TfPrint.printBitmap(datecdFile);
                    //TfPrint.printBitmap(datecdFile);
                    System.IO.File.Delete(files[i]);
                }
            }
        }
        
        // サブプロシージャ：装飾用のバーコード表示パネルの更新、実際の出力とは関係のないライブラリを使用している
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

        // 設定テキストファイルの読み込み
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

        // Windows API をインポート
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def,
                                                  StringBuilder retVal, int size, string filepath);
        
        // 閉じるボタンやショートカットでの終了を許さない
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