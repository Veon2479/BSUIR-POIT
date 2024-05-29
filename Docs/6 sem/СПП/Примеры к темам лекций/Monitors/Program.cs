using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Monitors
{
    class Program
    {
        static List<int> List;

        static void Main(string[] args)
        {
            while (List.Count == 0)
            {
                Thread.Sleep(1);
            }
        }

        public static object sync = new object();

        public static void DoWorkT1()
        {
            lock (sync)
                lock (sync)
                {
                    Monitor.Wait(sync); // Monitor.Exit(sync); Monitor.Exit(sync);
                                        // подождать Monitor.Pulse(sync);
                                        // Monitor.Enter(sync); Monitor.Enter(sync)
                }
        }

        public static void DoWorkT2()
        {
            lock (sync)
                lock (sync)
                {
                    Monitor.Wait(sync); // Monitor.Exit(sync); Monitor.Exit(sync);
                                        // подождать Monitor.Pulse(sync);
                                        // Monitor.Enter(sync); Monitor.Enter(sync)
                }
        }

        public static void DoWorkT3()
        {
            lock (sync)
            {
                Monitor.Pulse(sync); // Monitor.Exit(sync); Monitor.Exit(sync);
                                     // выполнить Monitor.Pulse(sync);
                                     // Monitor.Enter(sync); Monitor.Enter(sync)
            }
        }

        public static int count;

        public static void Do()
        {
            var w = new SpinWait();
            w.SpinOnce();

            int c = 0;
            Thread.MemoryBarrier();
            while (count == 0)
            {
                Thread.MemoryBarrier();
                Thread.Sleep(c);
                c = 1;
            }
                
        }
    }
}
