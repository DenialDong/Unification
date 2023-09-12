using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing;
using System.Drawing;
using Newtonsoft.Json;
using OfficeOpenXml.Drawing.Chart.Style;


namespace ConvertExcel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // var folderPath =
            //     @"D:\Kop\GitWorkSpace\client\Unity\Assets\Scripts\Editor\KopEditor\TranslationLanguage";
            // var targetPath =
            //     @"D:\Kop\GitWorkSpace\configs\LanguageTable";
            var folderPath = args[0];
            var targetPath = args[1];
            ReadExcel.Instance.ReadFolder(folderPath, targetPath);
            // await WriteExcel.Instance.WriteToFolder(folderPath);
            ErrorMsgMgr.Instance.AutoPrintErrorOrSucces();
        }
    }
}