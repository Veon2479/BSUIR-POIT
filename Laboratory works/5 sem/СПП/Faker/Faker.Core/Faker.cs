using System.Runtime.CompilerServices;
using Faker.Core.Generators;

namespace Faker.Core;

public class Faker
{
    private readonly List<IValueGenerator> _generatorList = new();
    private readonly IValueGenerator _objectGenerator = new ObjectGenerator();
    public readonly long FakerId;
    public Faker()
    {
        FakerId = this.GetHashCode();
        
        _generatorList.Add(new BoolGenerator());
        _generatorList.Add(new ByteGenerator());
        _generatorList.Add(new CharGenerator());
        _generatorList.Add(new DecimalGenerator());
        _generatorList.Add(new DoubleGenerator());
        _generatorList.Add(new FloatGenerator());
        _generatorList.Add(new IntGenerator());
        _generatorList.Add(new LongGenerator());
        _generatorList.Add(new SbyteGenerator());
        _generatorList.Add(new ShortGenerator());
        _generatorList.Add(new StringGenerator());
        _generatorList.Add(new UintGenerator());
        _generatorList.Add(new UlongGenerator());
        _generatorList.Add(new UshortGenerator());
        _generatorList.Add(new ListGenerator());
    }
    
    private IValueGenerator GetGenerator(Type type)
    {
        bool isFound = false;
        int i = 0;
        while (i < _generatorList.Count && !isFound )
        {
            if (_generatorList[i].CanGenerate(type))
                isFound = true;
            if (!isFound)
                i++;
        }

        if (!isFound && i == _generatorList.Count)
            return _objectGenerator;
        else
            return _generatorList[i];
    }
    
    public object? Create<T>(GeneratorContext context)
    {
        return Create(typeof(T), context);
    }

    public object? Create(Type type, GeneratorContext context)
    {
        if (context == null)
        {
            throw new FakerException("No context is provided");
        }
        return GetGenerator(type).Generate(type, context);
    }
}