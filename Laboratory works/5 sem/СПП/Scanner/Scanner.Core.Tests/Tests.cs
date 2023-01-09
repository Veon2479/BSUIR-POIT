using System.Text;
using NUnit.Framework;

namespace Scanner.Core.Tests;

public class Tests
{
    [Test]
    public void SingleEmptyFileTest()
    {
        var scanner = new Scanner();
        System.IO.Directory.CreateDirectory("SimpleTest");
        System.IO.File.Create("SimpleTest/test.txt");
        scanner.ScanDirectory("SimpleTest");
        while (scanner.IsRunning()) { }

        var res = scanner.GetResult();
        Assert.AreEqual(1, res.Children.Count);
    }
    
    [Test]
    public void SingleFileTest()
    {
        var scanner = new Scanner();
        System.IO.Directory.CreateDirectory("SimpleHeavyTest");
        string data = "qwertyuiop[]asdfghjkl;'zxcvbnm,./";
        var f = System.IO.File.Open("SimpleHeavyTest/test.txt", System.IO.FileMode.Create);
        f.Write(Encoding.ASCII.GetBytes(data));
        f.Close();
        scanner.ScanDirectory("SimpleHeavyTest");
        while (scanner.IsRunning()) { }

        var res = scanner.GetResult();
        Assert.AreEqual(1, res.Children.Count);
        Assert.NotZero(res.Size);
    }

    [Test]
    public void MultipleEmptyFileTest()
    {
        var scanner = new Scanner();
        System.IO.Directory.CreateDirectory("MultipleTest");
        System.IO.File.Create("MultipleTest/test1.txt");
        System.IO.File.Create("MultipleTest/test2.txt");
        System.IO.File.Create("MultipleTest/test3.txt");

        scanner.ScanDirectory("MultipleTest");
        while (scanner.IsRunning()) { }

        var res = scanner.GetResult();
        Assert.AreEqual(1, res.Children.Count);
        Assert.AreEqual(3, res.Children[0].Children.Count);


    }
}