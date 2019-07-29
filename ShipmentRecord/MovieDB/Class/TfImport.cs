using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;
using System.IO;

namespace QA_Management
{
    public class TfImport
    {
        public string Serial { get; set; }
        public string Config { get; set; }
        public string ShipDate { get; set; }
        public string Status { get; set; }

        public static List<TfImport> LoadUserListFromExcelFile(string path)
        {
            var tf = new List<TfImport>();

            foreach (var line in File.ReadAllLines(path))
            {
                var columns = line.Split(',');
                //double dBuff;
                //double.TryParse(columns[4].Trim(), out dBuff);

                string buff = columns[0].Trim();
                if (buff.IndexOf("Serial") < 0 && buff != "" && buff != string.Empty)
                {
                    tf.Add(new TfImport
                    {
                        Serial = columns[0].Trim(),
                        Config = columns[1].Trim(),
                        ShipDate = columns[2].Trim(),
                        Status = columns[3],
                    });
                }
            }

            return tf;
        }
    }
}
