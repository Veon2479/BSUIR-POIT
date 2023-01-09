namespace Faker.Core.Generators;

public class SbyteGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (sbyte)gc.Random.Next(sbyte.MinValue, sbyte.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(sbyte));
        return res;     
    }
}