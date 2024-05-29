using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example
{
    public class Mutex
    {
        private Thread thread;

        public void Lock()
        {
            Thread t = Thread.CurrentThread;
            while (Interlocked.CompareExchange(ref thread, t, null) != null)
                Thread.Yield();
            Thread.MemoryBarrier();
        }

        public void Unlock()
        {
            Thread t = Thread.CurrentThread;
            if (Interlocked.CompareExchange(ref thread, null, t) != t)
                throw new SynchronizationLockException("Вызов Unlock без вызова Lock");
            Thread.MemoryBarrier();
        }
    }

    public class CountedObject
    {
        public static int count = 0;
        static Mutex mutex = new Mutex();

        public CountedObject()
        {
            mutex.Lock();
            try
            {
                count += 1;
            }
            finally
            {
                mutex.Unlock();
            }
        }

        ~CountedObject()
        {
            mutex.Lock();
            try
            {
                count -= 1;
            }
            finally
            {
                mutex.Unlock();
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
        }

        public static int CompareExchange(
            ref int location, int value, int comparand)
        {
            int result = location;
            if (location == comparand)
                location = value;
            return result;
        }
    }
}
