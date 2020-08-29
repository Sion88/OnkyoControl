using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SionTool.PC
{
    public class RegistryProcess
    {
        public static void SetRegistry(string m_KeyName, string m_KeyValue,string m_Path= @"HKEY_CURRENT_USER\Keyboard Layout\DATA")
        {
            string Key = m_Path.Split('\\').LastOrDefault();
            RegistryKey Reg = Registry.CurrentUser.OpenSubKey("Software", true);
            var aasd = Reg.GetSubKeyNames();
            if (!Reg.GetSubKeyNames().Contains(Key))
            {
                Reg.CreateSubKey(Key);
            }
                Microsoft.Win32.Registry.SetValue(m_Path, m_KeyName, m_KeyValue, RegistryValueKind.String);
            Reg.Close();
        }

        public static string GetRegistry(string m_KeyName, string m_Path = @"HKEY_CURRENT_USER\Keyboard Layout\DATA")
        {
            try
            {
                return Convert.ToString(Registry.GetValue(m_Path, m_KeyName, ""));
            }
            catch
            {
                return string.Empty;
            }
        }
        public static void DelRegistry(string m_Path = @"HKEY_CURRENT_USER\Keyboard Layout\DATA")
        {
            string Key = m_Path.Split('\\').LastOrDefault();
            RegistryKey Reg = Registry.CurrentUser.OpenSubKey("Software", true);
            Reg.DeleteSubKey(Key);
            Reg.Close();
        }
    }
}
