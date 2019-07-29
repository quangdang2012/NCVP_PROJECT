using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace IPQC
{
    public class TfSQL
    {
        NpgsqlConnection connection;
        //  static string conStringIpqcDbP4 = @"Server=192.168.193.4;Port=5432;User Id=pqm;Password=dbuser;Database=ip_pqmdb; CommandTimeout=100; Timeout=100;";
        static string conStringIpqcDbP4 = @"Server=192.168.145.4;Port=5432;User Id=pqm;Password=dbuser;Database=ip_pqmdb; CommandTimeout=100; Timeout=100;";

        public void getComboBoxData(string sql, ref ComboBox cmb)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command;
            DataSet ds = new DataSet();
            try
            {
                connection = new NpgsqlConnection(conStringIpqcDbP4);
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
                connection = new NpgsqlConnection(conStringIpqcDbP4);
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
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
            }
        }

        public double sqlExecuteScalarDouble(string sql)
        {
            double response;
            try
            {
                connection = new NpgsqlConnection(conStringIpqcDbP4);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = Convert.ToDouble(command.ExecuteScalar());
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + System.Environment.NewLine + ex.Message
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
                connection = new NpgsqlConnection(conStringIpqcDbP4);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = Convert.ToString(command.ExecuteScalar());
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar method failed." + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return String.Empty;
            }
        }

        public bool sqlExecuteScalarBool(string sql)
        {
            bool response;
            try
            {
                connection = new NpgsqlConnection(conStringIpqcDbP4);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = (bool)command.ExecuteScalar();
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + System.Environment.NewLine + ex.Message
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
                connection = new NpgsqlConnection(conStringIpqcDbP4);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = (long)command.ExecuteScalar();
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL executeschalar moethod failed." + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return 0;
            }
        }

        public bool sqlExecuteNonQuery(string sql, bool result_message_show)
        {
            try
            {
                connection = new NpgsqlConnection(conStringIpqcDbP4);
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
                    MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not successful!" + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        public int sqlExecuteNonQueryInt(string sql, bool result_message_show)
        {
            try
            {
                connection = new NpgsqlConnection(conStringIpqcDbP4);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                int response = command.ExecuteNonQuery();
                if (response >= 1)
                {
                    if (result_message_show) { MessageBox.Show("Successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    connection.Close();
                    return response;
                }
                else
                {
                    if (result_message_show) { MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    connection.Close();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                if (result_message_show)
                {
                    MessageBox.Show("Not successful!" + System.Environment.NewLine + ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                connection.Close();
                return 0;
            }
        }


        public void sqlDataAdapterFillDatatable(string sql, ref DataTable dt)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conStringIpqcDbP4);
            NpgsqlCommand command = new NpgsqlCommand();

            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter())
            {
                command.CommandText = sql;
                command.Connection = connection;
                adapter.SelectCommand = command;
                adapter.Fill(dt);
             }
        }

        // 測定結果の一括登録
        public bool sqlMultipleInsert(string model, string process, string inspect, DateTime lot, DateTime inspectdate, string line, string status, DataTable dt)
        {
            int res1;
            bool res2 = false;
            connection = new NpgsqlConnection(conStringIpqcDbP4);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                // 削除①：
                string sql1 = "delete from tbl_measure_history where " +
                    "model ='" + model + "' and " +
                    "inspect ='" + inspect + "' and " +
                    "lot ='" + lot + "' and " +
                    "inspectdate ='" + inspectdate + "' and " +
                    "line ='" + line + "'";
                System.Diagnostics.Debug.Print(sql1);
                NpgsqlCommand command1 = new NpgsqlCommand(sql1, connection);
                command1.ExecuteNonQuery();

                // 登録②：
                string sql2 = "insert into tbl_measure_history(model, process, inspect, lot, inspectdate, line, qc_user, status, row_set, m1, m2, m3, m4, m5, x, r) " +
                    "VALUES ('" + model + "','" + process + "','" + inspect + "', '" + lot + "','" + inspectdate + "','" + line + "',:qc_user,'" + status + "',:row_set, :m1, :m2, :m3, :m4, :m5, :x, :r)";
                NpgsqlCommand command2 = new NpgsqlCommand(sql2, connection);

                //command2.Parameters.Add(new NpgsqlParameter("model", NpgsqlTypes.NpgsqlDbType.Varchar));
                //command2.Parameters.Add(new NpgsqlParameter("process", NpgsqlTypes.NpgsqlDbType.Varchar));
                //command2.Parameters.Add(new NpgsqlParameter("inspect", NpgsqlTypes.NpgsqlDbType.Varchar));
                //command2.Parameters.Add(new NpgsqlParameter("lot", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
                //command2.Parameters.Add(new NpgsqlParameter("inspectdate", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
                //command2.Parameters.Add(new NpgsqlParameter("line", NpgsqlTypes.NpgsqlDbType.Varchar));
                command2.Parameters.Add(new NpgsqlParameter("qc_user", NpgsqlTypes.NpgsqlDbType.Varchar));
                //command2.Parameters.Add(new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar));
                command2.Parameters.Add(new NpgsqlParameter("row_set", NpgsqlTypes.NpgsqlDbType.Varchar));
                command2.Parameters.Add(new NpgsqlParameter("m1", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("m2", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("m3", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("m4", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("m5", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("x", NpgsqlTypes.NpgsqlDbType.Double));
                command2.Parameters.Add(new NpgsqlParameter("r", NpgsqlTypes.NpgsqlDbType.Double));

                int k = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //command2.Parameters[0].Value = model;
                    //command2.Parameters[1].Value = process;
                    //command2.Parameters[2].Value = inspect;
                    //command2.Parameters[3].Value = lot;
                    //command2.Parameters[4].Value = inspectdate;
                    //command2.Parameters[5].Value = line;
                    command2.Parameters[0].Value = dt.Rows[i]["qc_user"].ToString();
                    //command2.Parameters[7].Value = status;
                    command2.Parameters[1].Value = k.ToString();
                    command2.Parameters[2].Value = dt.Rows[i]["m1"];
                    command2.Parameters[3].Value = dt.Rows[i]["m2"];
                    command2.Parameters[4].Value = dt.Rows[i]["m3"];
                    command2.Parameters[5].Value = dt.Rows[i]["m4"];
                    command2.Parameters[6].Value = dt.Rows[i]["m5"];
                    command2.Parameters[7].Value = dt.Rows[i]["x"];
                    command2.Parameters[8].Value = dt.Rows[i]["r"];

                    System.Diagnostics.Debug.Print(command2.CommandText);
                    res1 = command2.ExecuteNonQuery();
                    if (res1 == -1) res2 = true;
                    k++;
                }
                
                if (!res2)
                {
                    transaction.Commit();
                    MessageBox.Show("Successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connection.Close();
                    transaction.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Not successful!" + System.Environment.NewLine + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }
    }
}
