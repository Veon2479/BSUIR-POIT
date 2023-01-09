namespace Faker.Core;

public class GeneratorContext
{
    public Random Random { get; }
    public Faker Faker { get; }
    public RecursionGuardian RecursionGuardian { get; }

    public GeneratorContext(Random random, Faker faker, int maxRecursionDepth)
    {
        Random = random;
        Faker = faker;
        RecursionGuardian = new RecursionGuardian(faker, maxRecursionDepth);
    }

}