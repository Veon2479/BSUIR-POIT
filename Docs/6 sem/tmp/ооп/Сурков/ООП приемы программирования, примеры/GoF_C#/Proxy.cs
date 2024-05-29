using System;

namespace GoF.Proxy
{
    public interface IWebClient
    {
        void Request();
    }

    public class HeavyWebClient : IWebClient
    {
        public HeavyWebClient()
        {
            Console.WriteLine("HeavyWebClient created");
        }

        public void Request()
        {
            Console.WriteLine("Request");
        }
    }

    public class VirtualProxy : IWebClient
    {
        private HeavyWebClient _client;

        public void Request()
        {
            if (_client == null)
                _client = new HeavyWebClient();
            _client.Request();
        }
    }

    public class ProtectionProxy : IWebClient
    {
        private HeavyWebClient _client;
        private bool _accessGranted = false;
        private const string PASSWORD = "12345";

        public void Authenticate(string password)
        {
            if (password == PASSWORD)
            {
                Console.WriteLine("Protection Proxy: access granted");
                _accessGranted = true;
            }
            else
                Console.WriteLine("Protection Proxy: access denied");
        }

        public void Request()
        {
            if (_accessGranted)
            {
                if (_client == null)
                    _client = new HeavyWebClient();
                _client.Request();
            }
            else
                Console.WriteLine("Protection Proxy: Request failed. Authenticate first");
        }
    }

    public static class Sample
    {
        public static void UserLogic(IWebClient client)
        {
            client.Request();
        }

        public static void Start()
        {
            // [1]
            {
                Console.WriteLine("-- Without proxy ----------");
                var client = new HeavyWebClient();
                Console.WriteLine("Client created");
                UserLogic(client);
            }

            // [2]
            {
                Console.WriteLine("\n-- Virtual proxy --------");
                var client = new VirtualProxy();
                Console.WriteLine("Client created");
                UserLogic(client);
            }

            // [3]
            {
                Console.WriteLine("\n-- Protection proxy -----");
                var client = new ProtectionProxy();
                Console.WriteLine("Client created");
                UserLogic(client);
                client.Authenticate("12345");
                UserLogic(client);
            }

            // [4]
            // Note, for lazy loading purposes only better to use Lazy<T> class instead of VirtualProxy one (easier)
            {
                Console.WriteLine("\n-- Lazy<T> --------------");
                var client = new Lazy<HeavyWebClient>();
                Console.WriteLine("Client created");
                UserLogic(client.Value); // object is created during first call to Value
            }
        }
    }
}
