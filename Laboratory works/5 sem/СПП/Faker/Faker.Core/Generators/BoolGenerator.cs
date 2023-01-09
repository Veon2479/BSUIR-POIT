namespace Faker.Core.Generators;

public class BoolGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return Convert.ToBoolean(gc.Random.Next() % 2);

    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(bool));
        return res;    
    }
    
}