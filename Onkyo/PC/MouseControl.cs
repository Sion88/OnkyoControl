using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace SionTool.PC
{
    /// <summary>
    /// 控制滑鼠
    /// </summary>
    public class MouseControl
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        private enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        private void BaseMove(int x, int y)
        {
            lock (this)
                mouse_event(MouseEventFlag.Move, x, y, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 移動距離
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(int x, int y) {
            lock (this)
                BaseMove(x, y);
        }
        /// <summary>
        /// 移動至
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(int x, int y)
        {
            //TODO重新寫邏輯
            //int OneMove = 10;
            //int OkRange = 6;
            //Point Now = new Point();
            //if (x != 0)
            //{
            //    int XRangeT = x + OkRange;
            //    int XRangeB = x - OkRange;
            //    while (!(XRangeT > Now.X && Now.X > XRangeB))
            //    {
            //        MoveHorizontal(Now.X < x ? OneMove : -OneMove);
            //        GetCursorPos(out Now);
            //    }
            //}
            //if (y != 0)
            //{
            //    var YRangeT = y + OkRange;
            //    var YRangeB = y - OkRange;
            //    while (!(YRangeT > Now.Y && Now.Y > YRangeB))
            //    {
            //        MoveVertical(Now.Y < y ? OneMove : -OneMove);
            //        GetCursorPos(out Now);
            //    }
            //}
        }
        /// <summary>
        /// 取得當下滑鼠位置
        /// </summary>
        /// <returns></returns>
        public Point GetPosition()
        {
            Point re = new Point();
            GetCursorPos(out re);
            return re;
        }
        /// <summary>
        /// 直接將滑鼠設定置那個位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }
        /// <summary>
        /// 左鍵按下
        /// </summary>
        public void LeftDown()
        {
            lock (this)
                mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 左鍵放開
        /// </summary>
        public void LeftUp()
        {
            lock (this)
                mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 左鍵單點
        /// </summary>
        /// <param name="x"></param>
        public void LeftClick()
        {
            lock (this)
            {
                LeftDown();
                LeftUp();
            }
        }
        /// <summary>
        /// 右鍵按下
        /// </summary>
        public void RightDown()
        {
            lock (this)
                mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 右鍵放開
        /// </summary>
        public void RightUp()
        {
            lock (this)
                mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
        }
        /// <summary>
        /// 右鍵單點
        /// </summary>
        public void RightClick()
        {
            lock (this)
            {
                RightDown();
                RightUp();
            }
        }
    }
}
