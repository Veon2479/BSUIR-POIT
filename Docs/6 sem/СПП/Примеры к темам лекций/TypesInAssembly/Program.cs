using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace TypesInAssembly
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string s = Path.GetFullPath(args[0]);
                Assembly a = Assembly.LoadFile(s);
                ListTypesInAssembly(a);
            }
        }

        static void ListTypesInAssembly(Assembly assembly)
        {
            Type[] ta = assembly.GetTypes();
            List<Type> tl = new List<Type>(ta);
            tl.Sort(CompareTypesByNamespace);
            foreach (Type t in tl)
            {
                Console.WriteLine(t.FullName);
            }
        }

        static int CompareTypesByNamespace(Type a, Type b)
        {
            if (a != null && b != null)
            {
                return a.Namespace.CompareTo(b.Namespace);
            }
            else
                throw new ArgumentNullException();
        }
    }

    public class MyClass1
    {
    }

    public class MyClass2
    {
    }
}
