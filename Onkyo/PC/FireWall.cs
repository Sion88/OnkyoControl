using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetFwTypeLib;

namespace SionTool.PC
{
    class FireWall
    {
        public enum Protocol
        {
            TCP = 6,
            UDP = 17
        }
        static INetFwPolicy2 cv_INetFwPolicy2 { get { return (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"), false); } }
        static INetFwMgr cv_INetFwMgr{ get { return (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr", false)); } }
        public static void AddRule(string m_Name, int[] m_Port, Protocol m_Protocol = Protocol.TCP)
        {
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule", false));
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRule.Description = "ToolSet";
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN; // inbound
            firewallRule.Protocol = (int)m_Protocol;
            firewallRule.Enabled = true;
            firewallRule.InterfaceTypes = "All";
            firewallRule.LocalPorts = string.Join(",", m_Port);
            firewallRule.Name = m_Name;
            cv_INetFwPolicy2.Rules.Add(firewallRule);

        }
        public static bool ControlRule(string m_Name, bool m_Open)
        {
            INetFwRule Temp = null;
            try
            {
                Temp = cv_INetFwPolicy2.Rules.Item(m_Name);
            }
            catch
            {
                return false;
            }
            Temp.Enabled = m_Open;
            Console.WriteLine(string.Join(" ", new string[] { Temp.Name, Temp.Protocol.ToString(), Temp.serviceName, Temp.LocalPorts, Temp.EdgeTraversal.ToString() }));
            return true;

        }
        public static void RemoveRule(string m_Name)
        {
            cv_INetFwPolicy2.Rules.Remove(m_Name);
        }
        public static bool IsOpen
        {
            get { return cv_INetFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled; }
        }
    }
}
