using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            //string s = "Some text";
            //byte[] data = Encoding.Unicode.GetBytes(s);
            //string base64 = Convert.ToBase64String(data);
            //Console.WriteLine(base64);
            byte[] data = new byte[] { 0x02, 0xFF, 0x0D };
            string base64 = Convert.ToBase64String(data);
            Console.WriteLine(base64);

            byte[] data2 = Convert.FromBase64String(base64);
            Console.ReadLine();
        }
    }
}
