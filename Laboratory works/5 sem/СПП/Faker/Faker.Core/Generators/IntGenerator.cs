namespace Faker.Core.Generators;

public class IntGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (int)gc.Random.Next(int.MinValue, int.MaxValue);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(int));
        return res;     }
}