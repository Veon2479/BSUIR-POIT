using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace DisposePattern
{
    public class LogFile : Object, IDisposable
    {
        private StreamWriter writer;
        private bool disposed;

        public LogFile(string filePath)
        {
            writer = new StreamWriter(filePath, append: true);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                writer.Close();
                disposed = true;
            }
        }

        public void Write(string str)
        {
            if (disposed)
                throw new ObjectDisposedException(this.ToString());

            writer.Write(str);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var f = new LogFile(@"./Log.txt");
            try
            {
                f.Write("Ключ на старт");
                f.Write("Протяжка-1");
                f.Write("Продувка");
                f.Write("Протяжка-2");
                f.Write("Ключ на дренаж");
                f.Write("Пуск");
                f.Write("Зажигание");
            }
            finally
            {
                f.Dispose();
            }

            // Короткий эквивалент в C# с оператором using:

            using (var f2 = new LogFile(@"./Log.txt"))
            {
                f2.Write("Ключ на старт");
                f2.Write("Протяжка-1");
                f2.Write("Продувка");
                f2.Write("Протяжка-2");
                f2.Write("Ключ на дренаж");
                f2.Write("Пуск");
                f2.Write("Зажигание");
            }

            Console.ReadLine();
        }
    }
}
