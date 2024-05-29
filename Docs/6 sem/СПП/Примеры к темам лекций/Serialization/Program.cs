using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Serialization
{
    [Serializable]
    public class Person
    {
        public string Name;
        public DateTime Birthday;
        public List<Person> Container;

        [OnSerializing]
        private void Serializing(StreamingContext context)
        {
        }

        [OnSerialized]
        private void Serialized(StreamingContext context)
        {
        }

        [OnDeserializing]
        private void Deserializing(StreamingContext context)
        {
            if (Container == null)
                Console.WriteLine("Container = null");
            else
                Console.WriteLine("Container != null");
        }

        [OnDeserialized]
        private void Deserialized(StreamingContext context)
        {
            bool exists = Container.Contains(this);
            Console.WriteLine("Container.Contains(this) = {0}", exists);
        }

        public Person()
        {
        }
    }

    [Serializable]
    public class Person2 : ISerializable
    {
        public string Name;
        public DateTime Birthday;
        public List<Person2> Container;

        public Person2()
        {
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected Person2(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Birthday = info.GetDateTime("Birthday");
            Container = (List<Person2>)info.GetValue(
                "Container", typeof(List<Person2>));
        }

        void ISerializable.GetObjectData(SerializationInfo info, 
            StreamingContext context)
        {
            info.SetType(this.GetType());
            info.AddValue("Name", Name);
            info.AddValue("Birthday", Birthday);
            info.AddValue("Container", Container, typeof(List<Person2>));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SerializeDeserializePerson();
            Console.WriteLine();
            SerializeDeserializePerson2();
            Console.ReadLine();
        }

        public static void SerializeDeserializePerson()
        {
            List<Person> persons = new List<Person>();
            persons.Add(
            new Person()
            {
                Name = "Петр Петрович",
                Birthday = new DateTime(1985, 1, 1),
                Container = persons
            });
            persons.Add(
                new Person()
                {
                    Name = "Иван Васильевич",
                    Birthday = new DateTime(1987, 12, 31),
                    Container = persons
                });
            MemoryStream stream = SerializeToMemory(persons);

            stream.Position = 0;
            List<Person> deserializedPersons = (List<Person>)DeserializeFromMemory(stream);

            foreach (Person p in deserializedPersons)
            {
                if (p.Container == deserializedPersons)
                    Console.WriteLine("{0}, {1}", p.Name, p.Birthday);
            }
        }

        public static void SerializeDeserializePerson2()
        {
            List<Person2> persons = new List<Person2>();
            persons.Add(
            new Person2()
            {
                Name = "Петр Петрович",
                Birthday = new DateTime(1985, 1, 1),
                Container = persons
            });
            persons.Add(
                new Person2()
                {
                    Name = "Иван Васильевич",
                    Birthday = new DateTime(1987, 12, 31),
                    Container = persons
                });
            MemoryStream stream = SerializeToMemory(persons);

            stream.Position = 0;
            List<Person2> deserializedPersons = (List<Person2>)DeserializeFromMemory(stream);

            foreach (Person2 p in deserializedPersons)
            {
                if (p.Container == deserializedPersons)
                    Console.WriteLine("{0}, {1}", p.Name, p.Birthday);
            }
        }

        public static MemoryStream SerializeToMemory(object graph)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Context = new StreamingContext(
                StreamingContextStates.All, null);
            formatter.Serialize(stream, graph);
            return stream;
        }

        public static object DeserializeFromMemory(MemoryStream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
    }
}
