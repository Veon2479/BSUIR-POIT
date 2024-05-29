using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace ConcurrentCollections
{
    class Program
    {
        static void Main()
        {
            var collection = new ConcurrentDictionary<int, string>();

            string value = collection.GetOrAdd(5, "Невод");
            Console.WriteLine(value);

            string newValue = "Незабудка";
            collection.AddOrUpdate(5, newValue, 
                (int key, string oldValue) =>
                {
                    Console.WriteLine($"Заменяем {oldValue} на {newValue}.");
                    return newValue;
                });

            if (collection.TryGetValue(5, out value))
                Console.WriteLine(value);
        }
    }
}
