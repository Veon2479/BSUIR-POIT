using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example
{
    public class CountedObject
    {
        public static int count = 0;
        static object sync = new object();

        public CountedObject()
        {
            //count++;
            // ldsfld    int32 CountedObject::count
            // ldc.i4.1
            // add
            // stsfld    int32 CountedObject::count

            // mov AX, [count]
            // inc AX
            // mov [count], AX

            lock (sync)
                count += 1;

            //Monitor.Enter(sync);
            //try
            //{
            //    count += 1;
            //}
            //finally
            //{
            //    Monitor.Exit(sync);
            //}
        }

        ~CountedObject()
        {
            //lock (sync)
            //    count -= 1;

            bool lockTaken = false;
            try
            {
                Monitor.Enter(sync, ref lockTaken);
                count -= 1;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(sync);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var obj = new CountedObject();
                    }
                });
            }
            Thread.Sleep(2000);
            Console.WriteLine(CountedObject.count);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
            GC.WaitForPendingFinalizers();
            Console.WriteLine(CountedObject.count);
            Console.ReadLine();
        }
    }
}
