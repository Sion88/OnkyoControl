using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onkyo
{

    public class Config
    {
        private SIni cv_Sini;
        public string OnkyoIP { get; }

        public Config(string m_FileName)
        {
            cv_Sini = new SIni(m_FileName);
            OnkyoIP = cv_Sini.GetKeyValue("Base", "OnkyoIP", "192.168.2.3");
        }      
    }
    public static class Extensions
    {
        public static int ToInt(this string m_Value)
        {
            int Re = 0;
            int.TryParse(m_Value, out Re);
            return Re;
        }
    }

}
