using System;
using System.Data;
using Npgsql;

namespace JigQuick
{
    class TfSQL
    {
        NpgsqlConnection connection;
        string conStringTesterDb = @"Server=192.168.145.12;Port=5432;User Id=mes;Password=dbuser;Database=mesdb; CommandTimeout=100; Timeout=100;";

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
                connection = new NpgsqlConnection(conStringTesterDb);
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
    }
}
