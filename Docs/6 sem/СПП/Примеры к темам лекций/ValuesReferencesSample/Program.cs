using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValuesReferencesSample
{
    class Program
    {
        public struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X;
            public int Y;

            public override string ToString()
            {
                return string.Format("({0},{1})", X, Y);
            }

        }

        public class Rectangle
        {
            public Rectangle()
            {
            }

            public Point A;
            public Point B;
        }
        
        static void Main(string[] args)
        {
            Point p = new Point();

            object o = p;

            p.X = 10;
            p.Y = 10;
            Point p2 = (Point)(o);

            Console.WriteLine("p = ", p.ToString());
            Console.WriteLine("o = ", o.ToString());

            Console.ReadLine();
        }

        static public void Do()
        {
            System.Int32 a = 5;

            object o = a;
        }
    }
}
