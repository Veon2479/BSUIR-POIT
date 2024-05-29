using System;
using System.Threading;

namespace AsyncDelegates
{
    class Program
    {
        static void Main()
        {
            Action action = Do;
            IAsyncResult result = action.BeginInvoke(WorkCompleted, null);
            Console.WriteLine("Ожидание окончания выполнения делегата.");
            action.EndInvoke(result);
        }

        static void Do()
        {
            Console.WriteLine("Выполняется метод Do.");
            Thread.Sleep(1000);
        }

        static void WorkCompleted(object state)
        {
            Console.WriteLine("Работа выполнена.");
        }
    }
}
