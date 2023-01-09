



// var sh = SerializationHub.GetInstance();

using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Sockets;
using Tracer;
using Tracer.Serialization;

namespace Example;

public class Example
{
    private static void Test(int i)
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        Thread.Sleep(256);
        if (i >= 0)
        {
            Test(i - 1);
            Test(i - 1);
        }
        tracer.StopTrace();
    }

    private static void TestA()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        Thread.Sleep(20);
        TestB();
        TestB();
        tracer.StopTrace();

    }

    private static void TestB()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        Thread.Sleep(40);
        tracer.StopTrace();

    }
    
    private async Task DoAsync()
    {
        await Task.Run(TestA);
    }

    static async Task Main(string[] args)
    {
        SerializationHub sh = SerializationHub.GetInstance();
        var tracer = Tracer.Tracer.GetInstance();
        
        Test(0);
        var example = new Example();
        await example.DoAsync();
        var res = tracer.GetTraceResult();
        var list = sh.GetMethodNames();
        foreach (var i in list)
        {
            sh.Serialize(res, i);
        }
    }
}