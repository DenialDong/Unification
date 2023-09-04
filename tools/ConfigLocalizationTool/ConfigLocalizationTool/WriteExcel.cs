using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace ConvertExcel
{
    public sealed class WriteExcel : Singleton<WriteExcel>
    {
        private Dictionary<string, string> TypeCastDic = new Dictionary<string, string>
        {
            { "int", "number" },
            { "string", "string" },
            { "bool", "bool" },
            { "list", "array" },
            { "array", "array" },
        };

        private List<string> langPathList = new List<string>
        {
            "LanguageTable_en.xlsx",
            "LanguageTable_ja.xlsx",
            "LanguageTable_ko.xlsx",
            "LanguageTable_zh-cn.xlsx",
            "LanguageTable_zh-TW.xlsx",
        };
        private ConcurrentBag<Task> Tasks = new ConcurrentBag<Task>();

        public async Task WriteToFolder(string folderPath)
        {
            Tasks.Clear();
            folderPath = $"{folderPath}/LanguageTable";
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                ErrorMsgMgr.Instance.AddErrorMsg($"{folderPath}不存在");
                return;
            }

            var langList = ExcelDataMgr.Instance.GetLangList();
            foreach (var langPath in langPathList)
            {
                Tasks.Add(Task.Run(() => 
                {
                    try
                    {
                        WriteToTargetLanguage($"{folderPath}/{langPath}", langList);
                    }
                    catch(Exception ex)
                    {
                        // Handle the exception as appropriate, e.g., logging or storing it somewhere.
                        ErrorMsgMgr.Instance.AddErrorMsg($"Failed to read {langPath}: {ex.Message}");
                    }
                }));
            }
            await Task.WhenAll(Tasks);
        }

        private void WriteToTargetLanguage(string path, HashSet<string> langList)
        {
            Dictionary<string, string> excelContent = new Dictionary<string, string>();
            ExcelPackage excel = null;
            Stream stream = null;
            try
            {
                excel = new ExcelPackage($"{path}");

                var worksheet =
                    excel.Workbook.Worksheets[0];
                int lastRow = worksheet.Dimension.End.Row;

                int startIndex = 1;
                while (startIndex <= lastRow)
                {
                    var text = worksheet.Cells[startIndex, 1].Text;
                    if (text != null && !text.Contains("##"))
                    {
                        break;
                    }

                    startIndex++;
                }

                for (int row = startIndex; row <= lastRow; row++)
                {
                    excelContent[worksheet.Cells[row, 2].Text] = worksheet.Cells[row, 3].Text;
                }

                int i = startIndex;
                foreach (var content in excelContent)
                {
                    if (langList.Contains(content.Key))
                    {
                        worksheet.Cells[i, 2].Value = content.Key;
                        worksheet.Cells[i, 3].Value = content.Value;
                        ++i;
                    }
                }

                foreach (var lang in langList)
                {
                    if (!excelContent.ContainsKey(lang))
                    {
                        worksheet.Cells[i, 2].Value = lang;
                        worksheet.Cells[i, 3].Value = "";
                        ++i;
                    }
                }

                //excel.Save();
                stream = new FileStream($"{path}", FileMode.Create,
                    FileAccess.Write, FileShare.ReadWrite);
                excel.SaveAs(stream);
            }
            catch (Exception e)
            {
                ErrorMsgMgr.Instance.AddErrorMsg(
                    $"{path}写入出错, 检查是否Excel是否在另外的进程中打开\n 详细信息:\n{e}\n\n");
            }
            finally
            {
                if (excel != null)
                    excel.Dispose();
                if (stream != null)
                    stream.Dispose();
            }
        }

        private void AppendNormalColumn(ExcelWorksheet worksheet, NormalColumn column, int col)
        {
            int row = 1;
            foreach (var rowContent in column.GetColumnContent())
            {
                worksheet.SetValue(row, col, rowContent);
                row++;
            }
        }

        private void AppendDataColumn(ExcelWorksheet worksheet, DataColumn column, int col)
        {
            string columnFieldName = column.GetFieldName();
            string columnAliasName = column.GetAliasName();
            string dataType;
            if (!TypeCastDic.TryGetValue(column.GetDataType(), out dataType))
            {
                dataType = "string";
            }

            worksheet.SetValue(1, col, columnFieldName);
            worksheet.SetValue(2, col, columnAliasName);
            worksheet.SetValue(3, col, dataType);

            int row = 4;
            foreach (var rowContent in column.GetColumnContent())
            {
                worksheet.SetValue(row, col, rowContent);
                row++;
            }
        }

        private void WriteToStructSheet(ExcelWorksheet workStructSheet, StructColumn structColumn)
        {
            workStructSheet.SetValue(1, 1, "id");
            workStructSheet.SetValue(2, 1, "ID");
            workStructSheet.SetValue(3, 1, "number");
            int structRow = 4;
            foreach (var structColumnContent in structColumn.GetColumnContent())
            {
                workStructSheet.SetValue(structRow++, 1, structColumnContent);
            }


            int structCol = 2;
            foreach (var dataColumn in structColumn.GetDataColumns())
            {
                workStructSheet.SetValue(1, structCol, dataColumn.GetFieldName());
                workStructSheet.SetValue(2, structCol, dataColumn.GetAliasName());
                if (TypeCastDic.TryGetValue(dataColumn.GetDataType(), out var dataStructType))
                {
                    workStructSheet.SetValue(3, structCol, dataStructType);
                }
                else
                {
                    workStructSheet.SetValue(3, structCol, "string");
                }

                int structSheetRow = 4;
                foreach (var rowContent in dataColumn.GetColumnContent())
                {
                    workStructSheet.SetValue(structSheetRow, structCol, rowContent);
                    structSheetRow++;
                }

                structCol++;
            }
        }
    }
}