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
    public class TfSqlTracy
    {
        NpgsqlConnection connection;
        static string conStringTraceSheetDb = @"Server=192.168.193.4;Port=5432;User Id=pqm;Password=dbuser;Database=tracyadb; CommandTimeout=100; Timeout=100;";
        //static string conStringPqmDb = @"Server=192.168.193.4;Port=5432;User Id=pqm;Password=dbuser;Database=pqmdb; CommandTimeout=100; Timeout=100;";

        // Ａ：オペレーター
        public bool sqlInsertOperatorInfo(string batchNo, DataTable dtApp)
        {
            // 個別ＳＱＬ実行結果と、その積み上げである全体ＳＱＬ実行結果
            int res1;
            bool res2 = false;

            connection = new NpgsqlConnection(conStringTraceSheetDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                string sql0 = "delete from t_operator " + "where batch_no='" + batchNo + "'";
                System.Diagnostics.Debug.Print(sql0);
                NpgsqlCommand command0 = new NpgsqlCommand(sql0, connection);
                command0.ExecuteNonQuery();

                for (int i = 0; i < dtApp.Rows.Count; i++)
                {
                    string process = dtApp.Rows[i]["process"].ToString();

                    // ＤＴプロセス番号が空の場合は、処理なし
                    if (process == string.Empty)
                    {
                        System.Diagnostics.Debug.Print("処理なし");
                    }
                    else
                    {
                        string sql1 = "INSERT INTO t_operator(batch_no, process, operator, machine) " +
                            "VALUES (:batch_no, :process, :operator, :machine)";
                        NpgsqlCommand command = new NpgsqlCommand(sql1, connection);
                        command.Parameters.Add(new NpgsqlParameter("batch_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("process", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("operator", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("machine", NpgsqlTypes.NpgsqlDbType.Varchar));

                        command.Parameters[0].Value = batchNo;
                        command.Parameters[1].Value = dtApp.Rows[i]["process"].ToString();
                        command.Parameters[2].Value = dtApp.Rows[i]["operator"].ToString();
                        command.Parameters[3].Value = dtApp.Rows[i]["machine"].ToString();

                        res1 = command.ExecuteNonQuery();
                        if (res1 == -1) res2 = true;
                    }
                }

                if (!res2)
                {
                    transaction.Commit();
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }


        // Ｂ：パーツ
        public bool sqlDeleteInsertPartsInfo(string batchNo, DataTable dtApp)
        {
            // 個別ＳＱＬ実行結果と、その積み上げである全体ＳＱＬ実行結果
            int res1;
            bool res2 = false;
            
            connection = new NpgsqlConnection(conStringTraceSheetDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                string sql0 = "delete from t_parts_invoice " + "where batch_no='" + batchNo + "'";
                System.Diagnostics.Debug.Print(sql0);
                NpgsqlCommand command0 = new NpgsqlCommand(sql0, connection);
                command0.ExecuteNonQuery();

                for (int i = 0; i < dtApp.Rows.Count; i++)
                {
                    string parts = dtApp.Rows[i]["parts_no"].ToString();

                    // ＤＴ部品番号が空の場合は、処理なし
                    if (parts == string.Empty)
                    {
                        System.Diagnostics.Debug.Print("処理なし");
                    }
                    else
                    {
                        string sql1 = "INSERT INTO t_parts_invoice(batch_no, parts_no, parts_name, parts_supplier, parts_invoice, qty, note) " +
                            "VALUES (:batch_no, :parts_no, :parts_name, :parts_supplier, :parts_invoice, :qty, :note)";
                        NpgsqlCommand command = new NpgsqlCommand(sql1, connection);
                        command.Parameters.Add(new NpgsqlParameter("batch_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("parts_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("parts_name", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("parts_supplier", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("parts_invoice", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("qty", NpgsqlTypes.NpgsqlDbType.Double));
                        command.Parameters.Add(new NpgsqlParameter("note", NpgsqlTypes.NpgsqlDbType.Varchar));

                        command.Parameters[0].Value = batchNo;
                        command.Parameters[1].Value = dtApp.Rows[i]["parts_no"].ToString();
                        command.Parameters[2].Value = dtApp.Rows[i]["parts_name"].ToString();
                        command.Parameters[3].Value = dtApp.Rows[i]["parts_supplier"].ToString();
                        command.Parameters[4].Value = dtApp.Rows[i]["parts_invoice"].ToString();
                        command.Parameters[5].Value = dtApp.Rows[i]["qty"];
                        command.Parameters[6].Value = dtApp.Rows[i]["note"].ToString();

                        res1 = command.ExecuteNonQuery();
                        if (res1 == -1) res2 = true;
                    }
                }

                if (!res2)
                {
                    transaction.Commit();
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }


        // Ｃ：副資材
        public bool sqlDeleteInsertSubMaterialInfo(string batchNo, DataTable dtApp)
        {
            // 個別ＳＱＬ実行結果と、その積み上げである全体ＳＱＬ実行結果
            int res1;
            bool res2 = false;

            connection = new NpgsqlConnection(conStringTraceSheetDb);
            connection.Open();
            NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
                string sql0 = "delete from t_sub_mat_invoice " + "where batch_no='" + batchNo + "'";
                System.Diagnostics.Debug.Print(sql0);
                NpgsqlCommand command0 = new NpgsqlCommand(sql0, connection);
                command0.ExecuteNonQuery();

                for (int i = 0; i < dtApp.Rows.Count; i++)
                {
                    string submatno = dtApp.Rows[i]["sub_mat_no"].ToString();

                    // ＤＴ副資材番号が空の場合は、処理なし
                    if (submatno == string.Empty)
                    {
                        System.Diagnostics.Debug.Print("処理なし");
                    }
                    else
                    {
                        string sql1 = "INSERT INTO t_sub_mat_invoice(batch_no, sub_mat_no, sub_mat_name, sub_mat_supplier, sub_mat_invoice, validity) " +
                            "VALUES (:batch_no, :sub_mat_no, :sub_mat_name, :sub_mat_supplier, :sub_mat_invoice, :validity)";
                        NpgsqlCommand command = new NpgsqlCommand(sql1, connection);
                        command.Parameters.Add(new NpgsqlParameter("batch_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("sub_mat_no", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("sub_mat_name", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("sub_mat_supplier", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("sub_mat_invoice", NpgsqlTypes.NpgsqlDbType.Varchar));
                        command.Parameters.Add(new NpgsqlParameter("validity", NpgsqlTypes.NpgsqlDbType.TimestampTZ));

                        command.Parameters[0].Value = batchNo;
                        command.Parameters[1].Value = dtApp.Rows[i]["sub_mat_no"].ToString();
                        command.Parameters[2].Value = dtApp.Rows[i]["sub_mat_name"].ToString();
                        command.Parameters[3].Value = dtApp.Rows[i]["sub_mat_supplier"].ToString();
                        command.Parameters[4].Value = dtApp.Rows[i]["sub_mat_invoice"].ToString();
                        command.Parameters[5].Value = DateTime.Parse(dtApp.Rows[i]["validity"].ToString());

                        res1 = command.ExecuteNonQuery();
                        if (res1 == -1) res2 = true;
                    }
                }

                if (!res2)
                {
                    transaction.Commit();
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
                MessageBox.Show("Not successful!" + "\r\n" + ex.Message
                                , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connection.Close();
                return false;
            }
        }


        // Ｂ：パーツ
        // パーツ情報の、ＴＢＩテーブルへの一括登録
        //public bool sqlMultipleInsertPartsInfoToTbiTable(string tbiTable, string batch, string model, DateTime batchDate, DataTable dt)
        //{
        //    String sernoFieldDummy = "Parts";
        //    int res1;
        //    bool res2 = false;
        //    connection = new NpgsqlConnection(conStringPqmDb);
        //    connection.Open();
        //    NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        //    try
        //    {
        //        // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
        //        string sql0 = "delete from " + tbiTable + " where lot = '" + batch + "' and serno = '" + sernoFieldDummy + "'";
        //        System.Diagnostics.Debug.Print(sql0);
        //        NpgsqlCommand command0 = new NpgsqlCommand(sql0, connection);
        //        command0.ExecuteNonQuery();

        //        string sql = "INSERT INTO " + tbiTable + " (serno, lot, model, regist_date, invoice, part_code, part_name, vendor, shipdate) " +
        //            "VALUES (:serno, :lot, :model, :regist_date, :invoice, :part_code, :part_name, :vendor, :shipdate)";
        //        NpgsqlCommand command = new NpgsqlCommand(sql, connection);

        //        command.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("lot", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("model", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("regist_date", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
        //        command.Parameters.Add(new NpgsqlParameter("invoice", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("part_code", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("part_name", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("vendor", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("shipdate", NpgsqlTypes.NpgsqlDbType.TimestampTZ));

        //        DateTime buff = batchDate;

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            command.Parameters[0].Value = sernoFieldDummy;
        //            command.Parameters[1].Value = batch;
        //            command.Parameters[2].Value = model;
        //            command.Parameters[3].Value = (buff = buff.AddMilliseconds(1));
        //            command.Parameters[4].Value = dt.Rows[i]["parts_invoice"].ToString();
        //            command.Parameters[5].Value = dt.Rows[i]["parts_no"].ToString();
        //            command.Parameters[6].Value = dt.Rows[i]["parts_name"].ToString();
        //            command.Parameters[7].Value = dt.Rows[i]["parts_supplier"].ToString();
        //            command.Parameters[8].Value = batchDate;

        //            if (dt.Rows[i]["parts_no"].ToString() != string.Empty)
        //            {
        //                res1 = command.ExecuteNonQuery();
        //                if (res1 == -1) res2 = true;                    
        //            }
        //        }

        //        if (!res2)
        //        {
        //            transaction.Commit();
        //            connection.Close();
        //            return true;
        //        }
        //        else
        //        {
        //            transaction.Rollback();
        //            MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            connection.Close();
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        MessageBox.Show("Not successful!" + "\r\n" + ex.Message
        //                        , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        connection.Close();
        //        return false;
        //    }
        //}

        //// Ｃ：副資材
        //// 副資材情報の、ＴＢＩテーブルへの一括登録
        //public bool sqlMultipleInsertSubMaterialInfoToTbiTable(string tbiTable, string batch, string model, DateTime batchDate, DataTable dt)
        //{
        //    String sernoFieldDummy = "Sub Material";
        //    int res1;
        //    bool res2 = false;
        //    connection = new NpgsqlConnection(conStringPqmDb);
        //    connection.Open();
        //    NpgsqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        //    try
        //    {
        //        // ＤＢテーブルに既に存在するデータを削除し、登録。ただし例外発生時、削除も登録もロールバック。
        //        string sql0 = "delete from " + tbiTable + " where lot = '" + batch + "' and serno = '" + sernoFieldDummy + "'";
        //        System.Diagnostics.Debug.Print(sql0);
        //        NpgsqlCommand command0 = new NpgsqlCommand(sql0, connection);
        //        command0.ExecuteNonQuery();

        //        string sql = "INSERT INTO " + tbiTable + " (serno, lot, model, regist_date, invoice, part_code, part_name, vendor, shipdate) " +
        //            "VALUES (:serno, :lot, :model, :regist_date, :invoice, :part_code, :part_name, :vendor, :shipdate)";
        //        NpgsqlCommand command = new NpgsqlCommand(sql, connection);

        //        command.Parameters.Add(new NpgsqlParameter("serno", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("lot", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("model", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("regist_date", NpgsqlTypes.NpgsqlDbType.TimestampTZ));
        //        command.Parameters.Add(new NpgsqlParameter("invoice", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("part_code", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("part_name", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("vendor", NpgsqlTypes.NpgsqlDbType.Varchar));
        //        command.Parameters.Add(new NpgsqlParameter("shipdate", NpgsqlTypes.NpgsqlDbType.TimestampTZ));

        //        // 
        //        DateTime buff = batchDate;

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            command.Parameters[0].Value = sernoFieldDummy;
        //            command.Parameters[1].Value = batch;
        //            command.Parameters[2].Value = model;
        //            command.Parameters[3].Value = (buff = buff.AddMilliseconds(1));
        //            command.Parameters[4].Value = dt.Rows[i]["sub_mat_invoice"].ToString();
        //            command.Parameters[5].Value = dt.Rows[i]["sub_mat_no"].ToString();
        //            command.Parameters[6].Value = dt.Rows[i]["sub_mat_name"].ToString();
        //            command.Parameters[7].Value = dt.Rows[i]["sub_mat_supplier"].ToString();
        //            command.Parameters[8].Value = batchDate;

        //            if (dt.Rows[i]["sub_mat_no"].ToString() != string.Empty)
        //            {
        //                res1 = command.ExecuteNonQuery();
        //                if (res1 == -1) res2 = true;
        //            }
        //        }

        //        if (!res2)
        //        {
        //            transaction.Commit();
        //            connection.Close();
        //            return true;
        //        }
        //        else
        //        {
        //            transaction.Rollback();
        //            MessageBox.Show("Not successful!", "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            connection.Close();
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        MessageBox.Show("Not successful!" + "\r\n" + ex.Message
        //                        , "Database Responce", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        connection.Close();
        //        return false;
        //    }
        //}
    }
}
