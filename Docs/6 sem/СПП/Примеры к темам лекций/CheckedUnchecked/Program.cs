using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckedUnchecked
{
    class Program
    {
        public static int TakePercentChecked(int value, int percent)
        {
            checked // В языке C#, в отличие от С++, имеется контроль переполнения
            {
                return value * percent / 100;
            }
        }

        public static int TakePercentUnchecked(int value, int percent)
        {
            unchecked
            {
                return value * percent / 100;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                int percent = TakePercentUnchecked(int.MaxValue, 50);
                Console.WriteLine(percent); // Результат 0
                percent = TakePercentChecked(int.MaxValue, 50); // Исключение
                Console.WriteLine(percent); // Этот оператор никогда не выполнится
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
