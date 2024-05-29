using System;
using System.Threading;

namespace RefParams
{
    public class Point
    {
        public int X;

        public int Y;
    }

    class Program
    {
        static void Main(string[] args)
        {
            int a = 10;
            Interlocked.Exchange(ref a, 20);
            Console.WriteLine($"a = {a}");

            var array = new int[] { 1, 2, 3 };
            ref int element = ref array[0];
            element = 20;
            Console.WriteLine($"array[0] = {array[0]}");
            
            var point = new Point { X = 10, Y = 30 };
            ref int x = ref point.X;
            x = 20;
            Console.WriteLine($"point.X = {point.X}");
        }
    }
}
