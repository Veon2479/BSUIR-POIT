using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace CustomThreadPool
{
    public class TaskQueue
    {
        private List<Thread> threads;
        private Queue<Action> tasks;

        public TaskQueue(int threadCount)
        {
            tasks = new Queue<Action>();
            threads = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                var t = new Thread(DoThreadWork);
                threads.Add(t);
                t.IsBackground = true;
                t.Start();
            }
        }

        public int ThreadCount
        {
            get { return threads.Count; }
        }

        public void EnqueueTask(Action task)
        {
            lock (tasks)
            {
                tasks.Enqueue(task);
                Monitor.Pulse(tasks);
            }
        }

        private Action DequeueTask()
        {
            lock (tasks)
            {
                while (tasks.Count == 0)
                    Monitor.Wait(tasks);
                return tasks.Dequeue();
            }
        }

        private void DoThreadWork()
        {
            while (true)
            {
                Action task = DequeueTask();
                try
                {
                    task();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }

    class Program
    {
        static int TaskCount = 0;

        static void TestTask()
        {
            try
            {
                var taskNumber = Interlocked.Increment(ref TaskCount);
                WriteTaskNumber(taskNumber);
                
                Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void WriteTaskNumber(int taskNumber)
        {
            for (int i = 0; i < 10000; ++i)
                Console.Write(" {0} ", taskNumber);
        }

        static void Main(string[] args)
        {
            var taskQueue = new TaskQueue(3);
            for (int i = 0; i < 10; i++)
                taskQueue.EnqueueTask(TestTask);
            Console.ReadLine();
        }
    }
}
