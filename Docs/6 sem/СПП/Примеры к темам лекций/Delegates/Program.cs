using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Delegates
{
    public delegate void Callback(string text);

    //public class Callback : MulticastDelegate
    //{
    //    public extern Callback(object @object, IntPtr method);
    //    public virtual extern IAsyncResult BeginInvoke(
    //        string text, AsyncCallback callback, object @object);
    //    public virtual extern void EndInvoke(IAsyncResult result);
    //    public virtual void Invoke(string text)
    //    {
    //        Delegate[] delegates = _invocationList as Delegate[];
    //        if (delegates != null)
    //        {
    //            foreach (Callback d in delegates)
    //            {
    //                d.Invoke(text);
    //            }
    //        }
    //        else
    //        {
    //            if (_target != null)
    //                _target._methodPtr(text);
    //            else
    //                _methodPtr(text);
    //        }
    //    }
    //}

    class Program
    {
        static string[] Commands = { "Ключ на старт", "Протяжка-1", "Продувка",
            "Протяжка-2", "Ключ на дренаж", "Земля-борт", "Пуск", "Зажигание",
            "Предварительная, промежуточная, главная, подъем" };

        static void LogCommands(Callback callback)
        {
            foreach (string cmd in Commands)
                if (callback != null)
                    callback(cmd);
        }

        static void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }

        static void WriteToMsgBox(string text)
        {
            MessageBox.Show(text);
        }

        void WriteToFile(string text)
        {
            using (var writer = new StreamWriter("log.txt", append: true))
                writer.WriteLine(text);
        }

        static void ExampleWithStaticAndInstance()
        {
            Callback cbStatic = new Callback(WriteToMsgBox);
            Program p = new Program();
            Callback cbInstance = new Callback(p.WriteToFile);
            LogCommands(cbStatic);
            LogCommands(cbInstance);
        }

        static void ExampleWithChain()
        {
            Program p = new Program();

            Callback cbChain = null;
            Callback cb1 = WriteToConsole;
            Callback cb2 = WriteToMsgBox;
            Callback cb3 = p.WriteToFile;

            cbChain += cb1;
            cbChain += cb2;
            cbChain += cb3;

            LogCommands(cbChain);

            cbChain -= WriteToMsgBox;

            LogCommands(cbChain);
        }

        public static void SafeInvoke(Callback callback, string str)
        {
            if (callback != null)
            {
                Delegate[] delegates = callback.GetInvocationList();
                List<Exception> errors = null;
                foreach (Callback d in delegates)
                {
                    try
                    {
                        d(str);
                    }
                    catch (Exception ex)
                    {
                        if (errors == null)
                            errors = new List<Exception>();
                        errors.Add(ex);
                    }
                }
                if (errors != null)
                    throw new AggregateException(errors);
            }
        }

        static void Main(string[] args)
        {
            ExampleWithStaticAndInstance();
            ExampleWithChain();
            Console.ReadLine();
        }
    }
}
