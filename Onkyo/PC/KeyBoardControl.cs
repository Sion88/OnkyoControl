using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SionTool.PC
{

    /// <summary>
    /// 鍵盤控制
    /// </summary>
    public class KeyBoardControl
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, KeyBoardEventFlag dwFlags, IntPtr dwExtraInfo);
        private enum KeyBoardEventFlag : uint
        {
            PageUpKey = 0x21,
            UpArrow = 0x28,
            KeyDown = 0x01,
            KeyUp = 0x02,
            CtrlKey = 0x11,
            CKey = 0x43,
            EnterKey = 0x0D,
            TabKey = 0x09
        }
        public void KeyDown(Keys key)
        {
            lock (this)
                keybd_event((byte)key, 0, KeyBoardEventFlag.KeyDown, (IntPtr)0);
        }
        public void KeyUp(Keys key)
        {
            lock (this)
                keybd_event((byte)key, 0, KeyBoardEventFlag.KeyUp, (IntPtr)0);
        }
        public void KeyClick(Keys key)
        {
            lock (this)
            {
                KeyDown(key);
                Thread.Sleep(40);
                KeyUp(key);
            }
        }
    }
}
