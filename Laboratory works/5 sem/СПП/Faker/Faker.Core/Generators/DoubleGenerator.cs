namespace Faker.Core.Generators;

public class DoubleGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return gc.Random.NextDouble();
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(double));
        return res;
    }
}