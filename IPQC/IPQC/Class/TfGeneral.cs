using System;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IPQC
{
    public class TfGeneral
    {

        public static void closeOpenForm(string formName)
        {
            bool bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl) { Application.OpenForms[formName].Close(); }
        }

        public static bool checkOpenFormExists(string formName)
        {
            bool bl = false;
            foreach (Form buff in Application.OpenForms)
            {
                if (buff.Name == formName) { bl = true; }
            }
            if (bl) { Application.OpenForms[formName].Focus(); }
            return bl;
        }
    }
}
