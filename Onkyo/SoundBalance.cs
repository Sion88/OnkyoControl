using SionTool;
using SionTool.PC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onkyo
{
    public partial class SoundBalance : Form
    {
        IntPtr cv_Left;
        IntPtr cv_Right;
        Thread cv_CheckThread;
        public SoundBalance()
        {
            
           
            cv_CheckThread = new Thread(() => {

                killProcess("Rundll32");
                while (true) {
                    if(Process.GetProcessesByName("Rundll32").Length==0)
                        if (!GetLeftRight())
                        {
                            this.Invoke(new Action(() => {
                                MessageBox.Show("無法正確啟動調音");
                                this.Close();
                            }));
                        }
                    Thread.Sleep(5000);
                }
            });
            cv_CheckThread.Start();
            InitializeComponent();
            Show();
            this.TopMost = true;
        }
        
        protected override void OnClosed(EventArgs e)
        {
            try { AllBtn_Click(null, null); } catch { }
            if (cv_CheckThread != null)
                cv_CheckThread.Abort();
                killProcess("Rundll32");
            base.OnClosed(e);
        }
        public bool GetLeftRight() {

            try
            {

                Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL mmsys.cpl");
                Thread.Sleep(500);
                IntPtr SoundWindows = UIControl.FindWindow(null, "聲音");
                IntPtr ConfirmBtn = UIControl.FindWindowEx(SoundWindows, IntPtr.Zero, "Button", "確定");
                IntPtr 播放視窗 = UIControl.FindWindowEx(SoundWindows, IntPtr.Zero, null, "播放");
                IntPtr U = UIControl.FindWindowEx(播放視窗, IntPtr.Zero, "SysListView32", "");
                IntPtr ContentBtn = UIControl.FindWindowEx(播放視窗, IntPtr.Zero, "Button", "內容(&P)");
                UIControl.KeyboardClick(U, Keys.Down);
                new Thread(() => { UIControl.SendMessage(ContentBtn, UIControl.BM_CLICK, IntPtr.Zero, IntPtr.Zero); }).Start();
                Thread.Sleep(500);
                IntPtr AV = UIControl.FindWindow(null, "AV Receiver - 內容");
                if (AV == IntPtr.Zero)
                    return false;
                IntPtr TabControl = UIControl.FindWindowEx(AV, IntPtr.Zero, "SysTabControl32", "");
                UIControl.KeyboardClick(TabControl, Keys.Right);
                UIControl.KeyboardClick(TabControl, Keys.Right);
                var BalanceBtn = UIControl.GetChildUIInfo(AV).Where(x => { if (x.ClassName == "Button" && x.WindowText == "平衡(&B)") return true; return false; }).ToArray()[0].intPtr;
                new Thread(() => { UIControl.SendMessage(BalanceBtn, UIControl.BM_CLICK, IntPtr.Zero, IntPtr.Zero); }).Start();
                Thread.Sleep(500);
                IntPtr Balance = UIControl.FindWindow(null, "平衡");
                if (Balance == IntPtr.Zero)
                    return false;
                var ALLCH = UIControl.GetChildUIInfo(Balance).Where(x => { if (x.ClassName == "msctls_trackbar32" && x.WindowText == "") return true; return false; }).ToArray();
                cv_Left = ALLCH[0].intPtr;
                cv_Right = ALLCH[1].intPtr;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void killProcess(string m_ProcessName)
        {
            try
            {
                Process[] proc = Process.GetProcessesByName(m_ProcessName);
                proc[0].Kill();
            }
            catch (Exception e)
            {
            }
        }
        private void LeftBtn_Click(object sender, EventArgs e)
        {

            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Left, Keys.Right);

            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Right, Keys.Left);
        }

        private void RightBtn_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Right, Keys.Right);

            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Left, Keys.Left);
        }

        private void AllBtn_Click(object sender, EventArgs e)
        {

            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Left, Keys.Right);
            for (var i = 0; i < 100; i++)
                UIControl.KeyboardClick(cv_Right, Keys.Right);
        }
    }
}
