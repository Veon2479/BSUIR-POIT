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
            Console.WriteLine("ThreadId во время работы Fibonachi: {0}", Thread.CurrentThread.ManagedThreadId);
            return result;
        }

        static Task<long> FibonachiTask(int count)
        {
            Task<long> task = new Task<long>(() =>
            {
                Console.WriteLine("ThreadId во время работы FibonachiTask: {0}", Thread.CurrentThread.ManagedThreadId);
                return Fibonachi(count);
            });
            task.Start();
            return task;
        }

        static async Task<long> FibonachiAsync(int count)
        {
            Console.WriteLine("ThreadId во время работы FibonachiAsync: {0}", Thread.CurrentThread.ManagedThreadId);
            return await FibonachiTask(count);
            //return Fibonachi(10);
        }

        static async IAsyncEnumerable<long> GetFibonachiList(int count)
        {
            for (int i = 0; i < count; i++)
                yield return await FibonachiAsync(i);
        }

        static async Task WriteFibonachiList1(int count)
        {
            var stream = GetFibonachiList(count);
            await foreach (long x in stream)
            {
                Console.WriteLine("Fibonachi: " + x);
            }
            Console.WriteLine();
        }

        static async Task WriteFibonachiList2(int count)
        {
            var stream = GetFibonachiList(count);
            IAsyncEnumerator<long> enumerator = stream.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                long x = enumerator.Current;
                Console.WriteLine($"Fibonachi: {x}  ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ThreadId главного потока: {0}", Thread.CurrentThread.ManagedThreadId);

            //Task task = new Task(() => { Fibonachi(10); });
            //task.Start();
            //Console.WriteLine("Запустили задачу Task");
            //task.Wait();
            //Console.WriteLine("Дождались задачу Task");
            //Console.WriteLine();

            //Task<long> task2 = new Task<long>(() => Fibonachi(10));
            //task2.Start();
            //Console.WriteLine("Запустили задачу Task<long>");
            //task2.Wait();
            //Console.WriteLine($"Дождались задачу Task<long>, результат: {task2.Result}");
            //Console.WriteLine();

            //Task<long> task3 = FibonachiTask(10);
            //Console.WriteLine("Запустили задачу FibonachiTask");
            //task3.Wait();
            //Console.WriteLine($"Дождались задачу FibonachiTask, результат: {task3.Result}");
            //Console.WriteLine();

            //Task<long> task4 = FibonachiAsync(10);
            //Console.WriteLine("Запустили задачу FibonachiAsync");
            //task4.Wait();
            //long result = task4.Result;
            //Console.WriteLine($"Дождались задачу FibonachiAsync, результат: {result}");
            //Console.WriteLine();

            //Console.WriteLine("Запускаем асинхронную задачу FibonachiAsync");
            //long result2 = FibonachiAsync(10).GetAwaiter().GetResult();
            //Console.WriteLine($"Дождались асинхронную задачу FibonachiAsync, результат: {result2}");
            //Console.WriteLine();

            Console.WriteLine("Читаем асинхронный поток данных FibonachiList");
            WriteFibonachiList1(10).GetAwaiter().GetResult();
            Console.WriteLine("Дождались асинхронный поток данных FibonachiList");

            Console.ReadLine();
        }
    }
}
