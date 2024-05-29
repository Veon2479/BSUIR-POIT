using System;

namespace GoF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //GoF.Decorator.Sample.Start();
                //GoF.Proxy.Sample.Start();
                //GoF.Bridge.Sample.Start();
                //GoF.Adapter.Sample.Start();
                //GoF.Singleton.Sample.Start();
                //GoF.SingletonWrong.Sample.Start();
                GoF.Composite.Sample.Start();
                //GoF.Flyweight.Sample.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                //Console.WriteLine("Error: {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
