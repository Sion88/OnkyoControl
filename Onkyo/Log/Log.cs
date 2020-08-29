using SionTool.Flie;
using System;
using System.IO;
using System.Timers;

namespace SionTool.Log
{
    public class Loger
    {
        public readonly Char ContentCenterChar = '—';
        readonly int FileSizeLimit = 5000000;
        readonly string Path;
        readonly string FileName;
        readonly int Cycle = 10 * 1000;
        System.Timers.Timer MyTimer;
        StreamWriter Writer;
        int Sequence = 0;
        string RealFileName;
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="m_FileNane">檔案名稱</param>
        /// <param name="m_Path">路徑</param>
        public Loger(string m_FileNane, string m_Path = "")
        {
            if (!string.IsNullOrWhiteSpace(m_Path) && m_Path[m_Path.Length - 1] != '\\')
                m_Path = m_Path + @"\";
            Path = m_Path;
            FileName = m_FileNane;
            SFile.CreateForder(Path);
            UpdateSelectFile();
            MyTimer = new System.Timers.Timer();
            MyTimer.Elapsed += MyTimer_Elapsed1;
            MyTimer.Interval = Cycle;
            MyTimer.Start();
        }

        private void MyTimer_Elapsed1(object sender, ElapsedEventArgs e)
        {
            UpdateSelectFile();
        }

        private void UpdateSelectFile()
        {
            lock (this)
            {
                if (Writer != null)
                    Writer.Dispose();
                Boolean Isthis = false;
                while (!Isthis)
                {
                    RealFileName = Path + DateTime.Now.ToString("yyyyMMdd_") + FileName + "_" + Sequence.ToString() + ".txt";
                    if (System.IO.File.Exists(RealFileName))
                    {
                        FileStream FileStream = System.IO.File.OpenRead(RealFileName);
                        Boolean Ch = FileStream.Length > FileSizeLimit;
                        FileStream.Dispose();
                        if (Ch)
                        {
                            Sequence++;
                            continue;
                        }
                    }
                    Isthis = true;
                }
                Writer = System.IO.File.AppendText(RealFileName);
            }
        }
        public void WriteLog(int Level, string Content)
        {
            lock (this)
            {
                Writer.WriteLine(Level.ToString() + ContentCenterChar + DateTime.Now.ToString("HH:mm:ss.fff") + ContentCenterChar + Content);
                Writer.Flush();
            }
        }
    }
}
