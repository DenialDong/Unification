using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace ConvertExcel
{
    public sealed class ReadExcel : Singleton<ReadExcel>
    {
        private string m_LangBeanName = "Lang";

        private ConcurrentBag<Task> Tasks = new ConcurrentBag<Task>();


        public void ReadFolder(string folderPath, string targetPath)
        {
            WriteExcel.Instance.ReadchineseExcel(targetPath);
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            foreach (FileInfo file in folder.GetFiles("*.json"))
            {
                Dictionary<string, object> data = ReadJsonToDictionary(file.FullName);

                WriteExcel.Instance.WriteToFolder(targetPath, data, file.Name);
            }
        }

        public Dictionary<string, object> ReadJsonToDictionary(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            return dictionary;
        }

        
        private void ReadAllExcelData(DirectoryInfo folder)
        {
            if (!folder.Exists)
            {
                ErrorMsgMgr.Instance.AddErrorMsg($"{folder.Name}文件夹不存在");
                return;
            }

            foreach (DirectoryInfo subFolder in folder.GetDirectories())
            {
                ReadAllExcelData(subFolder);
            }

            foreach (FileInfo file in folder.GetFiles("*.xlsx"))
            {
                if (file.Name.Contains("~$")
                    || file.Name.Contains("LanguageTable")
                    || file.Name.Contains("__"))
                {
                    continue;
                }

                ReadOneExcel(file, folder.FullName);
            }
        }

        public void ReadOneExcel(FileInfo file, string folderPath)
        {
            FileStream fileStream;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel;
            try
            {
                fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                excel = new ExcelPackage(fileStream);
                List<BaseExcelSheet> sheets = new List<BaseExcelSheet>();

                if (excel.Workbook.Worksheets.Count == 0)
                {
                    ErrorMsgMgr.Instance.AddErrorMsg($"{file.Name}表里的sheet数量为0");
                    return;
                }

                List<string> langList = new List<string>();
                foreach (var sheet in excel.Workbook.Worksheets)
                {
                    if (ValidateSheet(sheet))
                    {
                        List<string> sheetLangList = GetSheetLangList(sheet, file.Name);
                        langList.AddRange(sheetLangList);
                    }
                }

                ExcelDataMgr.Instance.AddLang(file.Name, langList);
                excel.Dispose();
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                ErrorMsgMgr.Instance.AddErrorMsg($"读取{file.Name}出错 \n详细信息:\n{e}\n\n");
            }
        }

        private List<string> GetSheetLangList(ExcelWorksheet sheet, string fileName)
        {
            List<string> langList = new List<string>();
            var typeIdx = GetTypeColIndex(sheet);
            int maxColumnNum = sheet.Dimension.End.Column;
            for (var col = 1; col <= maxColumnNum; col++)
            {
                var type = GetValue(sheet, typeIdx, col);
                switch (validateLang(type))
                {
                    case LangBean.None:
                        break;
                    case LangBean.LangItSelf:
                        var langItSelfList = GetColByIndex(col, sheet);
                        langList.AddRange(langItSelfList);
                        break;
                    case LangBean.BeanContainLang:
                        var beanContainLangList = GetLangInSideBean(sheet, col, type, fileName);
                        langList.AddRange(beanContainLangList);
                        break;
                }
            }

            return langList;
        }


        private LangBean validateLang(string type)
        {
            if (type == m_LangBeanName)
            {
                return LangBean.LangItSelf;
            }

            var beanInfo = ExcelDataMgr.Instance.GetBeanInfoByName(type);
            if (beanInfo != null)
            {
                return LangBean.BeanContainLang;
            }

            return LangBean.None;
        }

        private bool ValidateSheet(ExcelWorksheet sheet)
        {
            if (sheet.Name.Contains('#'))
            {
                return false;
            }

            bool hasVar = false;
            bool hasType = false;
            for (int i = 1; i <= 4; i++)
            {
                var value = GetValue(sheet, i, 1);
                if (value == "##var")
                {
                    hasVar = true;
                }

                if (value == "##type")
                {
                    hasType = true;
                }
            }

            if (hasVar && hasType)
            {
                return true;
            }

            return false;
        }

        private void ReadBeanExcel(string folderPath)
        {
            FileStream fileStream;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel;
            try
            {
                fileStream = new FileStream($"{folderPath}/__beans__.xlsx", FileMode.Open, FileAccess.Read,
                    FileShare.ReadWrite);
                excel = new ExcelPackage(fileStream);

                if (excel.Workbook.Worksheets.Count == 0)
                {
                    ErrorMsgMgr.Instance.AddErrorMsg($"__beans__.xlsx表里的sheet数量为0");
                    return;
                }

                foreach (var sheet in excel.Workbook.Worksheets)
                {
                    int fullNameIndex = GetBeanColIndexByName("full_name", sheet);
                    int sepIndex = GetBeanColIndexByName("sep", sheet);
                    int fieldsIndex = GetBeanColIndexByName("*fields", sheet);
                    int startIndex = GetBeanStartIndex(sheet);
                    if (startIndex == -1 || GetValue(sheet, startIndex, 2) == "")
                    {
                        ErrorMsgMgr.Instance.AddErrorMsg($"cannot find start index in __beans__.xlsx");
                        return;
                    }

                    int maxRowNum = sheet.Dimension.End.Row;
                    int row = startIndex;
                    while (row <= maxRowNum)
                    {
                        var lang = GetValue(sheet, row, fieldsIndex + 1);
                        if (lang == "")
                            break;

                        if (lang == m_LangBeanName && !GetValue(sheet, row, 1).Contains("##"))
                        {
                            int endIndex = row;

                            while (endIndex > 0)
                            {
                                var fullName = GetValue(sheet, endIndex, fullNameIndex);
                                var firstCol = GetValue(sheet, endIndex, 1);
                                if (!firstCol.Contains("##") && fullName.Length > 0)
                                {
                                    break;
                                }

                                --endIndex;
                            }

                            Bean bean;
                            string beanName = GetValue(sheet, endIndex, fullNameIndex);
                            bean.sep = GetValue(sheet, endIndex, sepIndex);
                            bean.index = row - endIndex;
                            ExcelDataMgr.Instance.AddBean(beanName, bean);
                        }

                        ++row;
                    }

                    break;
                }

                excel.Dispose();
                fileStream.Dispose();
            }
            catch (Exception e)
            {
                ErrorMsgMgr.Instance.AddErrorMsg($"读取__beans__.xlsx出错 \n详细信息:\n{e}\n\n");
            }
        }

        private int GetBeanColIndexByName(string name, ExcelWorksheet sheet)
        {
            int maxColumnNum = sheet.Dimension.End.Column;
            for (var col = 1; col <= maxColumnNum; col++)
            {
                if (name == GetValue(sheet, 1, col))
                {
                    return col;
                }
            }

            return -1;
        }

        private int GetBeanStartIndex(ExcelWorksheet sheet)
        {
            int maxRowNum = sheet.Dimension.End.Row;
            for (int row = 1; row <= maxRowNum; row++)
            {
                if (!GetValue(sheet, row, 1).Contains("##"))
                {
                    return row;
                }
            }

            return -1;
        }

        private int GetTypeColIndex(ExcelWorksheet sheet)
        {
            int maxRowNum = sheet.Dimension.End.Row;
            for (int row = 1; row <= maxRowNum; row++)
            {
                if (GetValue(sheet, row, 1).Contains("##type"))
                {
                    return row;
                }
            }

            return -1;
        }

        private List<string> GetColByIndex(int col, ExcelWorksheet sheet)
        {
            List<string> list = new List<string>();
            int maxRowNum = sheet.Dimension.End.Row;
            for (int row = 1; row <= maxRowNum; row++)
            {
                if (!GetValue(sheet, row, 1).Contains("##"))
                {
                    var lang = GetValue(sheet, row, col);
                    if (lang.Length > 0)
                    {
                        list.Add(lang);
                    }
                }
            }

            return list;
        }

        private List<string> GetLangInSideBean(ExcelWorksheet sheet, int col, string type, string fileName)
        {
            List<string> list = new List<string>();
            var beanInfo = ExcelDataMgr.Instance.GetBeanInfoByName(type);
            var beanType = validateBeanType(sheet, col);
            switch (beanType)
            {
                case BeanType.None:
                    ErrorMsgMgr.Instance.AddErrorMsg($"{fileName}表的 ##var配置有误 ");
                    break;
                case BeanType.Separate:
                    foreach (var bean in beanInfo)
                    {
                        var separateList = GetColByIndex(col + bean.index, sheet);
                        list.AddRange(separateList);
                    }

                    break;
                case BeanType.OneColumn:
                    var oneColumnList = GetLangInsideOneColumnBean(col, beanInfo, sheet);
                    list.AddRange(oneColumnList);
                    break;
            }

            return list;
        }

        private List<string> GetLangInsideOneColumnBean(int col, List<Bean> beanInfo, ExcelWorksheet sheet)
        {
            List<string> list = new List<string>();
            if (beanInfo.Count > 0)
            {
                int maxRowNum = sheet.Dimension.End.Row;
                for (int row = 1; row <= maxRowNum; row++)
                {
                    if (!GetValue(sheet, row, 1).Contains("##"))
                    {
                        var lang = GetValue(sheet, row, col);
                        if (lang.Length > 0)
                        {
                            var structInfo = lang.Split(beanInfo[0].sep);
                            foreach (var bean in beanInfo)
                            {
                                if (structInfo.Length > bean.index)
                                {
                                    list.Add(structInfo[bean.index]);
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }


        private BeanType validateBeanType(ExcelWorksheet sheet, int col)
        {
            int varTypeNum = 0;
            int varNum = 0;
            int maxRowNum = sheet.Dimension.End.Row;
            for (int row = 1; row <= maxRowNum; row++)
            {
                if (GetValue(sheet, row, 1).Length == 0)
                    break;
                if (GetValue(sheet, row, 1).Contains("##var"))
                {
                    varTypeNum++;
                    if (GetValue(sheet, row, col).Length > 0)
                    {
                        varNum++;
                    }
                }
            }

            if (varTypeNum == 2 && varNum == 2)
            {
                return BeanType.Separate;
            }

            if (varTypeNum >= 1 && varNum == 1)
            {
                return BeanType.OneColumn;
            }

            return BeanType.None;
        }

        private ExcelSheet ReadOtherSheet(ExcelWorksheet otherSheet)
        {
            if (otherSheet == null) return null;
            List<BaseExcelColumn> sheetColumns = new List<BaseExcelColumn>();
            try
            {
                int maxColumnNum = otherSheet.Dimension.End.Column;
                int maxRowNum = otherSheet.Dimension.End.Row;
                for (var col = 1; col <= maxColumnNum; col++)
                {
                    List<string> columnContent = new List<string>();
                    for (int row = 1; row <= maxRowNum; row++)
                    {
                        columnContent.Add(GetValue(otherSheet, row, col));
                    }

                    sheetColumns.Add(new NormalColumn(columnContent, col));
                }
            }
            catch
            {
                return null;
            }


            return new ExcelSheet(otherSheet.Name, sheetColumns);
        }

        private ExcelSheet ReadDataSheet(ExcelWorksheet dataSheet, string excelName)
        {
            if (dataSheet == null) return null;
            List<BaseExcelColumn> sheetColumns = new List<BaseExcelColumn>();
            int maxColumnNum;
            int maxRowNum;
            try
            {
                maxColumnNum = dataSheet.Dimension.End.Column;
                maxRowNum = dataSheet.Dimension.End.Row;
            }
            catch
            {
                return null;
            }

            for (var col = 2; col <= maxColumnNum; col++)
            {
                //一列column需要的数据
                List<string> columnContent = new List<string>();
                string fieldName = "";
                string aliasName = "";
                string dataType = "";

                StructColumn structColumn = null;
                for (var row = 1; row <= maxRowNum; row++)
                {
                    switch (GetValue(dataSheet, row, 1))
                    {
                        case "##comment":
                            aliasName = GetValue(dataSheet, row, col);
                            break;
                        case "##var":
                            if (fieldName.Length == 0)
                                fieldName = GetValue(dataSheet, row, col);
                            if (fieldName.Contains("*"))
                            {
                                structColumn = GetStructColumn(dataSheet, ref col, row, excelName);
                            }

                            break;
                        case "##type":
                            dataType = GetValue(dataSheet, row, col);
                            break;
                        case "##":
                            break;
                        case "":
                            columnContent.Add(GetValue(dataSheet, row, col));
                            break;
                    }

                    if (structColumn != null) break;
                }

                if (structColumn != null)
                {
                    sheetColumns.Add(structColumn);
                }
                else
                {
                    sheetColumns.Add(new DataColumn(fieldName, aliasName, dataType, columnContent, col));
                }
            }

            return new ExcelSheet(dataSheet.Name, sheetColumns);
        }

        private string GetValue(ExcelWorksheet firstWorksheet, int row, int col)
        {
            if (firstWorksheet.Cells[row, col].Value == null)
            {
                return "";
            }
            else
            {
                return firstWorksheet.Cells[row, col].Value.ToString();
            }
        }


        private StructColumn GetStructColumn(ExcelWorksheet curWorksheet, ref int col, int row, string excelName)
        {
            int startCol = col;
            List<DataColumn> structDataColumns = new List<DataColumn>();
            List<string> structColumnContent = new List<string>();
            string structFieldName = curWorksheet.Cells[row, col].Value.ToString();
            bool first = true;
            do
            {
                string fieldName = "";
                string aliasName = "";
                string dataType = "";
                List<string> columnContent = new List<string>();

                for (int structRow = 1; structRow <= curWorksheet.Dimension.End.Row; ++structRow)
                {
                    if (structRow == row) continue;
                    switch (GetValue(curWorksheet, structRow, 1))
                    {
                        case "##comment":
                            aliasName = GetValue(curWorksheet, structRow, col);
                            break;
                        case "##var":
                            fieldName = GetValue(curWorksheet, structRow, col);
                            break;
                        case "##type":
                            dataType = GetValue(curWorksheet, structRow, col);
                            break;
                        case "##":
                            break;
                        case "":
                            columnContent.Add(GetValue(curWorksheet, structRow, col));
                            if (first)
                            {
                                structColumnContent.Add((columnContent.Count).ToString());
                            }

                            break;
                    }
                }

                structDataColumns.Add(new DataColumn(fieldName, aliasName, dataType, columnContent, col));
                col++;
                first = false;
                if (col > curWorksheet.Dimension.End.Column) break;
            } while (GetValue(curWorksheet, row, col).Length == 0 &&
                     GetValue(curWorksheet, row + 1, col).Length > 0);

            col--;
            return new StructColumn(excelName, structFieldName, structColumnContent, startCol, structDataColumns);
        }

        // private Dictionary<string, ExcelStructSheet> GetAllStruct(ExcelWorksheet structSheet)
        // {
        //     Dictionary<string, ExcelStructSheet> allStructDic = new Dictionary<string, ExcelStructSheet>();
        //     int maxColumnNum = structSheet.Dimension.End.Column;
        //     int maxRowNum = structSheet.Dimension.End.Row;
        //
        //     int fullNameIndex = 0;
        //     int fieldNameIndex = 0;
        //     int typeIndex = 0;
        //     for (var col = 1; col <= maxColumnNum; col++)
        //     {
        //         switch (GetValue(structSheet, 1, col))
        //         {
        //             case "full_name":
        //                 fullNameIndex = col;
        //                 break;
        //             case "*fields":
        //                 fieldNameIndex = col;
        //                 typeIndex = col + 1;
        //                 break;
        //             default:
        //                 break;
        //         }
        //
        //         if (fullNameIndex != 0 && fullNameIndex != 0 && typeIndex != 0)
        //             break;
        //     }
        //
        //     for (var row = 1; row <= maxRowNum; row++)
        //     {
        //         string fullName = "";
        //         switch (GetValue(structSheet, row, 1))
        //         {
        //             case "":
        //                 fullName = structSheet.Cells[row, fullNameIndex].Value.ToString();
        //                 Dictionary<string, string> structDic = new Dictionary<string, string>();
        //                 do
        //                 {
        //                     structDic.Add(structSheet.Cells[row, fieldNameIndex].Value.ToString(),
        //                         structSheet.Cells[row, typeIndex].Value.ToString());
        //                     row++;
        //                 } while (structSheet.Cells[row, fullNameIndex].Value.ToString().Length == 0 &&
        //                          structSheet.Cells[row, fieldNameIndex].Value.ToString().Length > 0);
        //
        //                 allStructDic.Add(fullName, new ExcelStructSheet(fullName, structDic));
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        //
        //     return allStructDic;
        // }

        // private int GetTypeIndex(ExcelWorksheet curWorksheet)
        // {
        //     for (int col = 1; col <= curWorksheet.Dimension.End.Column; ++col)
        //     {
        //         if (curWorksheet.Cells[1, col].Value.ToString() == "##type")
        //         {
        //             return col;
        //         }
        //     }
        //     return -1;
        // }
    }
}