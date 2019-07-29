using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;

namespace BoxIdDb
{
    public class ShPrint
    {

        public void createBoxidFiles(string dir, string id, string model, string user, DataGridView dgv1, ref DataGridView dgv2, ref TextBox txt)
        {
            //boxId 名のテキストファイルを生成
            string file = dir + id + ".txt";  //+"_" + config + "_" + VBStrings.Right(user, 1) + ".txt";
            FileInfo fi = new FileInfo(file);
            using (FileStream fs = fi.Create())
            {
                fs.Close();
            }

            //boxId 名のビットマップファイルを生成（①まとめ用）
            Bitmap bmp1 = new Bitmap(80 * 7, txt.Height + dgv2.Height * 5);
            Graphics grD = Graphics.FromImage(bmp1);
            grD.Clear(Color.White);

            //テキストボックスのビットマップを生成（②サブ）   
            string cfg;
            cfg = model;
            txt.Text = "Box ID: " + id + "   Model: " + cfg;
            txt.Width = 80 * 5;
            Bitmap bmp2 = new Bitmap(txt.Width, txt.Height);
            txt.DrawToBitmap(bmp2, new Rectangle(0, 0, txt.Width, txt.Height));
            grD.DrawImage(bmp2, 0, 0);

            //データグリットビュー用のビットマップを生成（③サブ）
            adjustDummyDatagridview(dgv1, ref dgv2);
            Bitmap bmp3 = new Bitmap(dgv2.Width, dgv2.Height);
            dgv2.DrawToBitmap(bmp3, new Rectangle(0, 0, dgv2.Width, dgv2.Height));

            //ビットマップのコピー＆ペースト（１回目）
            for (int i = 0; i < 5; i++)
            {
                Rectangle destRect = new Rectangle(0, txt.Height + dgv2.Height * i, 80 * 7, dgv2.Height);
                Rectangle srcRect = new Rectangle(80 * 7 * i, 0, 80 * 7, dgv2.Height);
                grD.DrawImage(bmp3, destRect, srcRect, GraphicsUnit.Pixel);
            }

            //８ビット形式に変換し、ファイルに保存する
            Bitmap bmp4 = ShBitmap.CopyToBpp(bmp1, 8);
            file = dir + id + ".bmp";
            bmp4.Save(file, ImageFormat.Bmp);
        }

        //ダミーデータグリットビューの幅調整
        private int adjustDummyDatagridview(DataGridView dgv1, ref DataGridView dgv2)
        {
            DataTable dt = ((DataTable)dgv1.DataSource).Copy();
            dgv2.DataSource = dt;

            int k = dgv2.Columns.Count;
            dgv2.Width = 80 * k;
            for (int i = 0; i < k; i++)
            {
                dgv2.Columns[i].Width = 80;
            }
            return k;
        }
    }

}
