namespace Faker.Core;

public interface IValueGenerator
{
    object? Generate<T>(GeneratorContext gc)
    {
        return Generate(typeof(T), gc);
    }
    object? Generate(Type type, GeneratorContext gc);

    bool CanGenerate<T>()
    {
        return CanGenerate(typeof(T));
    }
    bool CanGenerate(Type type);
}