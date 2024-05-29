using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Syndication;

namespace WcfServiceLibrary1
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WhetherService : IWhetherService
    {
        private string LastLocation;

        public double GetTemperature(string location)
        {
            Console.WriteLine("Last location: {0}", LastLocation);
            LastLocation = location;
            if (string.Compare(location, "Minsk", true) == 0)
                return +15;
            else if (string.Compare(location, "Vitebsk", true) == 0)
                return +13;
            else 
                return double.MinValue;
        }

        public WhetherInfo GetWhetherInfo(string location)
        {
            Console.WriteLine("Last location: {0}", LastLocation);
            LastLocation = location;
            if (string.Compare(location, "Minsk", true) == 0)
            {
                var res = new WhetherInfo()
                {
                    Temperature = 15,
                    Humidity = 50,
                    Pressure = 760,
                };
                return res;
            }
            else if (string.Compare(location, "Vitebsk", true) == 0)
            {
                var res = new WhetherInfo()
                {
                    Temperature = 13,
                    Humidity = 48,
                    Pressure = 750,
                };
                return res;
            }
            else
                return null;
        }

        public Stream GetWhetherInfo2(string location, string resultType)
        {
            switch (resultType)
            {
                case "html":
                    return GetWhetherInfoAsHtml(location);
                case "text":
                    return GetWhetherInfoAsText(location);
                case "json":
                    return GetWhetherInfoAsJson(location);
                case "xml":
                default:
                    return GetWhetherInfoAsCustomXml(location);
            }
        }

        public SyndicationFeedFormatter GetWhetherInfoFeed(string format)
        {
            string[] locations = { "Minsk", "Vitebsk" };
            //WhetherInfo res = GetWhetherInfo(location);
            var feedData = new SyndicationFeed("WhetherInfo", 
                "Whether information for the given location",
                new Uri("http://localhost:8080/WhetherService/whether/feed"));
            List<SyndicationItem> items = new List<SyndicationItem>();
            foreach (string location in locations)
            {
                SyndicationItem item = new SyndicationItem(location, 
                    string.Format("{0} temperature: {1}", 
                        location, GetTemperature(location)),
                    new Uri("http://localhost:8080/WhetherService/whether/get?loc="
                        + location), "ItemID", DateTime.Now);
                items.Add(item);
            }
            feedData.Items = items;
            SyndicationFeedFormatter feed;
            if (string.Compare(format, "rss20", true) == 0)
                feed = new Rss20FeedFormatter(feedData);
            else
                feed = new Atom10FeedFormatter(feedData);
            return feed;
        }

        private Stream GetWhetherInfoAsText(string location)
        {
            WhetherInfo res = GetWhetherInfo(location);
            if (res != null)
            {
                var stream = new MemoryStream();
                var t = new StreamWriter(stream, Encoding.UTF8);
                t.WriteLine("Temperature: {0}", res.Temperature);
                t.WriteLine("Humidity: {0}", res.Humidity);
                t.WriteLine("Pressure: {0}", res.Pressure);
                t.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
                return null;
        }

        private Stream GetWhetherInfoAsCustomXml(string location)
        {
            WhetherInfo res = GetWhetherInfo(location);
            if (res != null)
            {
                var stream = new MemoryStream();
                var t = new XmlSerializer(typeof(WhetherInfo));
                t.Serialize(stream, res);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
                return null;
        }

        private Stream GetWhetherInfoAsJson(string location)
        {
            WhetherInfo res = GetWhetherInfo(location);
            if (res != null)
            {
                var stream = new MemoryStream();
                DataContractJsonSerializer s = 
                    new DataContractJsonSerializer(typeof(WhetherInfo));
                s.WriteObject(stream, res);
                stream.Seek(0, SeekOrigin.Begin);
                WebOperationContext.Current.OutgoingResponse.ContentType = 
                    "application/json; charset=utf-8";
                return stream;
            }
            else
                return null;
        }

        private Stream GetWhetherInfoAsHtml(string location)
        {
            //OperationContext.Current.OutgoingMessageHeaders.Add(
            //    MessageHeader.CreateHeader("charset", "", "utf-8 text/plain"));
            Console.WriteLine("Last location: {0}", LastLocation);
            LastLocation = location;
            if (string.Compare(location, "Minsk", true) == 0)
            {
                var res = new WhetherInfo()
                {
                    Temperature = 15,
                    Humidity = 50,
                    Pressure = 760,
                };
                var stream = new MemoryStream();
                var t = new StreamWriter(stream, Encoding.UTF8);
                t.WriteLine("<html><body>");
                t.WriteLine("Temperature: {0}", res.Temperature);
                t.WriteLine("Humidity: {0}", res.Humidity);
                t.WriteLine("Pressure: {0}", res.Pressure);
                t.WriteLine("</body></html>");
                t.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
                return null;
        }

        static void Main()
        {
            var host = new WebServiceHost(
                typeof(WhetherService),
                new Uri("http://localhost:8080/WhetherService/"));
            host.Open();
            Console.WriteLine("Press ENTER to stop the service");
            Console.ReadLine();
        }
    }
}
