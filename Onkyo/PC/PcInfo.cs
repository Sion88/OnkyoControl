using System;
using System.Management;

namespace SionTool.PC
{
    public class PcInfo
    {
        public static string GetAllPcInfo()
        {
            return GetPcInfo("Win32_BaseBoard", "SerialNumber") + ";" +
              GetPcInfo("Win32_Processor", "ProcessorId") + ";" +
              GetPcInfo("Win32_PhysicalMedia  WHERE SerialNumber!=NULL", "SerialNumber") + ";" +
              GetPcInfo("Win32_NetworkAdapter WHERE((MACAddress Is Not NULL) AND(Manufacturer <> 'Microsoft'))", "MACAddress");
        }
        public static string GetPcInfo(string Item, string Property)
        {
            string Re = "";
            var all = new ManagementObjectSearcher("Select * from " + Item);
            foreach (var i in all.Get())
            {
                if (Re.Length > 0)
                    Re += ",";
                Re += Convert.ToString(i[Property]).Trim();
            }
            return Re;
        }
    }
}
