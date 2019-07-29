using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace IPQC
{
    public class ExcelClass
    {
        // データテーブルをＣＳＶへエクスポート
        public void ExportToCsv(DataTable dt, string strFilePath)
        {
            try
            {
                // Create the CSV file to which grid data will be exported.
                StreamWriter sw = new StreamWriter(strFilePath, false);
                // First we will write the headers.
                //DataTable dt = m_dsProducts.Tables[0];
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);

                // Now write all the rows.

                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString());
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }

                    sw.Write(sw.NewLine);
                }
                sw.Close();
                MessageBox.Show("The data export complete." + System.Environment.NewLine + strFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error happened in the process.");
                throw ex;
            }
        }

        // データテーブルをエクセルへエクスポート 
        public void ExportToExcel(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();
                Excel.Worksheet ws = excelApp.ActiveSheet;

                // column headings
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;
                }

                // rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.Cells[(i + 2), (j + 1)] = dt.Rows[i][j];
                    }
                }
                excelApp.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error happened in the process.");
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        // データテーブルをエクセルへエクスポート（ＸＲ管理図・ヒストグラム付き）    
        public void ExportToExcelWithXrChart(DataTable dt)
        {
            if (dt.Rows.Count == 0) return;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // column headings
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;
            }

            // rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[(i + 2), (j + 1)] = dt.Rows[i][j];
                }
            }

            int row = xlWorkSheet.UsedRange.Rows.Count;
            string address1 = "B1:B" + row.ToString() + ",K1:K" + row.ToString() + ",M1:N" + row.ToString();
            string address2 = "B1:B" + row.ToString() + ",L1:L" + row.ToString();
            string address3 = "AB41:AC53";
            string address4 = "F1:J" + row.ToString();

            // チャート１（Ｘ－Ｒ管理図１）
            Excel.Range chartRange1;
            Excel.ChartObjects xlCharts1 = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart1 = (Excel.ChartObject)xlCharts1.Add(800, 10, 600, 250);
            Excel.Chart chartPage1 = myChart1.Chart;

            chartRange1 = xlWorkSheet.get_Range(address1);
            chartPage1.SetSourceData(chartRange1, misValue);
            chartPage1.ChartType = Excel.XlChartType.xlLine;
            chartPage1.HasLegend = false;
            chartPage1.HasTitle = true;
            chartPage1.ChartTitle.Text = "X  " + dt.Rows[0]["inspect"].ToString() + "  " + dt.Rows[0]["line"].ToString();

            Excel.Axis xAxis1 = (Excel.Axis)chartPage1.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis1.CategoryType = Excel.XlCategoryType.xlCategoryScale;

            Excel.SeriesCollection SeriesCollection1 = (Excel.SeriesCollection)myChart1.Chart.SeriesCollection(misValue);
            Excel.Series s2 = SeriesCollection1.Item(2);
            s2.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbCoral;
            Excel.Series s3 = SeriesCollection1.Item(3);
            s3.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbCoral;

            // チャート２（Ｘ－Ｒ管理図２）
            Excel.Range chartRange2;
            Excel.ChartObjects xlCharts2 = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart2 = (Excel.ChartObject)xlCharts1.Add(800, 280, 600, 250);
            Excel.Chart chartPage2 = myChart2.Chart;

            chartRange2 = xlWorkSheet.get_Range(address2);
            chartPage2.SetSourceData(chartRange2, misValue);
            chartPage2.ChartType = Excel.XlChartType.xlLine;
            chartPage2.HasLegend = false;
            chartPage2.HasTitle = true;
            chartPage2.ChartTitle.Text = "R  " + dt.Rows[0]["inspect"].ToString() + "  " + dt.Rows[0]["line"].ToString();

            Excel.Axis xAxis2 = (Excel.Axis)chartPage2.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis2.CategoryType = Excel.XlCategoryType.xlCategoryScale;

            // チャート３（ヒストグラム）
            Excel.Range chartRange3;
            Excel.ChartObjects xlCharts3 = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart3 = (Excel.ChartObject)xlCharts1.Add(800, 550, 350, 250);
            Excel.Chart chartPage3 = myChart3.Chart;

            string[,] formulas = new string[13, 3];
            string[] formula1 = new string[]
            {
                "BIN",
                "=MIN(" + address4 + ")",
                "=AA42+(AA$53-AA$42)/10",
                "=AA43+(AA$53-AA$42)/10",
                "=AA44+(AA$53-AA$42)/10",
                "=AA45+(AA$53-AA$42)/10",
                "=AA46+(AA$53-AA$42)/10",
                "=AA47+(AA$53-AA$42)/10",
                "=AA48+(AA$53-AA$42)/10",
                "=AA49+(AA$53-AA$42)/10",
                "=AA50+(AA$53-AA$42)/10",
                "=AA51+(AA$53-AA$42)/10",
                "=MAX(" + address4 + ")",
            };
            string[] formula2 = new string[]
            {
                @"LABEL",
                @"=TEXT(AA42,""0.0"")",
                @"=TEXT(AA42,""0.0"")&"" - ""&TEXT(AA43,""0.0"")",
                @"=TEXT(AA43,""0.0"")&"" - ""&TEXT(AA44,""0.0"")",
                @"=TEXT(AA44,""0.0"")&"" - ""&TEXT(AA45,""0.0"")",
                @"=TEXT(AA45,""0.0"")&"" - ""&TEXT(AA46,""0.0"")",
                @"=TEXT(AA46,""0.0"")&"" - ""&TEXT(AA47,""0.0"")",
                @"=TEXT(AA47,""0.0"")&"" - ""&TEXT(AA48,""0.0"")",
                @"=TEXT(AA48,""0.0"")&"" - ""&TEXT(AA49,""0.0"")",
                @"=TEXT(AA49,""0.0"")&"" - ""&TEXT(AA50,""0.0"")",
                @"=TEXT(AA50,""0.0"")&"" - ""&TEXT(AA51,""0.0"")",
                @"=TEXT(AA51,""0.0"")&"" - ""&TEXT(AA52,""0.0"")",
                @"=TEXT(AA53,""0.0"")"
            };
            string[] formula3 = new string[]
            {
                @"FREQUENCY",
                @"=COUNTIF(" + address4 + @",""<=""&AA42)",
                @"=COUNTIF(" + address4 + @","">""&AA42)-COUNTIF(" + address4 + @","">""&AA43)",
                @"=COUNTIF(" + address4 + @","">""&AA43)-COUNTIF(" + address4 + @","">""&AA44)",
                @"=COUNTIF(" + address4 + @","">""&AA44)-COUNTIF(" + address4 + @","">""&AA45)",
                @"=COUNTIF(" + address4 + @","">""&AA45)-COUNTIF(" + address4 + @","">""&AA46)",
                @"=COUNTIF(" + address4 + @","">""&AA46)-COUNTIF(" + address4 + @","">""&AA47)",
                @"=COUNTIF(" + address4 + @","">""&AA47)-COUNTIF(" + address4 + @","">""&AA48)",
                @"=COUNTIF(" + address4 + @","">""&AA48)-COUNTIF(" + address4 + @","">""&AA49)",
                @"=COUNTIF(" + address4 + @","">""&AA49)-COUNTIF(" + address4 + @","">""&AA50)",
                @"=COUNTIF(" + address4 + @","">""&AA50)-COUNTIF(" + address4 + @","">""&AA51)",
                @"=COUNTIF(" + address4 + @","">""&AA51)-COUNTIF(" + address4 + @","">=""&AA52)",
                @"=COUNTIF(" + address4 + @","">=""&AA53)"
            };
            for (int i = 0; i < 13; i++)
            {
                xlWorkSheet.Cells[41 + i, 27].Formula = formula1[i];
                xlWorkSheet.Cells[41 + i, 28].Formula = formula2[i];
                xlWorkSheet.Cells[41 + i, 29].Formula = formula3[i];
            }

            chartRange3 = xlWorkSheet.get_Range(address3);
            chartPage3.SetSourceData(chartRange3, misValue);
            chartPage3.ChartType = Excel.XlChartType.xlColumnClustered;
            chartPage3.HasLegend = false;
            chartPage3.HasTitle = true;
            chartPage3.ChartTitle.Text = "Frequency  " + dt.Rows[0]["inspect"].ToString() + "  " + dt.Rows[0]["line"].ToString();

            Excel.ChartGroup ChartGroup1 = (Excel.ChartGroup)myChart3.Chart.ChartGroups(1);
            ChartGroup1.GapWidth = 0;

            xlApp.Visible = true;
        }

        // データテーブルをエクセルへエクスポート（箱ヒゲ図付き）    
        public void ExportToExcelWithBoxPlotChart(DataTable dt)
        {
            if (dt.Rows.Count == 0) return;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // column headings
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, (i + 1)] = dt.Columns[i].ColumnName;
            }

            // rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[(i + 2), (j + 1)] = dt.Rows[i][j];
                }
            }

            int row = xlWorkSheet.UsedRange.Rows.Count;
            string address1 = "C1:C" + row.ToString() + ",H1:L" + row.ToString();
            string address2 = "M2:M" + row.ToString();
            string address3 = "N2:N" + row.ToString();

            Excel.Range chartRange1;
            Excel.Range chartRange2;
            Excel.Range chartRange3;
            Excel.ChartObjects xlCharts1 = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart1 = (Excel.ChartObject)xlCharts1.Add(800, 10, 600, 250);
            Excel.Chart chartPage1 = myChart1.Chart;

            chartRange1 = xlWorkSheet.get_Range(address1);
            chartRange2 = xlWorkSheet.get_Range(address2);
            chartRange3 = xlWorkSheet.get_Range(address3);

            chartPage1.SetSourceData(chartRange1, misValue);
            chartPage1.ChartType = Excel.XlChartType.xlColumnStacked;
            chartPage1.HasTitle = true;
            chartPage1.ChartTitle.Text = dt.Rows[0]["inspect"].ToString() + "  " + dt.Rows[0]["line"].ToString();
            chartPage1.HasLegend = false;

            Excel.Axis xAxis1 = (Excel.Axis)chartPage1.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis1.CategoryType = Excel.XlCategoryType.xlCategoryScale;

            Excel.SeriesCollection oSeriesCollection = (Excel.SeriesCollection)myChart1.Chart.SeriesCollection(misValue);

            Excel.Series s5 = oSeriesCollection.Item(5);
            s5.ChartType = Excel.XlChartType.xlLine;
            s5.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbDarkOrchid;

            Excel.Series s4 = oSeriesCollection.Item(4);
            s4.ChartType = Excel.XlChartType.xlLine;
            s4.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbDarkOrchid;

            Excel.Series s1 = oSeriesCollection.Item(1);
            s1.Format.Fill.ForeColor.RGB = (int)Excel.XlRgbColor.rgbWhite;
            s1.Format.Fill.Transparency = 1;
            s1.Format.Line.Weight = 0;
            s1.HasErrorBars = true;
            s1.ErrorBar(Excel.XlErrorBarDirection.xlY, Excel.XlErrorBarInclude.xlErrorBarIncludeMinusValues,
                Excel.XlErrorBarType.xlErrorBarTypeCustom, chartRange2, chartRange2);

            Excel.Series s3 = oSeriesCollection.Item(3);
            s3.Format.Fill.ForeColor.RGB = (int)Excel.XlRgbColor.rgbAqua;
            s3.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbBlack;
            s3.Format.Line.Weight = 1.0F;
            s3.HasErrorBars = true;
            s3.ErrorBar(Excel.XlErrorBarDirection.xlY, Excel.XlErrorBarInclude.xlErrorBarIncludePlusValues,
                Excel.XlErrorBarType.xlErrorBarTypeCustom, chartRange3, chartRange3);

            Excel.Series s2 = oSeriesCollection.Item(2);
            s2.Format.Fill.ForeColor.RGB = (int)Excel.XlRgbColor.rgbAqua;
            s2.Format.Line.ForeColor.RGB = (int)Excel.XlRgbColor.rgbBlack;
            s2.Format.Line.Weight = 1.0F;

            xlApp.Visible = true;
        }
    }
}
