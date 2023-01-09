using System;
using NUnit.Framework;

namespace Faker.Tests;

public class SimpleTests
{
    private static readonly Core.Faker Faker = new();
    private static readonly Core.GeneratorContext Gc = new(new Random(), Faker, 5);
    
   

    [Test]
    public void StringTest()
    {
        Assert.IsInstanceOf<string>(Faker.Create<string>(Gc)!);
    }
    
    [Test]
    public void ShortTest()
    {
        Assert.IsInstanceOf<short>(Faker.Create<short>(Gc)!);
    }
    
    [Test]
    public void UshortTest()
    {
        Assert.IsInstanceOf<ushort>(Faker.Create<ushort>(Gc)!);
    }
    
    [Test]
    public void LongTest()
    {
        Assert.IsInstanceOf<long>(Faker.Create<long>(Gc)!);
    }
    
    [Test]
    public void UlongTest()
    {
        Assert.IsInstanceOf<ulong>(Faker.Create<ulong>(Gc)!);
    }
    
    [Test]
    public void IntTest()
    {
        Assert.IsInstanceOf<int>(Faker.Create<int>(Gc)!);
    }
    
    [Test]
    public void UintTest()
    {
        Assert.IsInstanceOf<uint>(Faker.Create<uint>(Gc)!);
    }
    
    [Test]
    public void FloatTest()
    {
        Assert.IsInstanceOf<float>(Faker.Create<float>(Gc)!);
    }
    
    [Test]
    public void DoubleTest()
    {
        Assert.IsInstanceOf<double>(Faker.Create<double>(Gc)!);
    }
    
    [Test]
    public void DecimalTest()
    {
        Assert.IsInstanceOf<decimal>(Faker.Create<decimal>(Gc)!);
    }
    
    [Test]
    public void CharTest()
    {
        Assert.IsInstanceOf<char>(Faker.Create<char>(Gc)!);
    }
    
    [Test]
    public void ByteTest()
    {
        Assert.IsInstanceOf<byte>(Faker.Create<byte>(Gc)!);
    }
    
    [Test]
    public void SbyteTest()
    {
        Assert.IsInstanceOf<sbyte>(Faker.Create<sbyte>(Gc)!);
    }

    [Test]
    public void BoolTest()
    {
        Assert.IsInstanceOf<bool>(Faker.Create<bool>(Gc)!);
    }
}