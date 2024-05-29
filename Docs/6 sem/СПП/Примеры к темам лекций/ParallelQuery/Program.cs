using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelQuery
{
    class Program
    {
        static void Main()
        {
            int[] numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<int> result = numbers
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .Where(n => n % 2 == 0)
                .Select(Square).ToList();

            result.ForEach(x => Console.WriteLine(x));
        }

        static int Square(int n)
        {   
            Console.WriteLine("Обработка {0} потоком {1}",
                n, Thread.CurrentThread.ManagedThreadId);
            return n * n;
        }
    }
}
