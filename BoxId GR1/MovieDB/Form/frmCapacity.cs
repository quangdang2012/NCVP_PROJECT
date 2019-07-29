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
        //�e�t�H�[��frmBoxid�փC�x���g������A���i�f���Q�[�g�j
        public delegate void RefreshEventHandler(object sender, EventArgs e);
        public event RefreshEventHandler RefreshEvent;

        // �R���X�g���N�^
        public frmCapacity()
        {
            InitializeComponent();
        }

        // ���[�h���̏���
        private void frmCapacity_Load(object sender, EventArgs e)
        {
            //�t�H�[���̏ꏊ���w��
            this.Left = 450;
            this.Top = 100;
        }

        // �T�u�v���V�[�W���F�e�t�H�[���ŌĂяo���A�e�t�H�[���̏����A�e�L�X�g�{�b�N�X�֊i�[���Ĉ����p��
        public void updateControls(string limit)
        {
            txtCountLimit.Text = limit;
        }

        // �T�u�v���V�[�W���F�e�t�H�[���ŌĂяo���A�q�t�H�[���̏����󂯓n��
        public int getLimit()
        {
            return int.Parse(txtCountLimit.Text);
        }

        // frmModule ���x��������̃V���A�����ilimit�j��ύX
        private void btnOK_Click(object sender, EventArgs e)
        {
            string limit = txtCountLimit.Text;
            int l;
            if (int.TryParse(limit, out l) && l > 0)
            {
                //�e�t�H�[��frmBoxid�̃f�[�^�O���b�g�r���[���X�V���邽�߁A�f���Q�[�g�C�x���g�𔭐�������
                this.RefreshEvent(this, new EventArgs());
                Close();
            }
        }
                        
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