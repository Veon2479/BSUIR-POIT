using System;
using System.Collections.Generic;

namespace GoF.Flyweight
{
    public class CharacterFactory
    {
        private readonly Dictionary<char, Character> _chars = new Dictionary<char, Character>();

        public Character GetCharacter(char key)
        {
            if (!_chars.ContainsKey(key))
            {
                _chars.Add(key, new Character(key));
                Console.WriteLine("*** New Character instance for key = {0}", key);
            }

            return _chars[key];
        }
    }

    public class Character
    {
        public Character(char symbol)
        {
            Symbol = symbol;
            Width = 100; // default
            Height = 120; // default
            Bitmap = new byte[1000]; // bitmap loaded
        }

        // intrinsic data (never changed, usually large)
        private byte[] Bitmap { get; set; } // bitmap to draw the character
        private char Symbol { get; set; }

        // extrinsic data (usually updated before every operation)
        public int Width { get; set; }
        public int Height { get; set; }
        
        public void Display()
        {
            Console.WriteLine("Display Symbol: {0}, Width: {1}, Height: {2}", Symbol, Width, Height);
        }
    }

    public static class Sample
    {
        public static void Start()
        {
            var factory = new CharacterFactory();

            foreach (char ch in "AAABBBAAAACCCBBBCCC")
            {
                Character character = factory.GetCharacter(ch); // it's object now! with many properties and methods!
                
                // adjust extrinsic properties
                character.Width += 1;
                character.Height += 1;

                character.Display();
            }

            // Note, for all the characters of "AAABBBAAAACCCBBBCCC", Character class was instantiated only 3 times.
        }
    }
}
