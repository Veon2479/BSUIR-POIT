namespace Faker.Core.Generators;

public class UshortGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (ushort)gc.Random.Next(ushort.MinValue, ushort.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(ushort));
        return res;     
    }
}