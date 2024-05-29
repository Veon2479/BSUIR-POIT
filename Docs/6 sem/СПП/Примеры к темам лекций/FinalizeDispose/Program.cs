using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime;

namespace FinalizeDispose
{
    public class LogFile : Object
    {
        public LogFile(string filePath)
        {
            stream = new FileStream(filePath, FileMode.Append,
                FileAccess.Write, FileShare.None);
        }

        ~LogFile() // protected override void Finalize()
        {
            Console.WriteLine("Выполняется LogFile.Finalize");
            Thread.Sleep(1000);
            stream.Close();
            Console.WriteLine("LogFile.Finalize выполнился");
        }

        private FileStream stream;
    }

    class Program
    {
        static void Main(string[] args)
        {
            LogFile f = new LogFile(@"D:\MyTestLog.txt");
            f = null;
            Console.WriteLine("LogFile превратился в мусор");
            //GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(3, GCCollectionMode.Default);
            for (int i = 0; i < 3000000; ++i)
            {
                object p = new object();
            }
            Console.WriteLine("Нажмите Enter для завершения");
            Console.ReadLine();
        }
    }
}
