using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Onkyo
{
    class OnkyoController
    {
        Socket cv_socket = null;
        Thread cv_Thread;
        public string Ip { get; set; }
        public string cv_NowVol { get; set; } = "50";
        public int Port { get; set; } = 60128;
        public bool IsOpen { get; private set; } = false;
        public delegate void Recieved(string str);
        public event Recieved OnRecieved;
        public delegate void VolChanged(int str);
        public event VolChanged OnVolChanged;
        readonly byte[] PacketTemplate = new byte[]
        {
                0x49, 0x53, 0x43, 0x50,
                0x00, 0x00, 0x00, 0x10,
                0x00, 0x00, 0x00, 0xFF,
                0x01, 0x00, 0x00, 0x00
        };
        readonly byte EOF = 0x0D;

        public OnkyoController(string m_Ip)
        {
            Ip = m_Ip;
        }

        public bool Open()
        {
            if (IsOpen || Ip == null)
                return false;
            IsOpen = true;
            cv_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000 };
            try
            {
                cv_socket.Connect(Ip, Port);
            }
            catch
            {
                return false;
            }
            cv_Thread = new Thread(ConnectProcess);
            cv_Thread.Start();
            return true;
        }
        private void ConnectProcess() {
            Volume_Read();
            while (IsOpen)
            {
                if (cv_socket == null)
                    break;
                if (cv_socket.Available > 0)
                {

                    var buffer = new byte[10240];
                    cv_socket.Receive(buffer, buffer.Length, SocketFlags.None);
                    var builder = new StringBuilder();
                    for (int i = 16; buffer[i] != 26; i++)
                    {
                        if (buffer[i] != 26)
                        {
                            int num = Convert.ToInt32(string.Format("{0:x2}", buffer[i]), 16);
                            builder.Append(char.ConvertFromUtf32(num));
                        }
                    }
                    var Str = builder.ToString();
                    if (Str.Contains("!1MVL"))
                    {
                        cv_NowVol = (int.Parse(Str.Replace("!1MVL", ""), System.Globalization.NumberStyles.HexNumber) / 2).ToString();
                        if (OnVolChanged != null)
                            OnVolChanged(int.Parse(cv_NowVol));
                    }
                    if (OnRecieved != null)
                        OnRecieved(Str);
                }
                Thread.Sleep(300);
            }
        }
        public void Close()
        {

            if (!IsOpen)
                return;
            IsOpen = true;
            cv_socket.Close();
            cv_socket.Dispose();
            cv_socket = null;
            cv_Thread = null;
        }
        private void Cmd(string Command)
        {
            if (!IsOpen)
                return;
            try
            {

                var cmdBytes = Encoding.ASCII.GetBytes(Command);
                List<byte> ret = PacketTemplate.ToList();
                //var temp=byte.Parse(string.Format("{0:X2}", (cmdBytes.Length + 1).ToString()), NumberStyles.HexNumber);
                var temp=BitConverter.GetBytes(cmdBytes.Length + 1);
                ret[11] = temp[0]; 
                ret.AddRange(cmdBytes);
                ret.Add(EOF);
                var b = ret.ToArray();
                cv_socket.Send(b, 0, b.Length, SocketFlags.None);
                Thread.Sleep(100);
            }
            catch (Exception x)
            {

            }
        }

        public void Power_On()
        {
            Cmd("!1PWR01");
        }
        public void Power_Off()
        {
            Cmd("!1PWR00");
        }

        public void Volume_Down()
        {
            Cmd("!1MVLDOWN1");            
        }
        public void Volume_UP()
        {
            Cmd("!1MVLUP1");
        }
        private void Volume_Read()
        {
            Cmd("!1MVLQSTN");
        }
        public void Volume(int Vol)
        {
            if (Vol < 0)
                Vol = 0;
            if (Vol > 60)
                Vol = 60;
            Cmd("!1MVL" + string.Format("{0:X2}", Vol * 2));
        }

        public void Input_DVD()
        {
            Cmd("!1SLI10");
        }
        public void Input_CBL_SAT()
        {
            Cmd("!1SLI01");
        }
        public void Input_Game()
        {
            Cmd("!1SLI02");
        }
        public void Input_AUX1()
        {
            Cmd("!1SLI03");
        }
        public void Input_PC()
        {            
            Cmd("!1SLI05");
        }
        public void Output_PureAudio()
        {            
            Cmd("!1LMD11");
        }
        public void Output_THXCinema()
        {            
            Cmd("!1LMD42");
        }
        public void Output_AllChStereo()
        {            
            Cmd("!1LMD0C");
        }
        public void Output_DTSNeural()
        {            
            Cmd("!1LMD82");
        }
        public void Output_DolbySurround()
        {            
            Cmd("!1LMD80");
        }
        public void GetInfo()//in Receive(!1IFAHDMI 5,Multich PCM,48 kHz,7.1 ch B,Pure Audio,5.1 ch,)
        {            
            Cmd("!1IFAQSTN");
        }
    }
}