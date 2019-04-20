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

        public List<ExcelData> getExcelFile() {
            try {
                //Create COM Objects. Create a COM object for everything that is referenced
                Excel.Application xlApp = new Excel.Application();
                //C:\Users\Dragan\Documents\Igor\my\pt\files
                // Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\E56626\Desktop\Teddy\VS2012\Sandbox\sandbox_test - Copy - Copy.xlsx");
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Dragan\Documents\Igor\my\pt\files\products.xlsx");
                //  Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                // Excel.Range xlRange = xlWorksheet.UsedRange;

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




                    // string value = r.Value2.ToString();
                }

                //Excel.Worksheet sheet = workbook.Sheets["Sheet1"] as Excel.Worksheet;
                //Excel.Range range = sheet.get_Range("A1", Missing.Value)

                /*
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount; j++)
                    {
                        //new line
                        if (j == 1)
                            Console.Write("\r\n");

                        //write the value to the console
                        //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                    }
                }
                */

                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

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
            } catch (Exception e) {
                return new List<ExcelData>();
            }

           
        }
    }

}
