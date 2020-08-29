using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SionTool.PC
{
    class PCScreenShot
    {
        int cv_x, cv_y, cv_Width, cv_Heigth;
        public PCScreenShot(int m_x, int m_y, int m_Width, int m_Heigth)
        {
            cv_x = m_x;
            cv_y = m_y;
            cv_Width = m_Width;
            cv_Heigth = m_Heigth;
        }
        public Bitmap ScreenShot()
        {
            Bitmap screenshot = new Bitmap(cv_Width, cv_Heigth, PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(screenshot);
            graph.CopyFromScreen(cv_x, cv_y, 0, 0, new Size(cv_Width, cv_Heigth), CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        public static  Bitmap ScreenShot(int x, int y, int Width, int Heigth)
        {
            Bitmap screenshot = new Bitmap(Width, Heigth, PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(screenshot);
            graph.CopyFromScreen(x, y, 0, 0, new Size(Width, Heigth), CopyPixelOperation.SourceCopy);
            return screenshot;
        }
        public static Bitmap ScreenShotPercent(int xPercent, int yPercent, int WidthPercent, int HeigthPercent)
        {
            var ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var ScreenHeigth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int x = ScreenWidth * xPercent / 100;
            int y = ScreenHeigth * yPercent / 100;
            int Width = ScreenWidth * WidthPercent / 100;
            int Height = ScreenHeigth * HeigthPercent / 100;
            Bitmap screenshot = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(screenshot);
            graph.CopyFromScreen(x, y, 0, 0, new Size(Width, Height), CopyPixelOperation.SourceCopy);
            return screenshot;
        }
    }
}
