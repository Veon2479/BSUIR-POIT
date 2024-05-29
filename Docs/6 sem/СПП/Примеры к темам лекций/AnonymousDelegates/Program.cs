using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnonymousDelegates
{
    class Program
    {
        public delegate TResult Func<in T, out TResult>(T arg);

        public delegate bool Predicate(string arg);

        public static bool StaticPredicate(string arg)
        {
            return arg.StartsWith("Ключ");
        }

        static string[] Commands = new string[] { "Ключ на старт", "Протяжка-1", "Продувка",
            "Протяжка-2", "Ключ на дренаж", "Пуск", "Зажигание" };

        static void Main(string[] args)
        {
            var subset = Commands.Where(StaticPredicate);

            string prefix = "Ключ";

            System.Func<string, bool> inPlacePredicate = delegate(string x) 
            { 
                return x.StartsWith(prefix);
            };

            subset = Commands.Where(inPlacePredicate);

            subset = Commands.Where(
                delegate (string x) 
                {
                    return x.StartsWith(prefix);
                }
            );

            subset = Commands.Where((x) => { return x.StartsWith(prefix); });

            subset = Commands.Where(x => x.StartsWith(prefix));

            Console.WriteLine(string.Join("\n", subset));
            Console.ReadLine();
        }
    }
}
