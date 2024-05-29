using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AtomicList
{
    public class Log
    {
        private List<string> items;
        int threadId;

        public void Add1(string message)
        {
            Monitor.Enter(items);
            // throw new ThreadAbortException();
            try
            {
                items.Add(message);
            }
            finally
            {
                Monitor.Exit(items);
            }
        }

        public void Add(string message)
        {
            //lock (items)
            //    items.Add(message);

            bool lockTaken = false;
            // throw new ThreadAbortException();
            try
            {
                // throw new ThreadAbortException();
                Monitor.Enter(items, ref lockTaken);
                // throw new ThreadAbortException();
                items.Add(message);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(items);
            }
        }

        public void AddAtomic(string message)
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            while (Interlocked.CompareExchange(ref threadId, 
                currentThreadId, 0) != 0)
            {
                Thread.Yield();
            }
            try
            {
                items.Add(message);
            }
            finally
            {
                Interlocked.Exchange(ref threadId, 0);
            }
        }
    }

    class Program
    {
        public static void DoWork(object arg)
        {
            try
            {
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();

                // throw;
            } 
        }

        static void Main(string[] args)
        {
            Thread thread = new Thread(DoWork);
            thread.Start();
            Thread.Sleep(100);

            thread.Abort();

            // Thread.CurrentThread.Abort();
            // throw new ThreadAbortException();
        }
    }
}
