using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SionTool.PC
{

    public class UIControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, StringBuilder lParam);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder Name, int count);
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public class UIInfo
        {
            public IntPtr intPtr;
            public string WindowText;
            public string ClassName;
            public Dictionary<IntPtr, UIInfo> ChildUI;
        }
        public const int WM_SETTEXT = 0x000C;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int BM_CLICK = 0x00F5;

        public static UIInfo GetUIInfo(IntPtr m_UI)
        {
            if (m_UI == IntPtr.Zero)
                return null;
            StringBuilder ClassName = new StringBuilder();
            var aaa = GetClassName(m_UI, ClassName, 1024);
            StringBuilder WindowText = new StringBuilder();
            var bbb = GetWindowText(m_UI, WindowText, 1024);
            return new UIInfo()
            {
                ClassName = ClassName.ToString(),
                WindowText = WindowText.ToString(),
                intPtr = m_UI
            };
        }
        public static List<Process> NowUIProcess
        {
            get { return Process.GetProcesses().Where((x) => { return x.MainWindowHandle != IntPtr.Zero; }).ToList(); }
        }
        public static List<IntPtr> GetUIChild(IntPtr m_IntPtr)
        {
            List<IntPtr> Re = new List<IntPtr>();
            EnumWindowProc childProc = new EnumWindowProc((x, y) => {
                Re.Add(x);
                return true;
            });
            EnumChildWindows(m_IntPtr, childProc, IntPtr.Zero);
            return Re;
        }
        private static List<UIInfo> ChildFormat(UIInfo m_UIInfo)
        {
            List<UIInfo> Re = new List<UIInfo>();
            foreach (var I in m_UIInfo.ChildUI.Select(x => { return x.Value; }).ToArray())
            {
                if (m_UIInfo.ChildUI.ContainsValue(I))
                {
                    var One = I;
                    var Child = GetUIChild(One.intPtr);
                    if (Child.Count > 0)
                    {
                        One.ChildUI = new Dictionary<IntPtr, UIInfo>();
                        foreach (var J in Child)
                        {
                            One.ChildUI.Add(J, m_UIInfo.ChildUI[J]);
                            m_UIInfo.ChildUI.Remove(J);
                        }
                        ChildFormat(One);
                    }
                }
            }
            return Re;
        }
        public static List<UIInfo> GetChildUIInfo(IntPtr m_IntPtr, bool m_Format = false)
        {

            List<UIInfo> Re = new List<UIInfo>();

            foreach (var I in GetUIChild(m_IntPtr))
            {
                Re.Add(GetUIInfo(I));
            }
            if (m_Format)
            {
                var main = GetUIInfo(m_IntPtr);
                main.ChildUI = Re.ToDictionary(v => v.intPtr, v => v);
                ChildFormat(main);
                Re = new List<UIInfo>() {
                    main
                };

            }
            return Re;
        }
        public static void Demo()
        {

            foreach (var I in NowUIProcess)
            {
                Console.WriteLine(I.ProcessName);
            }
            #region 修改預設聲音
            Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL mmsys.cpl");
            Thread.Sleep(500);
            IntPtr SoundWindows = FindWindow(null, "聲音");
            IntPtr ConfirmBtn = FindWindowEx(SoundWindows, IntPtr.Zero, "Button", "確定");
            IntPtr 播放視窗 = FindWindowEx(SoundWindows, IntPtr.Zero, null, "播放");
            IntPtr U = FindWindowEx(播放視窗, IntPtr.Zero, "SysListView32", "");
            IntPtr defultSettingBtn = FindWindowEx(播放視窗, IntPtr.Zero, "Button", "設為預設值(&S)");
            //SendMessage(U, WM_KEYDOWN, VK_DOWN, IntPtr.Zero);
            //SendMessage(U, WM_KEYUP, VK_DOWN, IntPtr.Zero);
            KeyboardClick(U, Keys.Down);
            KeyboardClick(defultSettingBtn, Keys.Down);
            SendMessage(defultSettingBtn, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            #endregion
        }

        public static void Demo2()
        {

            foreach (var I in NowUIProcess)
            {
                Console.WriteLine(I.ProcessName);
            }
            #region 修改預設聲音
            Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL mmsys.cpl");
            Thread.Sleep(500);
            IntPtr SoundWindows = FindWindow(null, "聲音");
            IntPtr ConfirmBtn = FindWindowEx(SoundWindows, IntPtr.Zero, "Button", "確定");
            IntPtr 播放視窗 = FindWindowEx(SoundWindows, IntPtr.Zero, null, "播放");
            IntPtr U = FindWindowEx(播放視窗, IntPtr.Zero, "SysListView32", "");
            IntPtr defultSettingBtn = FindWindowEx(播放視窗, IntPtr.Zero, "Button", "設為預設值(&S)");
            IntPtr ContentBtn = FindWindowEx(播放視窗, IntPtr.Zero, "Button", "內容(&P)");
            KeyboardClick(U, Keys.Down);
            new Thread(() => { SendMessage(ContentBtn, BM_CLICK, IntPtr.Zero, IntPtr.Zero); }).Start();
            Thread.Sleep(500);
            IntPtr AV = FindWindow(null, "AV Receiver - 內容");
            IntPtr TabControl = FindWindowEx(AV, IntPtr.Zero, "SysTabControl32", "");
            KeyboardClick(TabControl, Keys.Right);
            KeyboardClick(TabControl, Keys.Right);
            var BalanceBtn = GetChildUIInfo(AV).Where(x => { if (x.ClassName == "Button" && x.WindowText == "平衡(&B)") return true; return false; }).ToArray()[0].intPtr;
            new Thread(() => { SendMessage(BalanceBtn, BM_CLICK, IntPtr.Zero, IntPtr.Zero); }).Start();
            Thread.Sleep(500);
            IntPtr Balance = FindWindow(null, "平衡");
            var ALLCH = GetChildUIInfo(Balance).Where(x => { if (x.ClassName == "msctls_trackbar32" && x.WindowText == "") return true; return false; }).ToArray();
            // WriteText(ALLCH[0].intPtr, "0");
            for (var i = 0; i < 100; i++)
                KeyboardClick(ALLCH[1].intPtr, Keys.Left);

            for (var i = 0; i < 100; i++)
                KeyboardClick(ALLCH[0].intPtr, Keys.Right);
            KeyboardClick(ALLCH[0].intPtr, Keys.Left);

            #endregion
        }

        public static void WriteText(IntPtr m_UI, string m_Str)
        {
            SendMessage(m_UI, WM_SETTEXT, 0, m_Str);
        }
        public static void KeyboardClick(IntPtr m_UI, Keys m_Key)
        {
            SendMessage(m_UI, WM_KEYDOWN, (int)m_Key, IntPtr.Zero);
            SendMessage(m_UI, WM_KEYUP, (int)m_Key, IntPtr.Zero);
        }
    }
}
