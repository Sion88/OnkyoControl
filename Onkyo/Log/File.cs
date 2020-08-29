using System;
using System.IO;

namespace SionTool.Flie
{
    public class SFile
    {
      
        /// <summary>
        /// 建立資料夾，會判斷是否存在、有沒有填路徑
        /// </summary>
        /// <param name="m_Path">相對路徑</param>
        public static void CreateForder(string m_Path)
        {
            if (!string.IsNullOrWhiteSpace(m_Path))
            {
                bool exists = System.IO.Directory.Exists(m_Path);
                if (!exists)
                    System.IO.Directory.CreateDirectory(m_Path);
            }
        }
        public static void WriteFile(string m_Path,string m_FileName,string Content)
        {
            CreateForder(m_Path);
            using (StreamWriter sw = new StreamWriter(m_Path+m_FileName))   
            {
                sw.WriteLine(Content);
            }
        }

        /// <summary>
        /// 開啟其他程式
        /// </summary>
        /// <param name="_Path">相對路徑</param>
        public static void Run(String _Path) {
            try
            {
                string str = Path.GetFullPath(_Path);
                System.Diagnostics.Process.Start(str);
            }
            catch (Exception e)
            { throw e; }
        }
        public static string ReadAllText(String Path)
        {
            try
            {
                return System.IO.File.ReadAllText(Path);
            }
            catch { throw new Exception("讀取失敗!!可能有其他程式正在使用。"); }
        } 
    }
}
