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

        public CountedObject()
        {
            Interlocked.Increment(ref count);
            Debugger.Break();
        }

        ~CountedObject()
        {
            Interlocked.Decrement(ref count);
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
        }
    }
}
