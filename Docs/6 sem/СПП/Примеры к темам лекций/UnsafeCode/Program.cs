using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnsafeCode
{
    class Program
    {
        public static void UnsafeToLower(char[] array)
        {
            unsafe
            {
                fixed (char* a = &array[0])
                {
                    char* p = a;
                    while (*p != '\0')
                    {
                        *p = char.ToLower(*p);
                        p++;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            char[] arr = "ABCDEFGH\0".ToCharArray();
            UnsafeToLower(arr);
            Console.WriteLine(arr);
            Console.ReadLine();
        }
    }
}
