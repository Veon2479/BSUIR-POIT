using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace AsyncOperations
{
    class Program
    {
        public static void AsyncWrite()
        {
            string s = "My Test String 1 ";
            string s2 = "My Test String 2 ";

            using (var fs = new FileStream(@"D:/MyTestLog.txt", FileMode.Append,
                FileAccess.Write, FileShare.None))
            {
                byte[] data = Encoding.UTF8.GetBytes(s);
                fs.Write(data, 0, data.Length);

                IAsyncResult asyncResult = fs.BeginWrite(data, 0, data.Length, 
                    null, fs);

                //while (!asyncResult.IsCompleted)
                //{
                //    Console.WriteLine("Идет запись блока данных в файл");
                //}

                byte[] data2 = Encoding.UTF8.GetBytes(s2);
                fs.Write(data2, 0, data2.Length);
                Console.WriteLine("Идет запись блока данных в файл");

                //((FileStream)(asyncResult.AsyncState)).EndWrite(asyncResult);

                fs.EndWrite(asyncResult);

                //if (!asyncResult.IsCompleted)
                //    asyncResult.AsyncWaitHandle.WaitOne();
            }
        }

        public static void AsyncWrite2()
        {
            string s = "My Test String 1 ";
            var fs = new FileStream(@"D:/MyTestLog.txt", FileMode.Append,
                FileAccess.Write, FileShare.None);
            byte[] data = Encoding.UTF8.GetBytes(s);
            fs.BeginWrite(data, 0, data.Length, WriteCompleted, fs);
        }

        public static void WriteCompleted(IAsyncResult asyncResult)
        {
            var fs = (FileStream)(asyncResult.AsyncState);
            fs.EndWrite(asyncResult);
            fs.Close();
            Console.WriteLine("Запись блока данных в файл завершена");
        }

        public delegate void NotifyCallback(string arg, int a, int b);

        public static NotifyCallback Notify;

        static void Main(string[] args)
        {
            //AsyncWrite();
            AsyncWrite2();

            if (Notify != null)
            {
                IAsyncResult asyncResult = Notify.BeginInvoke(
                    " ", 1, 2, null, Notify);

                // ...

                Notify.EndInvoke(asyncResult);
            }

            Console.ReadLine();
        }

    }
}
