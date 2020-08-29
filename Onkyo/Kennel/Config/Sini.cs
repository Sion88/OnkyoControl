using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Onkyo
{
    public class SIni : IDisposable
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string cv_FileName;

        /// <summary> 
        /// 建構子。 
        /// </summary> 
        /// <param name="path">檔案路徑，相對路徑，不需.INI</param>      
        public SIni(string path = @"\Config")
        {
            cv_FileName = Path.GetFullPath(path);        
        }

        /// <summary> 
        /// 釋放資源(程式設計師呼叫)。 
        /// </summary> 
        public void Dispose()
        {
            GC.SuppressFinalize(this); //要求系統不要呼叫指定物件的完成項。 
        }

        /// <summary> 
        /// 設定 KeyValue 值。 
        /// </summary> 
        /// <param name="m_Section">Section。</param> 
        /// <param name="m_Key">Key。</param> 
        /// <param name="m_Value">Value。</param> 
        public void SetKeyValue(string m_Section, string m_Key, string m_Value)
        {
            WritePrivateProfileString(m_Section, m_Key, m_Value, this.cv_FileName);
        }

        /// <summary> 
        /// 取得 Key 相對的 Value 值，若沒有則使用預設值(DefaultValue)。 
        /// </summary> 
        /// <param name="m_Section">Section。</param> 
        /// <param name="m_Key">Key。</param> 
        /// <param name="m_DefaultValue">DefaultValue。</param>        
        public string GetKeyValue(string m_Section, string m_Key, string m_DefaultValue, int m_Size = 255)
        {
            StringBuilder sbResult = null;

            try
            {
                sbResult = new StringBuilder(m_Size);

                GetPrivateProfileString(m_Section, m_Key, "", sbResult, m_Size, this.cv_FileName);

                return (sbResult.Length > 0) ? sbResult.ToString() : m_DefaultValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
