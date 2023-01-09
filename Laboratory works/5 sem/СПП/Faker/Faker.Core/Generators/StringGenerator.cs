namespace Faker.Core.Generators;

public class StringGenerator : IValueGenerator
{
    public object? Generate(Type type, GeneratorContext gc)
    {
        var res = "";
        for (var i = 0; i < gc.Random.Next() % 100; i++)
        {
            res += (char)gc.Random.Next();
        }
        return res;
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(string));
        return res;
    }
}