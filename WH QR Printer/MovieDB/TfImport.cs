using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.IO;

namespace WhQrPrinter
{
    public class TfImport
    {
        public string ItemNumber { get; set; }
        public string ItemName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierInvoice { get; set; }
        public string Delivery { get; set; }
        public string DeliveredQTY { get; set; }
        public string Validity { get; set; }
        public string MaterialNo { get; set; }
        public string LotNo { get; set; }
        public string PONo { get; set; }
        public string POLine { get; set; }

        public static List<TfImport> LoadUserListFromPremacFile(string path)
        {
            var tf = new List<TfImport>();

            foreach (var line in File.ReadAllLines(path))
            {
                var columns = line.Split('?');
                double dBuff;
                double.TryParse(columns[11].Trim(), out dBuff);

                string buff = columns[0].Trim();
                if (buff.IndexOf("(CPFXE049)") < 0 && buff.IndexOf("SupplierCD") < 0 && buff != "" && buff != string.Empty)
                {
                    tf.Add(new TfImport
                    {
                        ItemNumber = columns[2].Trim(),
                        ItemName = columns[3].Trim(),
                        SupplierName = columns[1].Trim(),
                        SupplierInvoice = columns[30].Trim(),
                        Delivery = columns[10].Trim(),
                        DeliveredQTY = dBuff.ToString("#,##0"),
                        Validity = VBStrings.Left(columns[0].Trim(), 0)
                    });
                }
            }

            tf.Sort((a, b) => a.ItemNumber.CompareTo(b.ItemNumber));
            return tf;
        }

        public static List<TfImport> LoadUserListFromExcelFile(string path)
        {
            var tf = new List<TfImport>();

            foreach (var line in File.ReadAllLines(path))
            {
                var columns = line.Split(',');
                double dBuff;
                double.TryParse(columns[4].Trim(), out dBuff);

                string buff = columns[0].Trim();
                if (buff.IndexOf("Supplier Name") < 0 && buff != "" && buff != string.Empty)
                {
                    tf.Add(new TfImport
                    {
                        ItemNumber = columns[1].Trim(),
                        ItemName = columns[2].Trim(),
                        SupplierName = columns[0].Trim(),
                        SupplierInvoice = columns[5].Trim(),
                        Delivery = columns[3].Trim(),
                        DeliveredQTY = dBuff.ToString("#,##0"),
                        Validity = VBStrings.Left(columns[0].Trim(), 0)
                    });
                }
            }

            return tf;
        }

        //    public static List<TfImport> LoadUserListNCVHExcelFile(string path)
        //    {
        //        var tf = new List<TfImport>();

        //        foreach (var line in File.ReadAllLines(path))
        //        {
        //            var columns = line.Split(',');
        //            //double dBuff;
        //            //double.TryParse(columns[2].Trim(), out dBuff);

        //            string buff = columns[0].Trim();
        //            if (buff.IndexOf("MaterialNo") < 0 && buff != "" && buff != string.Empty)
        //            {
        //                tf.Add(new TfImport
        //                {
        //                    MaterialNo = columns[0].Trim(),
        //                    LotNo = columns[1].Trim(),
        //                    PONo = columns[3].Trim(),
        //                    POLine = columns[4].Trim(),
        //                    DeliveredQTY = columns[2].Trim(),
        //                });
        //            }
        //        }

        //        return tf;
        //    }
        //}

        public static DataTable LoadUserListNCVHExcelFile(string path, DataTable dt)
        {
            //var tf = new List<TfImport>();
            dt = new DataTable();
            dt.Columns.Add("MaterialNo", Type.GetType("System.String"));
            dt.Columns.Add("LotNo", Type.GetType("System.String"));
            dt.Columns.Add("DeliveredQTY", Type.GetType("System.String"));
            dt.Columns.Add("PONo", Type.GetType("System.String"));
            dt.Columns.Add("POLine", Type.GetType("System.String"));

            foreach (var line in File.ReadAllLines(path))
            {
                var columns = line.Split(',');
                //double dBuff;
                //double.TryParse(columns[2].Trim(), out dBuff);

                string buff = columns[0].Trim();
                if (buff.IndexOf("MaterialNo") < 0 && buff != "" && buff != string.Empty)
                {
                    dt.Rows.Add(columns[0].ToString(), columns[1].ToString(), columns[2].ToString(), columns[3].ToString(), columns[4].ToString());
                }
            }
            return dt;

        }
    }
}
