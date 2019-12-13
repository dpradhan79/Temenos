//-----------------------------------------------------------------------
// <copyright file="ExcelReader" company="Temenos">
// Copyright (c) Temenos All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// Standard Utilities
/// </summary>
namespace StandardUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Excel = Microsoft.Office.Interop.Excel;
    using ExlInterop = Microsoft.Office.Interop.Excel;

    /// <summary>
    /// @Modified - Cigniti
    /// </summary>
    public static class ExcelReader
    {
        #region Public Methods
        /// <summary>
        /// Reads the XLXS file.
        /// </summary>
        /// <param name="testDataFolderPath">The test data folder path.</param>
        /// <param name="dataFile">The data file.</param>
        /// <param name="sheetname">The sheetname.</param>
        /// <returns>Data Table</returns>
        public static DataTable ReadXLXSFile(string testDataFolderPath, string dataFile, string sheetname)
        {
            DataTable dt = new DataTable();
            return dt;
        }

        /// <summary>
        /// Reads the excel file.
        /// </summary>
        /// <param name="testDataFolderPath">The test data folder path.</param>
        /// <param name="dataFile">The data file.</param>
        /// <param name="sheetname">The sheetname.</param>
        /// <param name="isOLEDB">if set to <c>true</c> [is oledb].</param>
        /// <param name="testDataStartRow">The test data start row.</param>
        /// <returns>Data Table</returns>
        public static DataTable ReadExcelFile(string testDataFolderPath, string dataFile, string sheetname, bool isOLEDB, int testDataStartRow = 1)
        {
            DataTable dt = new DataTable();
            ////tblExlData holds only data rows of excel sheet
            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            try
            {
                string testDataFolder = testDataFolderPath;
                string[] testdataFiles = Directory.GetFiles(testDataFolder).Where(x => x.EndsWith(".xls") || x.EndsWith(".xlsx")).ToArray();

                foreach (string tstDataFile in testdataFiles)
                {
                    exlFileName = tstDataFile.Substring(tstDataFile.LastIndexOf("\\") + 1);
                    if (dataFile.Equals(exlFileName))
                    {
                        string exlDataSource = Path.Combine(testDataFolder, exlFileName);
                        if (isOLEDB)
                        {
                            dt = ReadExcelUsingOLEDB(exlDataSource);
                        }
                        else
                        {
                            ////dt = ReadExcelUsingInterop(exlDataSource, sheetname);
                            dt = ReadExcelUsingOpenXml(exlDataSource, sheetname, testDataStartRow);

                            //Below method is used to read the data from excel sheet , when NULL value is not assigned to
                            //empty data value to cell .
                            //dt = ExtractExcelSheetValuesToDataTable(exlDataSource, sheetname, testDataStartRow);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// Writes the excel file.
        /// </summary>
        /// <param name="testDataFolderPath">The test data folder path.</param>
        /// <param name="dataFile">The data file.</param>
        /// <param name="sheetname">The sheetname.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="val">The value.</param>
        public static void WriteExcelFile(string testDataFolderPath, string dataFile, string sheetname, int rowIndex, int columnIndex, string val)
        {
            DataTable dt = new DataTable();
            ////tblExlData holds only data rows of excel sheet
            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            string exlDataSource = string.Empty;
            try
            {
                string fileName = dataFile + ".xlsx";
                string testDataFolder = testDataFolderPath;
                string[] testDataFiles = Directory.GetFiles(testDataFolder).Where(x => x.EndsWith(".xls") || x.EndsWith(".xlsx")).ToArray();
                foreach (string tstDataFile in testDataFiles)
                {
                    exlFileName = tstDataFile.Substring(tstDataFile.LastIndexOf("\\") + 1);
                    if (fileName.Equals(exlFileName))
                    {
                        exlDataSource = Path.Combine(testDataFolder, exlFileName);

                        break;
                    }
                }

                WriteDataToExcelUsingInterop(exlDataSource, sheetname, rowIndex, columnIndex, val);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reads the excel using oledb.
        /// </summary>
        /// <param name="exlDataSource">The exl data source.</param>
        /// <returns>Data Table</returns>
        public static DataTable ReadExcelUsingOLEDB(string exlDataSource)
        {
            OleDbConnection oleDBConnection;
            OleDbDataAdapter oleDBAdapter;
            DataTable tblExcelSchema;
            string sheetName = string.Empty;
            ////dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                string excelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exlDataSource + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
                oleDBConnection = new OleDbConnection(excelConn);
                oleDBConnection.Open();
                tblExcelSchema = oleDBConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheetName = tblExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                oleDBAdapter = new OleDbDataAdapter("select * from [" + sheetName + "]", oleDBConnection);
                dt.TableName = "TestData";
                oleDBAdapter.Fill(dt);
                oleDBConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// Reads the excel using open XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="testDataStartRow">The test data start row.</param>
        /// <returns>Data Table</returns>
        public static DataTable ReadExcelUsingOpenXml(string fileName, string sheetName, int testDataStartRow)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = GetWorksheetFromSheetName(workbookPart, sheetName);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetOpenXmlSpreadSheetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    var itemIndex = 0;
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        var cellValue = GetOpenXmlSpreadSheetCellValue(spreadSheetDocument, cell);
                        dataRow[itemIndex] = (cellValue.Trim() == "NULL") ? string.Empty : cellValue;
                        itemIndex++;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Reads the excel using interop.
        /// </summary>
        /// <param name="exlDataSource">The exl data source.</param>
        /// <param name="sheetName">The sheetname.</param>
        /// <returns>Data Table</returns>
        public static DataTable ReadExcelUsingInterop(string exlDataSource, string sheetName)
        {
            ExlInterop.Application exlApp = new ExlInterop.Application();
            ExlInterop.Workbook exlWb = null;
            ExlInterop.Worksheet exlSheet;

            int rowCount = 0;
            int columnCount = 0;
            object cellValue = null;
            string colValue = string.Empty;

            ////dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                exlWb = exlApp.Workbooks.Open(exlDataSource);

                int numSheets = exlWb.Sheets.Count;
                for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
                {
                    exlSheet = (ExlInterop.Worksheet)exlWb.Sheets[sheetNum];
                    string strWorksheetName = exlSheet.Name;
                    exlWb.RefreshAll();
                    if (strWorksheetName.Equals(sheetName))
                    {
                        ExlInterop.Range range = exlSheet.UsedRange;
                        for (rowCount = 1; rowCount <= range.Rows.Count; rowCount++)
                        {
                            DataRow drow = dt.NewRow();
                            for (columnCount = 1; columnCount <= range.Columns.Count; columnCount++)
                            {
                                cellValue = (range.Cells[rowCount, columnCount] as Excel.Range).Value2;
                                colValue = cellValue == null ? string.Empty : Convert.ToString(cellValue);
                                ////Adding Header Row to DataTable
                                if (rowCount == 1)
                                {
                                    dt.Columns.Add(colValue);
                                }
                                else
                                {
                                    drow[columnCount - 1] = colValue;
                                }
                            }

                            if (rowCount > 1)
                            {
                                dt.Rows.Add(drow);
                                dt.AcceptChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                exlWb.Close();
                exlApp.Quit();
                ReleaseProcessObject(exlWb);
                ReleaseProcessObject(exlApp);
            }

            return dt;
        }

        /// <summary>
        /// Writes the data to excel using interop.
        /// </summary>
        /// <param name="exlDataSource">The exl data source.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="val">The value.</param>
        public static void WriteDataToExcelUsingInterop(string exlDataSource, string sheetName, int rowIndex, int columnIndex, string val)
        {
            ExlInterop.Application exlApp = new ExlInterop.Application
            {
                Visible = true
            };
            ExlInterop.Workbook exlWb = null;
            ExlInterop.Worksheet exlSheet;
            try
            {
                exlWb = exlApp.Workbooks.Open(exlDataSource, 0, false, 5, string.Empty, string.Empty, true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, false, false);
                exlSheet = (Microsoft.Office.Interop.Excel.Worksheet)exlWb.Worksheets[sheetName];
                exlSheet.Cells[rowIndex, columnIndex] = val;
                exlWb.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                exlWb.Close();
                exlApp.Quit();
                ReleaseProcessObject(exlWb);
                ReleaseProcessObject(exlApp);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the name of the worksheet from sheet.
        /// </summary>
        /// <param name="workbookPart">The workbook part.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <returns>Worksheet Part</returns>
        /// <exception cref="Exception">Could not find sheet with the given name</exception>
        private static WorksheetPart GetWorksheetFromSheetName(WorkbookPart workbookPart, string sheetName)
        {
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                throw new Exception(string.Format("Could not find sheet with name {0}", sheetName));
            }
            else
            {
                return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
            }
        }

        /// <summary>
        /// Gets the open XML spread sheet cell value.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>cell value as string</returns>
        private static string GetOpenXmlSpreadSheetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;

            if (cell.CellValue != null)
            {
                
                string value = cell.CellValue.InnerXml;
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
                }
                else
                {
                   
                    return value;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Releases the process object.
        /// </summary>
        /// <param name="obj">The object.</param>
        private static void ReleaseProcessObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }


        static DataTable ExtractExcelSheetValuesToDataTable(string fileName, string sheetName, int testDataStartRow)
        {
            DataTable dt = new DataTable();

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                //WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                //IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                //string relationshipId = sheets.First().Id.Value;
                //WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                //Worksheet workSheet = worksheetPart.Worksheet;
                //SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                //IEnumerable<Row> rows = sheetData.Descendants<Row>();


                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = GetWorksheetFromSheetName(workbookPart, sheetName);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }
                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();
                    int columnIndex = 0;
                    foreach (Cell cell in row.Descendants<Cell>())
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                tempRow[columnIndex] = ""; //Insert blank data here;
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                        columnIndex++;
                    }

                    dt.Rows.Add(tempRow);
                }

            }
            dt.Rows.RemoveAt(0);
            return dt;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return "";
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            //else if (cell.DataType != null && cell.DataType == CellValues.Date)
            //{

            //    return Convert.ToString(Convert.ToDateTime(stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText));
            //}
            else
            {
                return value;
            }

        }

        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;

        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        #endregion
    }
}
