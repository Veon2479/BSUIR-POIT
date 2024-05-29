using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace UnmanagedInterop
{
    [StructLayout(LayoutKind.Sequential)]
    public class SystemTime
    {
        public UInt16 wYear;
        public UInt16 wMonth;
        public UInt16 wDayOfWeek;
        public UInt16 wDay;
        public UInt16 wHour;
        public UInt16 wMinute;
        public UInt16 wSecond;
        public UInt16 wMilliseconds;

        [DllImport("kernel32", EntryPoint = "GetSystemTime",
            CallingConvention = CallingConvention.Winapi,
            CharSet = CharSet.Unicode)]
        public extern static void Get(SystemTime result);
    }

    class Program
    {
        static void Main()
        {
            var t = new SystemTime();
            SystemTime.Get(t);
            Console.WriteLine("{0}-{1}-{2} {3}:{4}:{5}", 
                t.wDay, t.wMonth, t.wYear, t.wHour, t.wMinute, t.wSecond);
            Console.ReadLine();
        }
    }
}
