using System.Net;
using System.Net.Sockets;

namespace KSIS3;

public class Client
{

    private Socket client;
    private IPEndPoint serverIP;
    private uint time;
    public Client(string sAddr)
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
        client.Bind(ipEndPoint);
        serverIP = new IPEndPoint(IPAddress.Parse(sAddr), 37);
        Console.WriteLine("LOG: client is ready");
    }

    public uint getTime()
    {
        Console.WriteLine("LOG: requesting server");
        byte[] req = new byte[0];
        client.SendTo(req, serverIP);
        byte[] msg = new byte[sizeof(uint)];
        EndPoint tmpIP = new IPEndPoint(IPAddress.Any, 0);
        Console.WriteLine("LOG: waiting for time");
        int code = client.ReceiveFrom(msg, ref tmpIP);
        time = BitConverter.ToUInt32(msg);
        Console.WriteLine("LOG: received time is: " + time );
        Console.WriteLine("LOG: system time is: " + DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        return time;

    }
}