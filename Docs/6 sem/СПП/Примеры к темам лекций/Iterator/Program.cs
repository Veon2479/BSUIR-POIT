using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace Iterator
{
    class Program
    {
        static void Main()
        {
            int[] numbers = new int[5] { 0, 1, 2, 3, 4 };

            foreach (int a in numbers)
            {
                Console.WriteLine(a);
            }

            IEnumerator it = numbers.GetEnumerator();
            while (it.MoveNext())
            {
                int a = (int)it.Current;
                Console.WriteLine(a);
            }

            var numberList = new List<int> { 0, 1, 2, 3, 4 };
            foreach (int a in numberList)
            {
                Console.WriteLine(a);
            }

            using (IEnumerator<int> itl = numberList.GetEnumerator())
            {
                while (itl.MoveNext())
                {
                    int a = itl.Current;
                    Console.WriteLine(a);
                }
            }
        }

        public static void TestStringList()
        {
            var l = new StringList()
            {
                "а", "б", "в", "г", "д"
            };

            foreach (string s in l.GetTop(3))
                Console.WriteLine(s);

            foreach (string s in l.GetTopEx(3))
                Console.WriteLine(s);

            List<string> top10 = l.GetTop(3).ToStringList();
            string[] top10Array = l.GetTop(3).ToArray();

            IEnumerable<string> enumerable = l.GetTopEx(3);
            IEnumerator<string> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string s = enumerator.Current;
                Console.WriteLine(s);
            }
        }

        public static void TestStringListMore()
        {
            var l = new StringList();
            List<string> result = l
                .Where((string t) => !string.IsNullOrWhiteSpace(t))
                .OrderBy((string s) => s, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }
    }

    public class StringList : List<string>
    {
        public IEnumerable<string> GetTop(int topCount)
        {
            int count = Math.Min(Count, topCount);
            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        public IEnumerable<string> GetTopEx(int topCount)
        {
            return new StringListTopCountEnumerable(this, topCount);
        }
    }

    public struct StringListTopCountEnumerable : IEnumerable<string>
    {
        private StringList stringList;
        private int topCount;

        public StringListTopCountEnumerable(StringList stringList, int topCount)
        {
            this.stringList = stringList;
            this.topCount = topCount;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new StringListTopCountEnumerator(stringList, topCount);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct StringListTopCountEnumerator : IEnumerator<string>
    {
        private StringList stringList;
        private int currentIndex;
        private int topCount;

        public StringListTopCountEnumerator(StringList stringList, int topCount)
        {
            this.stringList = stringList;
            this.topCount = topCount;
            currentIndex = -1;
        }

        public string Current => stringList[currentIndex];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < stringList.Count &&
                currentIndex < topCount;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        public void Dispose()
        {
        }
    }

    public static class EnumerableExtensions
    {
        public static StringList ToStringList(
            this IEnumerable<string> enumerator)
        {
            var result = new StringList();
            foreach (string s in enumerator)
                result.Add(s);
            return result;
        }

        public static IEnumerable<T> Where2<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
