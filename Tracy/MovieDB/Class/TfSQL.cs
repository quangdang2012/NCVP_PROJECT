using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using Npgsql;

namespace Tracy
{
    public class TfSQL
    {
        NpgsqlConnection connection;
        static string conStringTraceSheetDb = @"Server=192.168.145.4;Port=5432;User Id=pqm;Password=dbuser;Database=tracydb; CommandTimeout=100; Timeout=100;";
        //static string conStringPqmDb = @"Server=192.168.193.4;Port=5432;User Id=pqm;Password=dbuser;Database=pqmdb; CommandTimeout=100; Timeout=100;";

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
                cmb.Items.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cmb.Items.Add(row[0].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
            }
        }

        public void getComboBoxDataViaCsv(string sql, ref ComboBox cmb)
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
                connection.Close();

                string text = ds.Tables[0].Rows[0][0].ToString();
                if (text == String.Empty) return;

                cmb.Items.Clear();
                string[] words = text.Split(',');
                foreach (string s in words)
                {
                    string t = s.Trim();
                    cmb.Items.Add(t); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                connection.Close();
            }
        }

        public void getAutoCompleteData(string sql, ref TextBox txt)
        {
            txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();

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
                connection.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataCollection.Add(row[0].ToString());
                }
                txt.AutoCompleteCustomSource = DataCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                connection.Close();
            }
        }

        // 本プロジェクトでの用途：当月テーブルと翌月テーブルから、ＰＡＳＳの一番古い日付を取得
        // データが存在しない場合は、ＤＡＴＥＴＩＭＥの最大値を返す
        //public DateTime sqlExecuteScalarDate(string sql)
        //{
        //    DateTime response;
        //    try
        //    {
        //        connection = new NpgsqlConnection(conStringPqmDb);
        //        connection.Open();
        //        NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        //        command.CommandText = sql;
        //        response = (DateTime)command.ExecuteScalar();
        //        connection.Close();
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("SQL executeschalar moethod failed." + "\r\n" + ex.Message
        //        //                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        connection.Close();
        //        return DateTime.MaxValue;
        //    }
        //}

        public double sqlExecuteScalarDouble(string sql)
        {
            double response;
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = Convert.ToDouble(command.ExecuteScalar());
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return 100;
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

        public bool sqlExecuteScalarBool(string sql)
        {
            bool response;
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = (bool)command.ExecuteScalar();
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        public long sqlExecuteScalarLong(string sql)
        {
            long response;
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = (long)command.ExecuteScalar();
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return 0;
            }
        }

        // ＴＲＡＣＹ ＤＢ へのＳＱＬ実行
        public bool sqlExecuteNonQuery(string sql, bool result_message_show)
        {
            try
            {
                connection = new NpgsqlConnection(conStringTraceSheetDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                int response = command.ExecuteNonQuery();
                if (response >= 1)
                {
                    if (result_message_show) { MessageBox.Show("Successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Information); }                    
                    connection.Close();
                    return true;
                }
                else
                {
                    //MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        // ＰＱＭ ＤＢ へのＳＱＬ実行
        //public bool sqlExecuteNonQueryToPqmDb(string sql, bool result_message_show)
        //{
        //    try
        //    {
        //        connection = new NpgsqlConnection(conStringPqmDb);
        //        connection.Open();
        //        NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        //        int response = command.ExecuteNonQuery();
        //        if (response >= 1)
        //        {
        //            if (result_message_show) { MessageBox.Show("Successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        //            connection.Close();
        //            return true;
        //        }
        //        else
        //        {
        //            //MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            connection.Close();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        connection.Close();
        //        return false;
        //    }
        //}

        // ＦＩＬＬ ＦＲＯＭ ＴＲＡＣＹ ＤＢ
        public void sqlDataAdapterFillDatatable(string sql, ref DataTable dt)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conStringTraceSheetDb);
            NpgsqlCommand command = new NpgsqlCommand();

            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter())
            {
                command.CommandText = sql;
                command.Connection = connection;
                adapter.SelectCommand = command;
                adapter.Fill(dt);
             }
        }

        // ＦＩＬＬ ＦＲＯＭ ＰＱＭ ＤＢ
        //public void sqlDataAdapterFillDatatableFromTesterDb(string sql, ref DataTable dt)
        //{
        //    NpgsqlConnection connection = new NpgsqlConnection(conStringPqmDb);
        //    NpgsqlCommand command = new NpgsqlCommand();

        //    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter())
        //    {
        //        command.CommandText = sql;
        //        command.Connection = connection;
        //        adapter.SelectCommand = command;
        //        adapter.Fill(dt);
        //    }
        //}
    }
}
