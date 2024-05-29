using System;
using System.Runtime.InteropServices;

namespace Example
{
    [StructLayout(LayoutKind.Explicit)]
    public struct TwoBytes
    {
        [FieldOffset(0)]
        private byte FirstByte;
        [FieldOffset(1)]
        private byte SecondByte;
        [FieldOffset(0)]
        private ushort Word;

        public static bool IsLittleEndian()
        {
            var twoBytes = new TwoBytes { Word = 1 };
            return twoBytes.FirstByte == 1;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine(TwoBytes.IsLittleEndian());
        }
    }
}
