using Gma.System.MouseKeyHook;
using SionTool.Log;
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
    public partial class Form1 : Form
    {
        ShowText cv_SoundNotify = new ShowText();
        private IKeyboardMouseEvents cv_GlobalHook;
        NotifyIcon cv_Notify;
        OnkyoController cv_Con;
        public Form1()
        {
            InitializeComponent();
            this.Show();
            this.Hide();
            cv_Notify = CreateNotifyIcon();
            cv_Notify.Visible = true;
            this.SizeChanged += Form1_SizeChanged;
            cv_Con =new OnkyoController(Program.g_Config.OnkyoIP);
            cv_Con.OnVolChanged += Con_OnVolChanged;
            cv_Con.OnRecieved += Con_OnRecieved;
            cv_GlobalHook = Hook.GlobalEvents();
            cv_GlobalHook.KeyDown += Cv_GlobalHook_KeyDown;
            var asd =cv_Con.Open();
            radioButton1.CheckedChanged += Radio_Changed;
            radioButton2.CheckedChanged += Radio_Changed;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;

        }
 
        private Boolean IsControl = false;
        private void Cv_GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (Control.ModifierKeys == Keys.Control)
                        Vol_UpBtn_Click(sender, e);
                    break;
                case Keys.Down:
                    if (Control.ModifierKeys== Keys.Control)
                        Vol_DownBtn_Click(sender, e);
                    break;
                case Keys.Left:
                    if (Control.ModifierKeys == Keys.Control)
                        OpenBtn_Click(sender, e);
                    break;
                case Keys.Right:

                    if (Control.ModifierKeys == Keys.Control)
                        CloseBtn_Click(sender, e);
                    break;
                case Keys.NumPad0:
                    if (Control.ModifierKeys == Keys.Control)
                        PC_Click(sender, e);
                    break;
                case Keys.NumPad1:
                    if (Control.ModifierKeys == Keys.Control)
                        Game_Click(sender, e);
                    break;
                case Keys.NumPad4:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        cv_Con.Output_PureAudio();
                        cv_SoundNotify.setVal("PureAudio");
                    }
                    break;
                case Keys.NumPad5:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        cv_Con.Output_THXCinema();
                        cv_SoundNotify.setVal("THX");
                    }
                    break;
                case Keys.NumPad6:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        cv_Con.Output_DolbySurround();
                        cv_SoundNotify.setVal("DolbySurround");
                    }
                    break;
                case Keys.NumPad7:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        cv_Con.Output_DTSNeural();
                        cv_SoundNotify.setVal("DTSNeural");
                    }
                    break;
                case Keys.NumPad8:
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        cv_Con.Output_AllChStereo();
                        cv_SoundNotify.setVal("AllChStereo");
                    }
                    break;

                case Keys.NumPad9:
                    if (Control.ModifierKeys == Keys.Control)
                        CloseBtn_Click(sender, e);
                    break;
            }
        }

        private void Con_OnRecieved(string str)
        {
            Console.WriteLine(str);
        }

        private void Con_OnVolChanged(int str)
        {
            var Work = new Action(() =>
              {
                  cv_SoundNotify.setVal(str.ToString());
                  numericUpDown1.Value = str;
              });
            if (this.InvokeRequired)
                this.Invoke(Work);
            else
                Work();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cv_Con.Volume((int)numericUpDown1.Value);
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
        public void Close() {
            cv_Notify.Visible = false;
            cv_Notify.Dispose();
            cv_Con.Close();
            Process.GetCurrentProcess().Kill();
        }
        public NotifyIcon CreateNotifyIcon() {
            NotifyIcon Re = new NotifyIcon(new Container());
            Re.ContextMenu = CreateContextMenu();
            Re.Text = "Onkyo";
            Re.Icon = Properties.Resources.Onkyo;
            Re.DoubleClick += Show_Click;
            return Re;
        }
        public ContextMenu CreateContextMenu() {
            ContextMenu Re = new ContextMenu();
            MenuItem Open = new MenuItem();
            Open.Text = "Open";
            Open.Click += OpenBtn_Click;

            MenuItem Close = new MenuItem();
            Close.Text = "Close";
            Close.Click += CloseBtn_Click;

            MenuItem Vol_Up = new MenuItem();
            Vol_Up.Text = "Vol_Up";
            Vol_Up.Click += Vol_UpBtn_Click;

            MenuItem Vol_Down = new MenuItem();
            Vol_Down.Text = "Vol_Down";
            Vol_Down.Click += Vol_DownBtn_Click;


            MenuItem PC = new MenuItem();
            PC.Text = "PC";
            PC.Click += PC_Click;

            MenuItem Game = new MenuItem();
            Game.Text = "Game";
            Game.Click += Game_Click;


            MenuItem SoundBalance = new MenuItem();
            SoundBalance.Text = "SoundBalance";
            SoundBalance.Click += SoundBalanceBtn_Click;

            MenuItem Exit = new MenuItem();
            Exit.Text = "Exit";
            Exit.Click += Exit_Click;

            Re.MenuItems.AddRange(new MenuItem[] { SoundBalance, Exit });
            return Re;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Game_Click(object sender, EventArgs e)
        {
            cv_Con.Input_Game();
        }


        private void PC_Click(object sender, EventArgs e)
        {
            cv_Con.Input_PC();
            new Thread(() =>
            {
                Thread.Sleep(4000);
                var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "control.exe");
                System.Diagnostics.Process.Start(cplPath, "/name Microsoft.Sound");
                Thread.Sleep(10000);
                killProcess("Rundll32");
            }).Start();         
        }

        private void Radio_Changed(object sender, EventArgs e)
        {
            RadioButton I=sender as RadioButton;
            if (!I.Checked)
                return;
            switch (I.Text)
            {
                case "PC":
                    PC_Click(sender, e);
                        break;
                case "Game":
                    Game_Click(sender, e);
                    break;
            }
        }

        private void Show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void Vol_UpBtn_Click(object sender, EventArgs e)
        {
            cv_Con.Volume_UP();
        }

        private void Vol_DownBtn_Click(object sender, EventArgs e)
        {
            cv_Con.Volume_Down();
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            cv_Con.Power_On();
            int H = DateTime.Now.Hour;
            if (8 < H && H < 21)
            {
                cv_Con.Volume(55);
            }
            else
            {
                cv_Con.Volume(45);
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            cv_Con.Power_Off();
        }

        private void SoundBalanceBtn_Click(object sender, EventArgs e)
        {
            new SoundBalance();
        }
    }
}
