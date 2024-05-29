using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;

namespace GetType
{
    class Program
    {
        public struct Point
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return string.Format("({0},{1})", X, Y);
            }
        }

        public class Rectangle
        {
            public Point A;
            public Point B;

            public override string ToString()
            {
                return string.Format("[{0};{1}]", A, B);
            }
        }

        static void Main(string[] args)
        {
            object r = new Rectangle();

            Console.WriteLine("{0}\r\n", r);

            Type t = r.GetType();
            MemberInfo[] members = t.GetMembers();
            foreach (MemberInfo m in members)
            {
                Console.WriteLine(m.ToString());
            }

            Console.ReadLine();
        }
    }
}
