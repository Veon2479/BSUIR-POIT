using System;

namespace SpanStackalloc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IntegerToString(0));
            Console.WriteLine(IntegerToString(10));
            Console.WriteLine(IntegerToString(20));
        }
        
        static string IntegerToString(int n)
        {
            Span<char> buffer = stackalloc char[16];
            int i = buffer.Length;
            do
            {
                buffer[--i] = (char)(n % 10 + '0');
                n /= 10;
            } while (n != 0);
            return buffer.Slice(i, buffer.Length - i).ToString();
        }
    }
}
