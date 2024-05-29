using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VolatileVariables
{
    class Program
    {
        static volatile bool stopped = false;

        static void Main(string[] args)
        {
            var thread = new Thread(DoLongWork);
            thread.Start();
            Console.WriteLine("Работа начата");
            Thread.Sleep(500);
            stopped = true;
            thread.Join();
            Console.WriteLine("Работа завершена");
        }

        static void DoLongWork(object state)
        {
            bool work = false;
            while (!stopped)
                work = !work;
        }
    }
}
