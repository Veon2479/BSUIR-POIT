using System.Collections;

namespace Faker.Core.Generators;

public class ListGenerator : IValueGenerator
{
    public object? Generate<T>(GeneratorContext gc)
    {
        return Generate(typeof(T), gc);
    }

    public object? Generate(Type type, GeneratorContext gc)
    {
        IList? l = null;
        if (gc.RecursionGuardian.AddInstance(type, gc.Faker))
        {
            try
            {
                l = (IList?)Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                l = null;
            }

            if (l != null)
            {
                Type t = type.GetGenericArguments()[0];
                for (var i = 0; i < gc.Random.Next(2, 6); i++)
                {
                    var obj = gc.Faker.Create(t, gc);
                    l.Add(obj);
                }
            }
            var isDone = gc.RecursionGuardian.RemoveInstance(type, gc.Faker);
            if (!isDone)
                throw new FakerException("Consistency of inner data structure is broken!");
        }
        return l;
    }

    public bool CanGenerate(Type type)
    {
        bool res = (type == typeof(IList));
        return res; 
    }
}