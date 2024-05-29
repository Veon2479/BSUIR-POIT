using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticConstructors
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass.Instance.Do();
            MyClass.Instance.Do();
        }
    }

    class MyClass
    {
        public static MyClass Instance;

        public static MyClass()
        {
            Instance = new MyClass();
        }

        public void Do()
        {
            Console.WriteLine("Singleton pattern");
        }
    }
}
