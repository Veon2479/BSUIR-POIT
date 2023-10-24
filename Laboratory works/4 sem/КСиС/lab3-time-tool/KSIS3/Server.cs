using System.Net;
using System.Net.Sockets;


namespace KSIS3;

public class Server
{
    private Socket server;
    public Server()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 37);
        server.Bind(ipEndPoint);
        Console.WriteLine("LOG: server is ready");
    }

    public void start()
    {
        Console.WriteLine("LOG: server started\n");
        byte[] data = new byte[256];
        EndPoint clientIp = new IPEndPoint(IPAddress.Any, 0);   //0 means any port
        int count;
        uint time;
        DateTime Era = new DateTime(1970, 1, 1);
        while (true)
        {
            Console.WriteLine("LOG: waiting for connection");
            
            count = server.ReceiveFrom(data, ref clientIp);
            Console.WriteLine("LOG: request from " + clientIp.ToString() );
            
            
            time = (uint) DateTime.Now.Subtract(Era).TotalSeconds;
            count = server.SendTo( BitConverter.GetBytes(time), clientIp);
            Console.WriteLine("LOG: sent current time: " + time + "\n");

        }
    }
}