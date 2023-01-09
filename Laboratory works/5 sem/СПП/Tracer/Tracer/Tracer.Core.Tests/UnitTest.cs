using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Tracer;

namespace Core.Tests;

public class Tests
{

    [Test]
    public void VoidTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();
        var res = tracer.GetTraceResult();
        Assert.AreEqual(res.Children, new List<TraceResult>());
    }

    private void TestRec1(int num)
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        if (num > 0)
            TestRec1(num - 1);
        tracer.StopTrace();
    }
    
    [Test]
    public void SimpleRecursiveTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();
        TestRec1(2);
        var res = tracer.GetTraceResult();
        bool flag = (res.Children.Count == 1) && //head has test(2)
                    (res.Children[0].Children.Count == 1) &&  //test(2) has test(1)
                    (res.Children[0].Children[0].Children.Count == 1); //test(1) has test(0)
        Assert.AreEqual(true, flag);
    }

    private void TestRec2(int num)
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        if (num > 0)
        {
            TestRec2(num - 1);
            TestRec2(num - 1);
        }
        tracer.StopTrace();
    }
    
    [Test]
    public void BinaryRecursiveTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();
        TestRec2(2);
        var res = tracer.GetTraceResult();
        Assert.AreEqual(true, ValidateFoolTree(res.Children[0], 2));
        Assert.AreEqual(2, FindMaxDepth(res.Children[0], 0));

    }

    private int FindMaxDepth(TraceResult node, int curDepth)
    {
        var list = new List<int>();
        foreach (var i in node.Children)
        {
            list.Add(FindMaxDepth(i, curDepth + 1));
        }

        int res = curDepth;
        if (list.Count != 0)
        {
            var tmp = list.Max();
            res = (tmp > res) ? tmp : res;
        }

        return res;
    }

    private bool ValidateFoolTree(TraceResult node, int count)
    {
        bool res = true;
        foreach (var i in node.Children)
        {
            res &= ValidateFoolTree(i, count);
        }

        if (node.Children.Count != 0 && node.Children.Count != count)
            res = false;
        return res;
    }
    
    private static void TestRecN(int N, int num)
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.StartTrace();
        if (num > 0)
        {
            for (int i = 0; i < N; i++)
                TestRecN(N, num - 1);
        }
        tracer.StopTrace();
    }
    
    [Test]
    public void TwoTernaryTreesTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();
        TestRecN(3, 3);
        TestRecN(3, 2);
        var res = tracer.GetTraceResult();
        Assert.AreEqual(3, FindMaxDepth(res.Children[0], 0));
        Assert.AreEqual(true, ValidateFoolTree(res.Children[0], 3));
        
        Assert.AreEqual(2, FindMaxDepth(res.Children[1], 0));
        Assert.AreEqual(true, ValidateFoolTree(res.Children[1], 3));
    }

    [Test]
    public void FiveBinaryTreesTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();

        for (int i = 0; i < 5; i++)
        {
            TestRecN(2, 10-i);
        }
        var res = tracer.GetTraceResult();
        for (int i = 0; i < 5; i++)
        {
            Assert.AreEqual(10 - i, FindMaxDepth(res.Children[i], 0));
            Assert.AreEqual(true, ValidateFoolTree(res.Children[i], 2));
        }
    }

    [Test]
    public void SyncAndAsyncTreesTest()
    {
        var tracer = Tracer.Tracer.GetInstance();
        tracer.ResetTracer();
        TestRecN(2, 4);
        Task.Run(() => TestRecN(3, 5)).Wait();

        var res = tracer.GetTraceResult();
        
        Assert.AreEqual(4, FindMaxDepth(res.Children[0], 0));
        Assert.AreEqual(true, ValidateFoolTree(res.Children[0], 2));
        
        Assert.AreEqual(5, FindMaxDepth(res.Children[1], 0));
        Assert.AreEqual(true, ValidateFoolTree(res.Children[1], 3));
        
        Assert.AreNotEqual(res.Children[0].Tid, res.Children[1].Tid);
    }
    
}