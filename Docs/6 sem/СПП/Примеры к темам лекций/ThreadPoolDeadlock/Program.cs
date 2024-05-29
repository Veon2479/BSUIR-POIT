using System;
using System.Threading;

namespace ThreadPoolDeadlock
{
    class Program
    {
        static int CompletedTaskNumber = 0;
    
        static void Main(string[] args)
        {
            ThreadPool.GetMinThreads(out int workerThreads, out _);
            ThreadPool.SetMaxThreads(workerThreads, workerThreads);
            int n = workerThreads + 1; // если n = workerThreads, то нет блокировки
            ThreadPool.QueueUserWorkItem(Do, n);
            while (CompletedTaskNumber != n)
                Thread.Yield();
            Console.WriteLine("All tasks completed");
            Console.ReadLine();
        }
    
        static void Do(object value)
        {
            int current = (int) value;
            if (current > 1)
            {
                int next = current - 1;
                Console.WriteLine($"Task {current} waiting task {next}");
                ThreadPool.QueueUserWorkItem(Do, next);
                while (CompletedTaskNumber != next)
                    Thread.Yield();
            }
            Console.WriteLine($"Task {current} completed");
            CompletedTaskNumber = current;
        }
    }
}
