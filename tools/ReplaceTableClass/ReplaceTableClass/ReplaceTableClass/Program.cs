using System;
using System.IO;
using System.Text.RegularExpressions;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            // string filePath =
            //     @"D:\Kop\GitWorkSpace\client\Unity\Assets\Scripts\hotfix\Main\Codes\Generate\Configs\Tables.cs";
            string filePath = args[0];

            // 1. Read the file into a string
            string fileContent = File.ReadAllText(filePath);

            // 2. Replace the first pattern
            string pattern1 =
                @"LanguageTable(\w+)\s*=\s*new\s+LanguageTable_(\w+)\(loader\(""languagetable_(\w+)""\)\);\s*";
            string replacement1 = @"";
            fileContent = Regex.Replace(fileContent, pattern1, replacement1);

            // 3. Replace the second pattern
            string pattern2 = @"public\s+LanguageTable_(\w+)\s+LanguageTable(\w+)\s+\{\s*get;\s*\}\s*";
            string replacement2 = @"";
            fileContent = Regex.Replace(fileContent, pattern2, replacement2);

            string pattern3 = @"LanguageTable\w+\.ResolveRef\(this\);\s*";
            string replacement3 = @"";
            fileContent = Regex.Replace(fileContent, pattern3, replacement3);

            // 4. Write the modified string back to the file
            File.WriteAllText(filePath, fileContent);
            PrintSuccessMsg("成功删除table.cs language定义");
        }
        catch
        {
            PrintErrorMsg("删除table.cs language定义错误  ");
            Environment.ExitCode = 3;
        }
    }

    public static void PrintSuccessMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void PrintErrorMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}