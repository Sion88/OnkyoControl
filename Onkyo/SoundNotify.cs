using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Onkyo
{
    public partial class ShowText : Form
    {
        static int cv_ShowTime = 1500;
        DateTime cv_WorkTime;
        Thread cv_T;
        public ShowText()
        {        
            this.StartPosition = FormStartPosition.Manual;
            this.Hide();
            InitializeComponent();
        }
        public void setVal(string  m_I) {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => Show(m_I)));
            }
            else
                Show(m_I);
        }
        private void Show(string m_I)
        {
            {
                int MinX = 0;
                int MinY = 0;
                foreach (var I in Screen.AllScreens)
                {
                    if (I.Bounds.X < MinX)
                        MinX = I.Bounds.X;
                    if (I.Bounds.Y < MinY)
                        MinY = I.Bounds.Y;
                }
                this.Location = new Point(MinX, MinY);
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
                Text.Text = m_I;
                cv_WorkTime = DateTime.Now;
                if (cv_T == null)
                {
                    cv_T = new Thread(() =>
                    {
                        int Tmp = cv_ShowTime + 1;
                        Console.WriteLine((DateTime.Now - cv_WorkTime).TotalMilliseconds);
                        while ((DateTime.Now - cv_WorkTime).TotalMilliseconds< Tmp) {
                            Thread.Sleep(100);
                        }
                        this.Invoke(new Action(() =>
                        {
                            this.Hide();
                        }));
                        cv_T = null;
                    });
                    cv_T.Start();
                }
            }
        }
    }
}
