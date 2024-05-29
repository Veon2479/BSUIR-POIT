using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        static object sync = new object();

        public static void DoWork(object obj)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("\nПоток с номером {0}", threadId);
            for (int i = 0; i < 1000; ++i)
            {
                Console.Write(" {0}", obj);
                Thread.Sleep(10);
            }
        }

        static void DedicatedThreads()
        {
            Thread t1 = new Thread(DoWork, 16 * 1024);
            t1.Start("o");
            Thread t2 = new Thread(DoWork);
            t2.Start("X");
            Console.ReadLine();
        }

        static void DedicatedBackgroundThreads()
        {
            Thread t1 = new Thread(DoWork) { IsBackground = true };
            t1.Start("o");
            Thread t2 = new Thread(DoWork) { IsBackground = true };
            t2.Start("X");
            Console.ReadLine();
        }

        static void ThreadPoolThreads()
        {
            ThreadPool.QueueUserWorkItem(DoWork, "o");
            ThreadPool.QueueUserWorkItem(DoWork, "X");
            Console.ReadLine();
        }

        static void ThousandThreads()
        {
            for (int i = 0; i < 10000; i++)
                ThreadPool.QueueUserWorkItem(DoWork, i);
            Console.ReadLine();
        }

        public static void DoSilentWork(object obj)
        {
            for (int i = 0; i < 1000; ++i)
                Thread.Sleep(10);
        }

        public static void ReportCompleted(IAsyncResult obj)
        {
            Console.WriteLine("\nCompleted");
        }


        static void SetupThreadPool()
        {
            ThreadPool.GetMinThreads(out int minThreads, out int minIO);
            Console.WriteLine($"Min Threads = {minThreads}");
            ThreadPool.GetMaxThreads(out int maxThreads, out int maxIO);
            Console.WriteLine($"Max Threads = {maxThreads}");

            for (int i = 0; i < 10000; i++)
                ThreadPool.QueueUserWorkItem(DoWork, i);

            for (int i = 0; i < 1000; ++i)
            {
                ThreadPool.GetAvailableThreads(out int threads, out int IO);
                Console.WriteLine($"\nActive Threads = {maxThreads - threads}");
                Thread.Sleep(100);
            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            DedicatedThreads();
            //ThreadPoolThreads();
            //DedicatedBackgroundThreads();
            //ThousandThreads();
            //SetupThreadPool();
        }
    }
}
