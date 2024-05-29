using System;

namespace Example
{
    class Program
    {
        static void Main()
        {
            string json = "{\"Country\": \"Беларусь\", \"City\": \"Минск\"}";
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            Console.WriteLine(obj.Country);
            Console.WriteLine(obj.City);
        }
    }
}