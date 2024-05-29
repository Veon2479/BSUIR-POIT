using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace DisposePattern
{
    public class NativeBuffer : Object, IDisposable
    {
        private IntPtr handle;
        private bool disposed;

        public NativeBuffer(int size)
        {
            handle = Marshal.AllocHGlobal(size);
        }

        public IntPtr Handle
        {
            get
            {
                if (!disposed)
                    return handle;
                else
                    throw new ObjectDisposedException(ToString());
            }
        }

        ~NativeBuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
                Marshal.FreeHGlobal(handle);
        }
    }

    
    class Program
    {
        static void TestNativeBuffer()
        {
            NativeBuffer b = new NativeBuffer(1024);
            try
            {
                IntPtr h = b.Handle;
                Console.WriteLine("Работает!");
            }
            finally
            {
                b.Dispose();
            }
        }

        static void Main(string[] args)
        {
            TestNativeBuffer();
            Console.ReadLine();
        }
    }
}
