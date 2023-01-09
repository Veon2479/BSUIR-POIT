namespace Faker.Core.Generators;

public class FloatGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (float)gc.Random.NextDouble();
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(float));
        return res;
    }
}