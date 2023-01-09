namespace Faker.Core.Generators;

public class UlongGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (ulong)(gc.Random.NextInt64(long.MaxValue) * 2 - gc.Random.Next(1));
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(ulong));
        return res;     
    }
}