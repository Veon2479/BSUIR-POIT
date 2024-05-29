using System;
using System.Threading;
using System.Reflection;
using System.Runtime.Remoting;

namespace Example
{
    class Program
    {
        static void Main()
        {
            string callingDomainName = Thread.GetDomain().FriendlyName;
            Console.WriteLine("Имя домена по умолчанию: " + callingDomainName);

            Console.WriteLine();
            TestMarshalableByRefObject();
            Console.WriteLine();
            TestMarshalableByValueObject();
            Console.WriteLine();
            TestNonMarshalableObject();
        }

        static void TestMarshalableByRefObject()
        {
            AppDomain anotherDomain = AppDomain.CreateDomain("Домен-2");
            MyMarshalableByRefObject mbrt = (MyMarshalableByRefObject)anotherDomain
                .CreateInstanceAndUnwrap(Assembly.GetEntryAssembly().FullName,
                    "Example.MyMarshalableByRefObject");
            Console.WriteLine("Тип данных: " + mbrt.GetType()); // CLR "обманывает" программу
            Console.WriteLine("Является посредником: " + RemotingServices.IsTransparentProxy(mbrt));
            mbrt.TestMethod();
            AppDomain.Unload(anotherDomain);
            try
            {
                mbrt.TestMethod();
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Ошибка: домен был выгружен.");
            }
        }

        static void TestMarshalableByValueObject()
        {
            AppDomain anotherDomain = AppDomain.CreateDomain("Домен-2");
            MyMarshalableByRefObject mbrt = (MyMarshalableByRefObject)anotherDomain
                .CreateInstanceAndUnwrap(Assembly.GetEntryAssembly().FullName,
                    "Example.MyMarshalableByRefObject");
            MyMarshalableByValueObject mbvt = mbrt.CreateSerializableObject();
            Console.WriteLine("Является посредником: " + RemotingServices.IsTransparentProxy(mbvt));
            Console.WriteLine("Время создания возвращенного объекта: " + mbvt.ToString());
            AppDomain.Unload(anotherDomain);
            Console.WriteLine("Время создания возвращенного объекта: " + mbvt.ToString());
            Console.WriteLine("Успешное обращение к возвращенному объекту после выгрузки домена.");
        }

        static void TestNonMarshalableObject()
        {
            AppDomain anotherDomain = AppDomain.CreateDomain("Домен-2");
            MyMarshalableByRefObject mbrt = (MyMarshalableByRefObject)anotherDomain
                .CreateInstanceAndUnwrap(Assembly.GetEntryAssembly().FullName,
                    "Example.MyMarshalableByRefObject");
            mbrt.CreateNonSerializableObject(); // Исключение
        }
    }

    public class MyMarshalableByRefObject : MarshalByRefObject
    {
        public MyMarshalableByRefObject()
        {
            Console.WriteLine("Конструктор {0} запущен в домене {1}",
                this.GetType().ToString(), Thread.GetDomain().FriendlyName);
        }

        public void TestMethod()
        {
            Console.WriteLine("Метод TestMethod запущен в домене " +
                Thread.GetDomain().FriendlyName);
        }

        public MyMarshalableByValueObject CreateSerializableObject()
        {
            Console.WriteLine("Метод CreateSerializableObject запущен в домене " +
                Thread.GetDomain().FriendlyName);
            return new MyMarshalableByValueObject();
        }

        public MyNonMarshalableObject CreateNonSerializableObject()
        {
            Console.WriteLine("Метод CreateNonSerializableObject запущен в домене " +
                Thread.GetDomain().FriendlyName);
            MyNonMarshalableObject result = new MyNonMarshalableObject();
            Console.WriteLine("MyNonMarshalableObject объект создан");
            return result; // Ошибка при попытке вернуть объект в другой домен
        }
    }

    [Serializable]
    public class MyMarshalableByValueObject : Object
    {
        private DateTime creationTime = DateTime.Now;

        public MyMarshalableByValueObject()
        {
            Console.WriteLine("Конструктор {0} запущен в домене {1}, время: {2:D}",
                this.GetType().ToString(),
                Thread.GetDomain().FriendlyName,
                creationTime);
        }

        public override string ToString()
        {
            return creationTime.ToLongDateString();
        }
    }

    public class MyNonMarshalableObject : Object
    {
        public MyNonMarshalableObject()
        {
            Console.WriteLine("Конструктор {0} запущен в потоке {1}",
                this.GetType().ToString(), Thread.GetDomain().FriendlyName);
        }
    }
}
