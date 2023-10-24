// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

var srv = new TcpListener( IPAddress.Any, 50001 );
srv.Start();
Byte[] bytes = new Byte[ 4 + 8*2 + 8 ];
while (true)
{
    Console.WriteLine("\nPending for a client");
    TcpClient client = srv.AcceptTcpClient();
    Console.WriteLine("Connected!");
    NetworkStream stream = client.GetStream();
    int i = stream.Read(bytes, 0, bytes.Length );
    Console.WriteLine("Now is: "+DateTime.Now.Millisecond+", Then: "+BitConverter.ToInt64(bytes, 20));
    Console.WriteLine("ID is "+BitConverter.ToInt32(bytes, 0));
    Console.WriteLine(BitConverter.ToDouble(bytes, 4)+" "+BitConverter.ToDouble(bytes, 12));
    stream.Write( bytes, 0, bytes.Length );
    client.Close();
    Console.WriteLine("Closed connection");
}