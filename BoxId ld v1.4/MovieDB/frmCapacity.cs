using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Permissions;
using System.Collections;

namespace BoxIdDb
{
    public partial class frmCapacity : Form
    {
        //親フォームfrmBoxidへイベント発生を連絡（デレゲート）
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // コンストラクタ
        public frmCapacity()
        {
            InitializeComponent();
        }

        // ロード時の処理
        private void frmCapacity_Load(object sender, EventArgs e)
        {
            //フォームの場所を指定
            this.Left = 450;
            this.Top = 100;
        }

        // サブプロシージャ：親フォームで呼び出し、親フォームの情報を、テキストボックスへ格納して引き継ぐ
        public void updateControls(string limit)
        {
            txtCountLimit.Text = limit;
        }

        // サブプロシージャ：親フォームで呼び出し、子フォームの情報を受け渡す
        public int getLimit()
        {
            return int.Parse(txtCountLimit.Text);
        }

        // frmModule ラベルあたりのシリアル数（limit）を変更
        private void btnOK_Click(object sender, EventArgs e)
        {
            string limit = txtCountLimit.Text;
            int l;
            if (int.TryParse(limit, out l) && l > 0)
            {
                //親フォームfrmBoxidのデータグリットビューを更新するため、デレゲートイベントを発生させる
                this.RefreshEvent(this, new EventArgs());
                Close();
            }
        }
                        
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