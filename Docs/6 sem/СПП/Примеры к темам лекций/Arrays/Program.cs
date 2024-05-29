using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrays
{
    class Program
    {
        static int[] IntegerArray = { 5, 10, 15, 20 };

        static void Main(string[] args)
        {
            IntegerArray = new int[5];
            Console.WriteLine(IntegerArray[2]);
            object o = IntegerArray[2];
            Console.WriteLine(o.ToString());
            IntegerArray = null;
        }
    }
}
