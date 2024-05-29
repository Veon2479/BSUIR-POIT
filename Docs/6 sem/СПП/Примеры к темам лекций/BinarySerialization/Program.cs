using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Student> studentGroup = CreateStudentGroup();
            //SaveStudentGroup(@"P:\My\Academy\Code.MyLectures.DotNet\BinarySerialization\Test.bin",
            //    studentGroup);
            //Dictionary<string, Student> studentGroup2 =
            //    LoadStudentGroup(@"P:\My\Academy\Code.MyLectures.DotNet\BinarySerialization\Test.bin");
            //foreach (Student t in studentGroup2.Values)
            //    Console.WriteLine(t.ToString());

            SaveGroup(@"P:\My\Academy\Code.MyLectures.DotNet\BinarySerialization\Test.bin", studentGroup);
            Dictionary<string, Student> studentGroup3 =
                LoadGroup(@"P:\My\Academy\Code.MyLectures.DotNet\BinarySerialization\Test.bin");
            foreach (Student t in studentGroup3.Values)
                Console.WriteLine(t.ToString());
            Console.ReadLine();
        }

        public static void SaveGroup(string filePath, Dictionary<string, Student> group)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                binFormatter.Serialize(fs, group);
        }

        public static Dictionary<string, Student> LoadGroup(string filePath)
        {
            Dictionary<string, Student> result;
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                result = (Dictionary<string, Student>)binFormatter.Deserialize(fs);
            return result;
        }

        private static Dictionary<string, Student> CreateStudentGroup()
        {
            Dictionary<string, Student> studentGroup = 
                new Dictionary<string, Student>();
            studentGroup.Add("Сергей Иванов",
                new Student()
                {
                    FirstName = "Sergey",
                    SecondName = "Ivanov",
                    Birthday = new DateTime(1993, 5, 31),
                    Group = 951001,
                    Rating = 8.5
                });
            studentGroup.Add("Дарья Петрова",
                new Student()
                {
                    FirstName = "Dasha",
                    SecondName = "Petrova",
                    Birthday = new DateTime(1993, 6, 1),
                    Group = 951005,
                    Rating = 9.3
                });
            return studentGroup;
        }

        public static void SaveStudentGroup(string filePath, Dictionary<string, Student> group)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                foreach (Student t in group.Values)
                    t.SaveToStream(fs);
            }
        }

        public static Dictionary<string, Student> LoadStudentGroup(string filePath)
        {
            Dictionary<string, Student> result = new Dictionary<string, Student>();
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                Student s = Student.LoadFromStream(fs);
                result.Add("1", s);
                s = Student.LoadFromStream(fs);
                result.Add("2", s);
            }
            return result;
        }
    }

    [Serializable]
    public class Student
    {
        public string FirstName;
        public string SecondName;
        public DateTime Birthday;
        public int Group;
        public double Rating;
        [NonSerialized]
        public string Description;

        public void SaveToStream(FileStream stream)
        {
            byte[] data = Encoding.Unicode.GetBytes(FirstName);
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            stream.Write(dataLength, 0, dataLength.Length);
            stream.Write(data, 0, data.Length);
            data = Encoding.Unicode.GetBytes(SecondName);
            dataLength = BitConverter.GetBytes(data.Length);
            stream.Write(dataLength, 0, dataLength.Length);
            stream.Write(data, 0, data.Length);
            data = BitConverter.GetBytes(Group);
            stream.Write(data, 0, data.Length);
            data = BitConverter.GetBytes(Birthday.ToBinary());
            stream.Write(data, 0, data.Length);
            data = BitConverter.GetBytes(Rating);
            stream.Write(data, 0, data.Length);
        }

        public static Student LoadFromStream(FileStream stream)
        {
            Student result = new Student();
            byte[] data = new byte[1024 * 64];
            stream.Read(data, 0, 4);
            int len = BitConverter.ToInt32(data, 0);
            stream.Read(data, 0, len);
            result.FirstName = Encoding.Unicode.GetString(data, 0, len);
            stream.Read(data, 0, 4);
            len = BitConverter.ToInt32(data, 0);
            stream.Read(data, 0, len);
            result.SecondName = Encoding.Unicode.GetString(data, 0, len);
            stream.Read(data, 0, 4);
            result.Group = BitConverter.ToInt32(data, 0);
            stream.Read(data, 0, 8);
            result.Birthday = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            stream.Read(data, 0, 8);
            result.Rating = BitConverter.ToDouble(data, 0);
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}, группа {2}, родился {3}, средний балл {4}", 
                FirstName, SecondName, Group, Birthday, Rating);
        }
    }
}
