using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceClient.ServiceReference1;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Diagnostics;

namespace WcfServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var client = new WhetherServiceClient(
            //    new NetTcpBinding(SecurityMode.None, false),
            //    new EndpointAddress("net.tcp://localhost:5555/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/"));
            //var t = client.GetTemprature("Minsk");
            //var w = client.GetWhetherInfo("Minsk");
            //Console.WriteLine("Temperature: {0}, Humidity: {1}, Pressure: {2}",
            //    w.Temperature, w.Humidity, w.Pressure);

            //var sw = Stopwatch.StartNew();
            //var count = 1000;
            //for (int i = 0; i < count; i++)
            //{
            //    var wh = client.GetWhetherInfo("Minsk");
            //}
            //Console.WriteLine("{0} calls of GetTemprature(\"Minsk\") took {1}", count, sw.Elapsed);
            //Console.WriteLine("1 call of GetTemprature(\"Minsk\") takes {0} ms", (double)sw.ElapsedMilliseconds / count);

            //client.Close();

            //client = new WhetherServiceClient(
            //    new WSHttpBinding(SecurityMode.Message),
            //    new EndpointAddress("http://localhost:8731/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/"));
            //t = client.GetTemprature("Minsk");
            //w = client.GetWhetherInfo("Minsk");
            //Console.WriteLine("Temperature: {0}, Humidity: {1}, Pressure: {2}",
            //    w.Temperature, w.Humidity, w.Pressure);

            //sw = Stopwatch.StartNew();
            //for (int i = 0; i < count; i++)
            //{
            //    var wh = client.GetWhetherInfo("Minsk");
            //}
            //Console.WriteLine("{0} calls of GetTemprature(\"Minsk\") took {1}", count, sw.Elapsed);
            //Console.WriteLine("1 call of GetTemprature(\"Minsk\") takes {0} ms", (double)sw.ElapsedMilliseconds / count);

            //client.Close();

            //client = new WhetherServiceClient(
            //    new BasicHttpBinding(),
            //    new EndpointAddress("http://localhost:8731/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/"));
            //t = client.GetTemprature("Minsk");
            //w = client.GetWhetherInfo("Minsk");
            //Console.WriteLine("Temperature: {0}, Humidity: {1}, Pressure: {2}",
            //    w.Temperature, w.Humidity, w.Pressure);

            //sw = Stopwatch.StartNew();
            //for (int i = 0; i < count; i++)
            //{
            //    var wh = client.GetWhetherInfo("Minsk");
            //}
            //Console.WriteLine("{0} calls of GetTemprature(\"Minsk\") took {1}", count, sw.Elapsed);
            //Console.WriteLine("1 call of GetTemprature(\"Minsk\") takes {0} ms", (double)sw.ElapsedMilliseconds / count);

            //client.Close();

            var client = new WhetherServiceClient();
            var t = client.GetTemprature("Minsk");
            var w = client.GetWhetherInfo("Minsk");
            Console.WriteLine("Temperature: {0}, Humidity: {1}, Pressure: {2}",
                w.Temperature, w.Humidity, w.Pressure);
            client.Close();

            Console.ReadLine();
        }
    }
}
