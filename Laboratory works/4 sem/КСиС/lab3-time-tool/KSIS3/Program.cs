using KSIS3;

class ksis3
{
    //args[0] - mode (client (/c) or server (/s))
    //args[1] - server ip for client mode
    public static void Main(string[] args)
    {
        int prms = args.Length;
        switch (prms)
        {
            case 1:
                if (args[0] == "-s")
                {
                    var server = new Server();
                    server.start();
                }
                else
                {
                    Console.WriteLine("ERR: incorrect parameters");
                }
                break;
            case 2:
                if (args[0] == "-c")
                {
                    var client = new Client(args[1]);
                    var time = client.getTime();
                }
                else
                {
                    Console.WriteLine("ERR: incorrect parameters");
                }
                break;
            default:
                Console.WriteLine("ERR: incorrect number of parameters");
                break;
        }
        
        Console.WriteLine("LOG: exit");
        Console.ReadLine();
    }
}