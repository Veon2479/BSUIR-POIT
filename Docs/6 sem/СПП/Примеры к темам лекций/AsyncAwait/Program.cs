using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            ValueTask<int> task = TaskProcedure1();
            int result = task.GetAwaiter().GetResult();
            Console.WriteLine(result);
        }

        public async static ValueTask<int> TaskProcedure1()
        {
            ValueTask<int> task = TaskProcedure2();
            // асинхронные действия
            int result = await task; 
            return result;
        }

        public async static ValueTask<int> TaskProcedure2()
        {
            //Task<int> t1 = TaskProcedure3();
            //Task<int> t2 = TaskProcedure4();
            // ...
            int result = await TaskProcedure3() + await TaskProcedure4();
            return result;
        }

        public async static ValueTask<int> TaskProcedure3()
        {
            return 5;
        }

        public async static ValueTask<int> TaskProcedure4()
        {
            return 10;
        }
    }
}
