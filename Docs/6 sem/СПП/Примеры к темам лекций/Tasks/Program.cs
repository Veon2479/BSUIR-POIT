using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksLibrary
{
    class Program
    {
        static long Fibonachi(int count)
        {
            long previous = 1;
            long result = 0;
            for (int i = 0; i < count; i++)
            {
                long s = previous + result;
                previous = result;
                result = s;
            }
            Console.WriteLine("ThreadId во время работы Fibonachi: {0}", 
                Thread.CurrentThread.ManagedThreadId);
            return result;
        }
        
        static Task<long> FibonachiTask(int count)
        {
            Task<long> task = new Task<long>(() => Fibonachi(count));
            task.Start();
            return task;
        }

        static async Task<long> FibonachiAsync(int count)
        {
            Console.WriteLine("ThreadId во время работы FibonachiAsync: {0}", 
                Thread.CurrentThread.ManagedThreadId);
            long result = await FibonachiTask(count);
            return result;
        }

        static void TestFibonachiTask()
        {
            Task<long> task = FibonachiTask(10);
            Console.WriteLine("Запустили задачу FibonachiTask");
            task.Wait();
            Console.WriteLine("Дождались задачу FibonachiTask, результат: {0}", task.Result);
        }

        static void TestFibonachiAsync()
        {
            Console.WriteLine("Запускаем асинхронную задачу FibonachiAsync");
            long result = FibonachiAsync(10).GetAwaiter().GetResult();
            Console.WriteLine("Дождались задачу FibonachiAsync, результат: {0}", result);
            Console.WriteLine();
        }

        static void TestFibonachiTasks()
        {
            Task task = new Task(() => { Fibonachi(10); });
            task.Start();
            Console.WriteLine("Запустили задачу Task");
            task.Wait();
            Console.WriteLine("Дождались задачу Task");
            Console.WriteLine();

            Task<long> task2 = new Task<long>(() => Fibonachi(10));
            task2.Start();
            Console.WriteLine("Запустили задачу Task<long>");
            task2.Wait();
            Console.WriteLine("Дождались задачу Task<long>, результат: {0}", task2.Result);
            Console.WriteLine();

            Task<long> task3 = FibonachiTask(10);
            Console.WriteLine("Запустили задачу FibonachiTask");
            task3.Wait();
            Console.WriteLine("Дождались задачу FibonachiTask, результат: {0}", task3.Result);
            Console.WriteLine();

            Task<long> task4 = FibonachiAsync(10);
            Console.WriteLine("Запустили задачу FibonachiAsync");
            task4.Wait();
            long result = task4.Result;
            Console.WriteLine("Дождались задачу FibonachiAsync, результат: {0}", result);
            Console.WriteLine();
        }

        //static async IAsyncEnumerable<long> GetFibonachiList(int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        await Task.Delay(100);
        //        yield return Fibonachi(i);
        //    }
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("ThreadId главного потока: {0}", Thread.CurrentThread.ManagedThreadId);

            TestFibonachiTask();
            TestFibonachiAsync();
            TestFibonachiTasks();

            Console.ReadLine();
        }
    }
}
