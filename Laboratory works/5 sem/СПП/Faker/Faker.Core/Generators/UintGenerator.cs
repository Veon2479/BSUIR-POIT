namespace Faker.Core.Generators;

public class UintGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (uint)(gc.Random.NextInt64(uint.MaxValue + uint.MinValue) - uint.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(uint));
        return res;     
    }
}