namespace Faker.Core.Generators;

public class LongGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (long)gc.Random.NextInt64(long.MinValue, long.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(long));
        return res;     }
}