namespace Faker.Core.Generators;

public class ByteGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (byte)gc.Random.Next(255);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(byte));
        return res;     
    }
}