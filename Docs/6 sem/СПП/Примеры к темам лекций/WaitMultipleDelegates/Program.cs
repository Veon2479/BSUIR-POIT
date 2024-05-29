using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    public class ActionRunner
    {
        int runningCount;
        object sync = new object();

        public void RunAndWaitAll(Action[] actions)
        {
            runningCount = actions.Length;
            foreach (Action action in actions)
                ThreadPool.QueueUserWorkItem(ExecuteAction, action);
            lock (sync)
                if (runningCount > 0)
                    Monitor.Wait(sync);
        }

        private void ExecuteAction(object state)
        {
            var action = (Action)state;
            action();
            lock (sync)
            {
                runningCount--;
                if (runningCount == 0)
                    Monitor.Pulse(sync);
            }
        }
    }

    class Program
    {
        static void MyAction()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(id);
        }

        static void Main(string[] args)
        {
            var actions = new Action[1000];
            for (int i = 0; i < 1000; i++)
                actions[i] = MyAction;
            var actionRunner = new ActionRunner();
            actionRunner.RunAndWaitAll(actions);
            Console.WriteLine("Задачи выполнены.");
        }
    }
}