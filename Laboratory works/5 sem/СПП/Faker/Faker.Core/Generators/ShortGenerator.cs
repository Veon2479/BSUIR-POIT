namespace Faker.Core.Generators;

public class ShortGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (short)gc.Random.Next(short.MinValue, short.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(short));
        return res;     
    }
}