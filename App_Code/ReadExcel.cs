using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;       //microsoft Excel 14 object in references-> COM tab

/// <summary>
/// ReadExcel
/// </summary>

namespace Igprog {
    public class ReadExcel {
        public ReadExcel() {
            //
            // TODO: Add constructor logic here
            //
        }

        public class ExcelData {
            public int row;
            public string val;
        }

        public List<ExcelData> getExcelFile(string path) {
            //try {
                //Create COM Objects. Create a COM object for everything that is referenced
                Excel.Application xlApp = new Excel.Application();
                //C:\Users\Dragan\Documents\Igor\my\pt\files
                // Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\E56626\Desktop\Teddy\VS2012\Sandbox\sandbox_test - Copy - Copy.xlsx");
                //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Dragan\Documents\Igor\my\pt\files\products.xlsx");

                string test = Path.GetFullPath(path);
                //  Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(string.Format("{0}", test));
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Path.GetFullPath(path));


                Excel.Worksheet xlWorksheet = xlWorkbook.Sheets["Sheet1"] as Excel.Worksheet;
                Excel.Range xlRange = xlWorksheet.UsedRange; //.get_Range("A1", Missing.Value)

                List<ExcelData> xx = new List<ExcelData>();
                foreach (Excel.Range r in xlRange) {
                    if (r != null) {
                        ExcelData x = new ExcelData();
                        x.row = r.Row;
                        x.val = r.Text.ToString();
                        xx.Add(x);
                    }
                }

                

                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);

                return xx;
            //} catch (Exception e) {
            //    return new List<ExcelData>();
            //}

           
        }
    }

}
