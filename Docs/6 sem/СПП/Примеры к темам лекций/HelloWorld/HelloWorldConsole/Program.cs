using System;
using MyLibrary;

namespace HelloWorldConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MyClass c = new MyClass();
                string s = c.Concatinate("Ура! ", "Заработало!", " Да?");
                System.Console.WriteLine(s);
                Console.WriteLine("Нажмите Ввод для завершения...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
