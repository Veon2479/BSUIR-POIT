using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;

namespace ThreadLocalVariables
{
    class Program
    {
        static void Main()
        {
            ThreadPool.QueueUserWorkItem(Worker, "Задача 1");
            ThreadPool.QueueUserWorkItem(Worker, "Задача 2");
            Console.ReadKey();
        }

        static void Worker(object state)
        {
            Logger.TaskName = (string)state;
            for (int i = 0; i < 100; i++)
            {
                Logger.WriteLine("идёт работа");
                Thread.Sleep(100);
            }
        }
    }

    class Logger
    {
        [ThreadStatic]
        public static string TaskName;

        public static void WriteLine(string text)
        {
            Console.WriteLine($"{TaskName}: {text}");
        }
    }

    class Logger2
    {
        static readonly ThreadLocal<string> taskName;

        static Logger2()
        {
            taskName = new ThreadLocal<string>();
        }

        public static string TaskName
        {
            get => taskName.Value;
            set => taskName.Value = value;
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine($"{TaskName}: {text}");
        }
    }

    class Logger3
    {
        static readonly LocalDataStoreSlot slot;

        static Logger3()
        {
            slot = Thread.AllocateDataSlot();
        }

        public static string TaskName
        {
            get => (string)Thread.GetData(slot);
            set => Thread.SetData(slot, value);
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine($"{TaskName}: {text}");
        }
    }
}
