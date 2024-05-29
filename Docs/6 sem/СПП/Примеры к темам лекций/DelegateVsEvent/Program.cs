using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Delegates
{
    public delegate void ListUpdateEvent(ObservableList sender);

    public class ObservableList
    {
        private ListUpdateEvent updated;
        public List<string> List;

        public event ListUpdateEvent Updated
        {
            add { updated += value; }
            remove { updated -= value; }
        }

        public ObservableList(List<string> list)
        {
            List = list;
        }

        public void Add(string item)
        {
            List.Add(item);
            if (updated != null)
                updated(this);
        }

        public bool Remove(string item)
        {
            var result = List.Remove(item);
            if (updated != null)
                updated(this);
            return result;
        }
    }

    class Program
    {
        static void HandleEvent(ObservableList sender)
        {
            Console.WriteLine("Количество элементов: {0}",
                sender.List.Count);
        }

        static void Main(string[] args)
        {
            var list = new ObservableList(
                new List<string>());

            list.Updated += HandleEvent;
            // Запрещено: 
            // list.Updated = HandleEvent;
            // list.Updated(list);

            list.Add("Ключ на старт");
            list.Add("Протяжка-1");

            list.Updated -= HandleEvent;
            list.Add("Продувка");
        }
    }
}
