using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace ThreadAbort
{
    public delegate void TaskDelegate();

    public class TaskQueue
    {
        public TaskQueue(int threadCount)
        {
            tasks = new Queue<TaskDelegate>();
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

        public void EnqueueTask(TaskDelegate task)
        {
            lock (tasks)
            {
                tasks.Enqueue(task);
                Monitor.Pulse(tasks);
            }
        }

        private TaskDelegate DequeueTask()
        {
            lock (tasks)
            {
                while (tasks.Count == 0)
                    Monitor.Wait(tasks);
                var t = tasks.Dequeue();
                return t;
            }
        }

        private void DoThreadWork()
        {
            while (true)
            {
                var task = DequeueTask();
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
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private List<Thread> threads;
        private Queue<TaskDelegate> tasks;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Компилятор:
                //if (ex is ThreadAbortException) 
                //    throw;
            }
        }

        static void WriteTaskNumber(int taskNumber)
        {
            for (int i = 0; i < 10000; ++i)
                Console.Write(" {0} ", taskNumber);

            //Thread.CurrentThread.Interrupt();
            Thread.CurrentThread.Abort(); // throw new ThreadAbortException()
        }

        static object sync = new object();

        static void CreateTextFile(string path, ref StreamWriter result)
        {
            try
            {
            }
            finally
            {
                result = File.CreateText(@"D:\SampleFile.txt");
            }
        }
        
        static void TestTask1()
        {
            //using (StreamWriter w = File.CreateText(@"D:\SampleFile.txt"))
            //{
            //    w.WriteLine("Text");
            //}
            
            //StreamWriter w = File.CreateText(@"D:\SampleFile.txt");

            StreamWriter w = null;
            try
            {
                CreateTextFile(@"D:\SampleFile.txt", ref w);
                w.WriteLine("Text");
            }
            finally
            {
                if (w != null)
                    w.Dispose();
            }
        }

        static void TestTask2()
        {
            // --> Thread.Abort() из другого потока асинхронно к данному
            Monitor.Enter(sync);
            // ----------> Thread.Abort() из другого потока асинхронно к данному
            try
            {
                // --> Thread.Abort() из другого потока асинхронно к данному
                //...
                // --> Thread.Abort() из другого потока асинхронно к данному
            }
            finally
            {
                // ------------> Thread.Abort() из другого потока асинхронно к данному
                Monitor.Exit(sync);
                // --> Thread.Abort() из другого потока асинхронно к данному
            }
            // --> Thread.Abort() из другого потока асинхронно к данному
        }

        static void TestTask3()
        {
            bool locked = false;
            // ----------> Thread.Abort() из другого потока асинхронно к данному
            try
            {
                // ----------> Thread.Abort() из другого потока асинхронно к данному
                Monitor.Enter(sync, ref locked);
                // --> Thread.Abort() из другого потока асинхронно к данному
                //...
                // --> Thread.Abort() из другого потока асинхронно к данному
            }
            finally
            {
                // ------------> Thread.Abort() из другого потока асинхронно к данному
                // отложен до выхода из finally
                if (locked)
                    Monitor.Exit(sync);
                // --> Thread.Abort() из другого потока асинхронно к данному
                // отложен до выхода из finally
            }
            // --> Thread.Abort() из другого потока асинхронно к данному
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
