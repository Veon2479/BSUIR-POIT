namespace Faker.Core.Generators;

public class DecimalGenerator : IValueGenerator
{
    private int NextInt32(Random rn)
    {
        int firstBits = rn.Next(0, 1 << 4) << 28;
        int lastBits = rn.Next(0, 1 << 28);
        return firstBits | lastBits;
    }
    public object? Generate(Type type, GeneratorContext gc)
    {
        byte scale = (byte) gc.Random.Next(29);
        bool sign = gc.Random.Next(2) == 1;
        return new decimal(NextInt32(gc.Random), 
                        NextInt32(gc.Random),
                        NextInt32(gc.Random),
                        sign,
                        scale);
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(decimal));
        return res;
        
    }
}