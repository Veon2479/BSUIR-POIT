using System;

namespace Memory
{
    public class Parser
    {
        private string text; 
        private int pos;

        public Parser(string text) { this.text = text; }

        public string NextTokenAsString()
        {
            int start = pos;
            while (pos < text.Length && text[pos] != ' ') 
                pos++;
            return text.Substring(start, pos++ - start); // Создание новой строки
        }

        public ReadOnlyMemory<char> NextTokenAsMemory()
        {
            int start = pos;
            while (pos < text.Length && text[pos] != ' ')
                pos++;
            return text.AsMemory(start, pos++ - start); // Ссылка на фрагмент строки
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser("aaa bbb ccc");
            Console.WriteLine(parser.NextTokenAsMemory());
            Console.WriteLine(parser.NextTokenAsMemory());
            Console.WriteLine(parser.NextTokenAsMemory());
        }
    }
}
