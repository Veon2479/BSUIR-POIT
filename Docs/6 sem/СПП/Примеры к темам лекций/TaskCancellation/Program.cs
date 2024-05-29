using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            Task task = new Task(() => Procedure(cancellationTokenSource.Token), cancellationTokenSource.Token);
            task.Start();
            Thread.Sleep(2000);
            cancellationTokenSource.Cancel();
            task.Wait();
            Console.WriteLine("\nTask finished");
            Console.ReadLine();
        }

        private static void Procedure(CancellationToken cancellationToken)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("\nCancellation requested");
                        break;
                    }
                    Console.Write(" {0} ", i);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
