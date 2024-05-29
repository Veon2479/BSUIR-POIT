using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadStop
{
    class Stopper
    {
        public bool ShouldStop
        {
            get 
            {
                Thread.MemoryBarrier();
                return shouldStop; 
            }
            set
            {
                shouldStop = value;
                Thread.MemoryBarrier();
            }
        }

        private bool shouldStop;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var stopper = new Stopper();
            ThreadPool.QueueUserWorkItem(DoWork, stopper);
            Thread.Sleep(2000);
            stopper.ShouldStop = true;
            Console.ReadLine();

            var cancellation = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(DoWork2, cancellation.Token);
            Thread.Sleep(2000);
            cancellation.Cancel();
            Console.ReadLine();
        }

        public static void DoWork(object state)
        {
            Stopper stopper = (Stopper)state;
            for (int i = 0; i < 100000; ++i)
            {
                if (stopper.ShouldStop)
                    Thread.CurrentThread.Abort();
                Console.WriteLine(i);
            }
        }

        public static void DoWork2(object state)
        {
            try
            {
                CancellationToken cancel = (CancellationToken)state;
                for (int i = 0; i < 100000; ++i)
                {
                    //if (cancel.IsCancellationRequested)
                    //    break;
                    cancel.ThrowIfCancellationRequested();
                    //if (cancel.IsCancellationRequested)
                    //    throw new OperationCanceledException();
                    Console.WriteLine(i);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("DoWork2 прерван");
            }
        }
    }
}
