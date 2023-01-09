using System;
using System.Collections.Generic;
using Faker.Core;
using Faker.Tests.Classes;
using NUnit.Framework;

namespace Faker.Tests;

public class ComplexTests
{
    private static readonly Core.Faker Faker = new();
    private const int MaxRecursionDepth = 5;
    private static readonly Core.GeneratorContext Gc = new(new Random(), Faker, MaxRecursionDepth);

    [Test]
    public void SimpleObjectTest()
    {
        SimpleClass? obj = null;
        try
        {
            obj = (SimpleClass?)Faker.Create(typeof(SimpleClass), Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        Assert.AreNotEqual(null, obj);
    }
    [Test]
    public void SimpleRecursionTest()
    {
        SimpleRecursiveClass? obj = null;
        try
        {
            obj = (SimpleRecursiveClass?)Faker.Create<SimpleRecursiveClass>(Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        for (int i = 0; i <= MaxRecursionDepth; i++)
        {
            Assert.AreNotEqual(null, obj);
            if (obj != null)
                obj = obj.Node;
        }
        Assert.AreEqual(null, obj);

    }
    
    [Test]
    public void ImplicitRecursiveTest()
    {
        ImplRecClass1? obj = null;
        try
        {
            obj = (ImplRecClass1?)Faker.Create<ImplRecClass1>(Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        for (int i = 0; i <= MaxRecursionDepth; i++)
        {
            Assert.AreNotEqual(null, obj);
            Assert.AreNotEqual(null, obj!.Node);
            obj = obj!.Node!.Node;
        }
        Assert.AreEqual(null, obj);
    }
    
    [Test]
    public void SimpleListTest()
    {
        List<SimpleClass?>? obj = null;
        try
        {
            obj = (List<SimpleClass?>?)Faker.Create<List<SimpleClass?>>(Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        Assert.AreNotEqual(null, obj);
        foreach (var t in obj!)
        {
            Assert.AreNotEqual(null, t);
        }
    }
    
    [Test]
    public void RecursiveListTest()
    {
        RecursiveListClass? obj = null;
        try
        {
            obj = (RecursiveListClass?)Faker.Create<RecursiveListClass?>(Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        Assert.AreNotEqual(null, obj);
        
        Assert.AreNotEqual(null, obj!.Node);
        for (int i = 0; i < obj!.Node!.Count; i++)
        {
            Assert.AreNotEqual(null, obj!.Node[i]);
        }
    }

    [Test]
    public void StructureTest()
    {
        TestStruct? obj = null;
        try
        {
            obj = (TestStruct?)Faker.Create(typeof(TestStruct), Gc);
        }
        catch (FakerException e)
        {
            Console.WriteLine(e);
            throw;
        }
        Assert.AreNotEqual(null, obj);
    }
}