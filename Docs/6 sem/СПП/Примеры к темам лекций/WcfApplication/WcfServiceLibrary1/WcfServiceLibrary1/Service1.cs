using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WhetherService : IWhetherService
    {
        private string LastLocation;

        public WhetherService()
        {
            Console.WriteLine("Создан экземпляр WhetherService");
        }

        public double GetTemprature(string location)
        {
            //Console.WriteLine("Last location: {0}", LastLocation);
            LastLocation = location;
            if (string.Compare(location, "Minsk", true) == 0)
                return +15;
            else
                return double.MinValue;
        }

        public WhetherInfo GetWhetherInfo(string location)
        {
            //Console.WriteLine("Last location: {0}", LastLocation);
            LastLocation = location;
            WhetherInfo result = new WhetherInfo();
            if (string.Compare(location, "Minsk", true) == 0)
            {
                result.Temperature = 15;
                result.Humidity = 65;
                result.Pressure = 710;
            }
            else
            {
                result.Temperature = 0;
                result.Humidity = 50;
                result.Pressure = 760;
            };
            return result;
        }
    }

    class Program
    {
        static void Main()
        {
            var host = new ServiceHost(typeof(WhetherService));
            host.Open();
            Console.WriteLine("Press ENTER to stop the service");
            Console.ReadLine();
        }

        //static void Main()
        //{
        //    var host = new ServiceHost(typeof(WhetherService));

        //    var tcpBinding = new NetTcpBinding(SecurityMode.None, false);
        //    tcpBinding.MaxBufferPoolSize = int.MaxValue;
        //    tcpBinding.MaxBufferSize = int.MaxValue;
        //    tcpBinding.MaxReceivedMessageSize = int.MaxValue;
        //    tcpBinding.PortSharingEnabled = false;
        //    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
        //    host.AddServiceEndpoint(
        //        "WcfServiceLibrary1.IWhetherService", 
        //        tcpBinding,
        //        "net.tcp://localhost:5555/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/");
        //    //var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        //    //host.AddServiceEndpoint("WcfServiceLibrary1.IWhetherService", basicHttpBinding,
        //    //    "http://localhost:8731/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/");
        //    var wsHttpBinding = new WSHttpBinding(SecurityMode.Message);
        //    host.AddServiceEndpoint(
        //        "WcfServiceLibrary1.IWhetherService", 
        //        wsHttpBinding,
        //        "http://localhost:8731/Design_Time_Addresses/WcfServiceLibrary1/WhetherService/");

        //    host.Open();
        //    Console.WriteLine("Press ENTER to stop the service");
        //    Console.ReadLine();
        //}
    }
}
