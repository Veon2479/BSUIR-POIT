using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace UnmanagedInterop
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MutableString
    {
        [FieldOffset(0)]
        public string AsString;
        [FieldOffset(0)]
        public char[] AsCharArray;
        [FieldOffset(0)]
        public Int64[] AsUInt64Array;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MutableString s2 = new MutableString();
            s2.AsString = "\0\0\0\0\0\0\0\0";

            MutableString s = new MutableString();
            s.AsString = "Казнить  нельзя  помиловать ";
            s.AsCharArray[7] = ',';
            Console.WriteLine(s.AsString);
            s.AsCharArray[27] = '!';
            Console.WriteLine(s.AsString);

            for (int i = 0; i < s2.AsUInt64Array.Length; i++)
                Console.WriteLine(s2.AsUInt64Array[i]);

            s2.AsUInt64Array[7] = 0;

            for (int i = 0; i < s2.AsUInt64Array.Length; i++)
                Console.WriteLine(s2.AsUInt64Array[i]);

            Console.WriteLine(s.AsString);

            Console.ReadLine();
        }
    }
}
