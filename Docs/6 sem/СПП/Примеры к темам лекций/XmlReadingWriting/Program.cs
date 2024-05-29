using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XmlReadingWriting
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(path, "Test.xml");
            string targetPath = Path.Combine(path, "Test2.xml");
            string targetPath2 = Path.Combine(path, "Test3.xml");
            string targetPath3 = Path.Combine(path, "Test3.xml");

            ReadWriteXml(targetPath3);
            ReadWriteXmlDocument(filePath, targetPath);
            SerializeXml(targetPath2);
            DeserializeXml(targetPath2);
        }

        public static void ReadWriteXmlDocument(
            string filePath, string targetPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList nodes = xmlDoc.SelectNodes("StudentGroup/Student");
            Console.WriteLine("Найдено {0} элементов", nodes.Count);
            xmlDoc.Save(targetPath);
        }

        public static void SerializeXml(string filePath)
        {
            StudentGroup group = CreateStudentGroup();
            XmlSerializer serializer = new XmlSerializer(typeof(StudentGroup));
            using (XmlWriter writer = XmlWriter.Create(filePath,
                new XmlWriterSettings()
                {
                    Indent = true,
                    NewLineOnAttributes = true
                }))
            {
                serializer.Serialize(writer, group);
            }
        }

        public static void DeserializeXml(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(StudentGroup));
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                StudentGroup group = (StudentGroup)deserializer.Deserialize(reader);
            }
        }

        private static StudentGroup CreateStudentGroup()
        {
            StudentGroup group = new StudentGroup()
            {
                Students = new List<Student>() 
                {
                    new Student()
                    {
                        FirstName = "Андрей",
                        SecondName = "Иванов",
                        Sex = 'М',
                        Birthday = new DateTime(1985, 1, 1),
                        Rating = 9
                    },
                    new Student()
                    {
                        FirstName = "Елена",
                        SecondName = "Петрова",
                        Sex = 'Ж',
                        Birthday = new DateTime(1986, 12, 31),
                        Rating = 9
                    }
                }
            };
            return group;
        }

        public static void ReadWriteXml(string targetPath)
        {
            using (XmlWriter writer = XmlWriter.Create(targetPath,
                new XmlWriterSettings()
                {
                    Indent = true,
                    NewLineOnAttributes = true
                }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("StudentGroup");
                writer.WriteStartElement("Student");
                writer.WriteEndElement();
                writer.WriteStartElement("Student");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            using (XmlReader reader = XmlReader.Create(targetPath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                    }
                }
            }
        }
    }

    [XmlRoot]
    public class Student
    {
        [XmlAttribute(AttributeName = "Name")]
        public string FirstName;
        [XmlAttribute(AttributeName = "Surname")]
        public string SecondName;
        [XmlAttribute]
        public char Sex;
        [XmlAttribute]
        public DateTime Birthday;
        [XmlAttribute]
        public int Rating;
    }

    [XmlRoot]
    public class StudentGroup
    {
        [XmlArray]
        [XmlArrayItem(ElementName = "Student")]
        public List<Student> Students;
    }
}
