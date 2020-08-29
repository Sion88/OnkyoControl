using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SionTool.PC
{
    /// <summary>
    /// 管理目前有的程式，Focuse、改變程式顯示方式
    /// </summary>
    public class WindowControl
    {
        public static IntPtr NullMemory = new IntPtr(0);
        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, WindowSize nCmdShow);
        /// <summary>
        /// 畫面大小
        /// </summary>
        public enum WindowSize : int
        {
            Hide = 0,
            Normal = 1,
            Max = 3,
            ShowNotActivate = 4,
            Show = 5,
            Min = 6,
            ReStore = 9,
            ShowDefault = 10
        }
        /// <summary>
        /// Focuse程式
        /// </summary>
        /// <param name="sad">Process編號</param>
        public static void FocuseProcess(IntPtr sad)
        {
            SetForegroundWindow(sad);
            SetActiveWindow(sad);
        }
        /// <summary>
        /// Focuse程式
        /// </summary>
        /// <param name="ProcessName">程式名稱與工作管理員一樣</param>
        public static void FocuseProcessByName(string ProcessName)
        {
            FocuseProcess(GetWithWindowsProcess(ProcessName));
        }
        /// <summary>
        /// 取得當前運行程式的Process編號
        /// </summary>
        /// <param name="ProcessName">程式名稱與工作管理員一樣</param>
        public static IntPtr GetWithWindowsProcess(String ProcessName)
        {
            var prc = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            if (prc.Length > 0)
                foreach (var one in prc)
                    if (one.MainWindowHandle != NullMemory)
                        return one.MainWindowHandle;
            return NullMemory;
        }
        /// <summary>
        /// 改變程式顯示方式
        /// </summary>
        /// <param name="hWnd">Process編號</param>
        /// <param name="nCmdShow">顯示方式</param>
        public static void ChangeProcessWindowSize(IntPtr hWnd, WindowSize nCmdShow)
        {
            ShowWindowAsync(hWnd, nCmdShow);
        }
        /// <summary>
        /// 改變程式顯示方式
        /// </summary>
        /// <param name="ProcessName">程式名稱與工作管理員一樣</param>
        /// <param name="nCmdShow">顯示方式</param>
        public static void ChangeProcessWindowSizeByName(string ProcessName, WindowSize nCmdShow)
        {
            ShowWindowAsync(GetWithWindowsProcess(ProcessName), nCmdShow);
        }
        /// <summary>
        /// 改變當下Focuse程式的顯示方式
        /// </summary>
        /// <param name="nCmdShow">顯示方式</param>
        public static void ChangeNowWindowSize(WindowSize nCmdShow)
        {
            ShowWindowAsync(GetForegroundWindow(), nCmdShow);
        }
    }
}
