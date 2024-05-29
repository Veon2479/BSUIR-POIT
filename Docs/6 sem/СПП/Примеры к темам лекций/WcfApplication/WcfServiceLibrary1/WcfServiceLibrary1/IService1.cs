using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    [ServiceContract(Name = "WhetherService", 
        Namespace = "http://www.mycompany.com/whether/2010/05/24")]
    public interface IWhetherService
    {
        [OperationContract]
        double GetTemprature(string location);

        [OperationContract]
        WhetherInfo GetWhetherInfo(string location);
    }

    [DataContract]
    public class WhetherInfo
    {
        public WhetherInfo()
        {
        }

        [DataMember]
        public double Temperature { get; set; }

        [DataMember]
        public double Humidity { get; set; }

        [DataMember]
        public double Pressure { get; set; }

        public double GetTemperatureAsFahrenheit()
        {
            return Temperature * 9 / 5 + 32;
        }
    }
}
