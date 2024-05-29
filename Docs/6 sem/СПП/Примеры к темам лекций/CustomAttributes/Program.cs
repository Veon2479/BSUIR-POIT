using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TestMethodAttribute : Attribute
    {
        public TestMethodAttribute()
        {
        }

        public TestMethodAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; set; }
        public int Priority { get; set; }
    }

    public class Tests
    {
        [TestMethod("Функцинальность", Priority = 1)]
        // var attr = new TestMethodAttribute("Performance");
        // attr.Priority = 1;
        static void TestOne()
        {
            Console.WriteLine(nameof(TestOne));
        }

        [TestMethod("Функцинальность", Priority = 1)]
        static void TestTwo()
        {
            Console.WriteLine(nameof(TestTwo));
        }
    }

    public class Program
    {
        static List<MethodInfo> GetTestMethods(Assembly assembly)
        {
            List<MethodInfo> testMethods = new List<MethodInfo>();
            Type[] types = assembly.GetExportedTypes();
            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                foreach (MethodInfo method in methods)
                {
                    TestMethodAttribute[] attributes = (TestMethodAttribute[])method
                        .GetCustomAttributes(typeof(TestMethodAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                        testMethods.Add(method);
                }
            }
            return testMethods;
        }

        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<MethodInfo> methods = GetTestMethods(assembly);
            foreach (MethodInfo method in methods)
            {
                Action testAction = (Action)Delegate.CreateDelegate(typeof(Action), method);
                testAction();
            }
            Console.ReadLine();
        }
    }
}
