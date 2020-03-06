using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using System.Text;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace JigQuick
{
    class TfSQL
    {
        NpgsqlConnection connection;
        string conStringTesterDb = @"Server=192.168.145.12;Port=5432;User Id=pqm;Password=dbuser;Database=pqmdb; CommandTimeout=100; Timeout=100;";
        static string conStringTraceSheetDb = @"Server=192.168.145.4;Port=5432;User Id=pqm;Password=dbuser;Database=ynsdb; CommandTimeout=100; Timeout=100;";

        // 対象レコードをＤＡＴＡＴＡＢＬＥへ格納する（ＴＥＳＴＥＲ ＤＢ）
        public void sqlDataAdapterFillDatatableFromTesterDb(string sql, ref DataTable dt)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conStringTesterDb);
            NpgsqlCommand command = new NpgsqlCommand();

            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter())
            {
                command.CommandText = sql;
                command.Connection = connection;
                adapter.SelectCommand = command;
                adapter.Fill(dt);
            }
        }

        public string sqlExecuteScalarString(string sql)
        {
            string response;
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = Convert.ToString(command.ExecuteScalar());
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SQL executeschalar moethod failed." + "\r\n" + ex.Message
                //                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return String.Empty;
            }
        }

        public void getComboBoxData(string sql, ref ComboBox cmb)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command;
            DataSet ds = new DataSet();
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                command = new NpgsqlCommand(sql, connection);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                adapter.Dispose();
                command.Dispose();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cmb.Items.Add(row[0].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                connection.Close();
            }
        }

        public bool CheckTableExist(string tableName)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conStringTesterDb);
            string cmd = "SELECT EXISTS (SELECT * FROM " + tableName + ")";
            NpgsqlCommand command = new NpgsqlCommand(cmd, connection);
            connection.Open();
            try
            {
                int result = command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                connection.Close();
                return false;
            }
        }
    }
}
