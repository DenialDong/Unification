using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ConvertExcel
{
    public struct Bean
    {
        public string sep;
        public int index;
    }

    public sealed class ExcelDataMgr : Singleton<ExcelDataMgr>
    {
        private Dictionary<string, ExcelBook> m_ExcelBooksDic = new Dictionary<string, ExcelBook>();
        private Dictionary<string, List<string>> m_LangDic = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Bean>> m_BeanDic = new Dictionary<string, List<Bean>>();
        private Dictionary<string, object> m_JSONDic = new Dictionary<string, object>();
        public void AddExcelBook(string excelName, ExcelBook excelBook)
        {
            if (m_ExcelBooksDic.ContainsKey(excelName))
            {
                m_ExcelBooksDic[excelName] = excelBook;
            }
            else
            {
                m_ExcelBooksDic.Add(excelName, excelBook);
            }
        }

        public void AddLang(string excelName, List<string> langList)
        {
            if (m_LangDic.ContainsKey(excelName))
            {
                m_LangDic[excelName] = langList;
            }
            else
            {
                m_LangDic.Add(excelName, langList);
            }
        }

        public void AddBean(string beanName, Bean bean)
        {
            if (m_BeanDic.ContainsKey(beanName))
            {
                m_BeanDic[beanName].Add(bean);
            }
            else
            {
                var beanList = new List<Bean>();
                beanList.Add(bean);
                m_BeanDic.Add(beanName, beanList);
            }
        }

        public List<Bean> GetBeanInfoByName(string beanName)
        {
            if (m_BeanDic.ContainsKey(beanName))
            {
                return m_BeanDic[beanName];
            }

            return null;
        }

        public void PrintBean()
        {
            foreach (var beanName in m_BeanDic)
            {
                Console.WriteLine(beanName.Key);
                foreach (var Lang in beanName.Value)
                {
                    Console.WriteLine($"-- index:{Lang.index}  sep: {Lang.sep}");
                }
            }
        }
        
        public void PrintLang()
        {
            foreach (var langInfo in m_LangDic)
            {
                Console.WriteLine(langInfo.Key);
                foreach (var lang in langInfo.Value)
                {
                    Console.WriteLine($"-- {lang}");
                }
            }
        }
        public HashSet<string> GetLangList()
        {
            List<string> list = new List<string>();
            foreach (var langList in m_LangDic.Values)
            {
                list.AddRange(langList);
            }

            HashSet<string> set = new HashSet<string>(list);
            return set;
        }

        public ExcelBook GetExcelBookByExcelName(string excelName)
        {
            if (m_ExcelBooksDic.ContainsKey(excelName))
            {
                return m_ExcelBooksDic[excelName];
            }

            return null;
        }

        public Dictionary<string, ExcelBook> GetExcelBooks()
        {
            return m_ExcelBooksDic;
        }
    }
}