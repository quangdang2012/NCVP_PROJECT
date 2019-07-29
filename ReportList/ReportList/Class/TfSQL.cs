using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using Npgsql;
using System.Windows.Forms;

namespace ReportList
{
    public class TfSQL
    {
        NpgsqlConnection connection;
        static string conStringBoxidDb = @"Server=192.168.145.4;Port=5432;User Id=pqm;Password=dbuser;Database=ynsdb; CommandTimeout=100; Timeout=100;";
        public bool sqlMultipleInsertOverall(DataTable dt, string tbl)
        {
            int res1;
            bool res2 = false;
            connection = new NpgsqlConnection(conStringBoxidDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string sql = "INSERT INTO " + tbl + "(serno, config, ship_date, status) " +
                    "VALUES (:serno, :config, :ship_date, :status)";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                command.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("config", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("ship_date", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    command.Parameters[0].Value = dt.Rows[i]["Serial"].ToString();
                    command.Parameters[1].Value = dt.Rows[i]["Config"].ToString();
                    command.Parameters[2].Value = dt.Rows[i]["ShipDate"].ToString();
                    command.Parameters[3].Value = dt.Rows[i]["Status"].ToString();

                    res1 = command.ExecuteNonQuery();
                    if (res1 == -1) res2 = true;
                }

                if (!res2)
                {
                    transaction.Commit();
                    connection.Close();
                    MessageBox.Show("SNs are already registered", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        public bool sqlMultipleInsertLS(DataTable dt, string tbl)
        {
            int res1;
            bool res2 = false;
            connection = new NpgsqlConnection(conStringBoxidDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string sql = "INSERT INTO " + tbl + "(serno, config, ship_date, status) " +
                    "VALUES (:serno, :config, :ship_date, :status)";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                command.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("config", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("ship_date", NpgsqlTypes.NpgsqlDbType.Date));
                command.Parameters.Add(new NpgsqlParameter("status", NpgsqlTypes.NpgsqlDbType.Varchar));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    command.Parameters[0].Value = dt.Rows[i]["Serial"].ToString();
                    command.Parameters[1].Value = dt.Rows[i]["Config"].ToString();
                    command.Parameters[2].Value = DateTime.Parse(dt.Rows[i]["ShipDate"].ToString());
                    command.Parameters[3].Value = dt.Rows[i]["Status"].ToString();

                    res1 = command.ExecuteNonQuery();
                    if (res1 == -1) res2 = true;
                }

                if (!res2)
                {
                    transaction.Commit();
                    connection.Close();
                    MessageBox.Show("SNs are already registered", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        public bool sqlInsertDocument(string tbl, string docName, string docNo, string docType, string version, string model)
        {
            int res1;
            bool res2 = false;
            connection = new NpgsqlConnection(conStringBoxidDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                string sql = "INSERT INTO " + tbl + "(doc_name, doc_no, doc_type, version, update_date, model) " +
                    "VALUES (:doc_name, :doc_no, :doc_type, :version, :update_date, :model)";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                command.Parameters.Add(new NpgsqlParameter("doc_name", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("doc_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("doc_type", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("version", NpgsqlTypes.NpgsqlDbType.Varchar));
                command.Parameters.Add(new NpgsqlParameter("update_date", NpgsqlTypes.NpgsqlDbType.Date));
                command.Parameters.Add(new NpgsqlParameter("model", NpgsqlTypes.NpgsqlDbType.Varchar));

                command.Parameters[0].Value = docName;
                command.Parameters[1].Value = docNo;
                command.Parameters[2].Value = docType;
                command.Parameters[3].Value = version;
                command.Parameters[4].Value = DateTime.Today;
                command.Parameters[5].Value = model;

                res1 = command.ExecuteNonQuery();
                if (res1 == -1) res2 = true;

                if (!res2)
                {
                    transaction.Commit();
                    connection.Close();
                    MessageBox.Show("Document has been already registered", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }

        public void sqlDataAdapterFillDatatable(string sql, ref DataTable dt)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conStringBoxidDb);
            NpgsqlCommand command = new NpgsqlCommand();

            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter())
            {
                command.CommandText = sql;
                command.Connection = connection;
                adapter.SelectCommand = command;
                adapter.Fill(dt);
            }
        }

        public void getComboBoxData(string sql, ref ComboBox cmb)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command;
            DataSet ds = new DataSet();
            try
            {
                connection = new NpgsqlConnection(conStringBoxidDb);
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
                MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
            }
        }

        public string sqlExecuteScalarString(string sql)
        {
            string response;
            try
            {
                connection = new NpgsqlConnection(conStringBoxidDb);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                response = Convert.ToString(command.ExecuteScalar());
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.Print(ex.Message);
                connection.Close();
                return String.Empty;
            }
        }
    }
}