using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.ServiceModel.Syndication;

// http://localhost:8080/WhetherService/temperature/get?loc=Minsk
// http://localhost:8080/WhetherService/whether/get?loc=Minsk
// http://localhost:8080/WhetherService/whether2/get?loc=Minsk&type=xml
// http://localhost:8080/WhetherService/whether/feed?format=rss20
// http://localhost:8080/WhetherService/whether/feed?format=atom10

namespace WcfServiceLibrary1
{
    [ServiceContract(Name = "WhetherService", 
        Namespace = "http://www.mycompany.com/whether/2010/05/24")]
    public interface IWhetherService
    {
        [WebGet(UriTemplate = "temperature/get?loc={location}")]
        [OperationContract]
        double GetTemperature(string location);

        [WebInvoke(Method = "GET", 
            UriTemplate = "whether/get?loc={location}", 
            ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        WhetherInfo GetWhetherInfo(string location);

        [WebInvoke(Method = "GET",
            UriTemplate = "whether2/get?loc={location}&type={resultType}")]
        [OperationContract]
        Stream GetWhetherInfo2(string location, string resultType);

        [WebInvoke(Method = "GET",
            UriTemplate = "whether/feed?format={format}")]
        [OperationContract]
        [ServiceKnownType(typeof(Atom10FeedFormatter))]
        [ServiceKnownType(typeof(Rss20FeedFormatter))]
        SyndicationFeedFormatter GetWhetherInfoFeed(string format);
    }

    [XmlRoot]
    [DataContract]
    public class WhetherInfo
    {
        public WhetherInfo()
        {
        }

        [XmlAttribute]
        [DataMember]
        public double Temperature { get; set; }

        [XmlAttribute]
        [DataMember]
        public double Humidity { get; set; }

        [XmlAttribute]
        [DataMember]
        public double Pressure { get; set; }
    }
}
