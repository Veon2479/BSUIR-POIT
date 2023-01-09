using System.Reflection;

namespace Faker.Core.Generators;

public class ObjectGenerator : IValueGenerator
{
    public object? Generate<T>(GeneratorContext gc)
    {
        // if (typeof(T).IsValueType)
        // {
        //     return Activator.CreateInstance(typeof(T))!;
        // }
        // else
        // {
        //     return null;
        // }
        return Generate(typeof(T), gc);
    }

    public object? Generate(Type type, GeneratorContext gc)
    {
        object? obj = null;
        if (gc.RecursionGuardian.AddInstance(type, gc.Faker))
        {
            if (type.IsValueType)
            {
                obj = Activator.CreateInstance(type);
            }

            // var constructors = type.GetConstructors(
            //     System.Reflection.BindingFlags.Instance |
            //     System.Reflection.BindingFlags.Public).OrderByDescending(
            //     t => t.GetParameters().Length).ToList();
            var constructors = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .ToList();
            
            bool fl = true;
            int i = 0;
            
            while (fl && i < constructors.Count)
            {
                try
                {
                    var args = constructors[i].GetParameters()
                        .Select(p => p.ParameterType)
                        .Select(t => gc.Faker.Create(t, gc))
                        .ToArray();
                    
                    obj = constructors[i].Invoke(args);
                    
                    fl = false;
                }
                catch (Exception)
                {
                    // ignored
                }

                i++;

            }

            if (!fl && obj != null)
            {
                var fields = obj.GetType().GetMembers()
                    .Where(f => f.MemberType == MemberTypes.Field);
                var properties = obj.GetType().GetMembers()
                    .Where(p => p.MemberType == MemberTypes.Property);

                foreach (var f in fields)
                {
                    FieldInfo field = (FieldInfo)f;
                    try
                    {
                        if (Equals(field.GetValue(obj), GetDefaultValue(field.FieldType)))
                        {
                            field.SetValue(obj, gc.Faker.Create(field.FieldType, gc));
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                foreach (var p in properties)
                {
                    PropertyInfo property = (PropertyInfo)p;
                    try
                    {
                        if (Equals(property.GetValue(obj), GetDefaultValue(property.PropertyType)))
                        {
                            property.SetValue(obj, gc.Faker.Create(property.PropertyType, gc));
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            
            var isDone = gc.RecursionGuardian.RemoveInstance(type, gc.Faker);
            if (!isDone)
                throw new FakerException("Consistency of inner data structure is broken!");
        }
        return obj;
    }
    
    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    public bool CanGenerate(Type type)
    {
        return true;
    }
}