namespace Faker.Core.Generators;

public class CharGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        return (char)gc.Random.Next();
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(char));
        return res;    
    }
}