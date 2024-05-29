using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGeneric
{
    public class MyList<T> : IEnumerable<T>
        where T: class, new() //, IDisposable
    {
        public MyList()
        {
        }

        public void Add(T item)
        {
            CheckCapacity();
            items[count] = item;
            count++;
        }

        public int Count 
        { 
            get { return count; } 
        }

        //private class MyEnumerator : IEnumerator<T>
        //{
        //    public MyEnumerator(T[] items, int count)
        //    {
        //        this.items = items;
        //        this.count = count;
        //        currIndex = -1;
        //    }

        //    T Current
        //    {
        //        get 
        //        {
        //            if (currIndex >= 0 && currIndex < count)
        //                return items[currIndex];
        //            else
        //                throw new Exception("Iterator");
        //        }
        //    }

        //    bool MoveNext()
        //    {
        //        bool result = currIndex < count;
        //        if (result)
        //            currIndex++;
        //        else
        //            throw new Exception("Iterator");
        //        return result;
        //    }

        //    void Reset()
        //    {
        //        currIndex = -1;
        //    }

        //    private T[] items;
        //    private int count;
        //    private int currIndex;
        //}
        
        public IEnumerator<T> GetEnumerator()
        {
            //return new MyEnumerator(items, count);
            int curr = -1;
            foreach (T a in items)
            {
                curr++;
                if (curr < count)
                    yield return a;
                else
                    break;
            }
        }

        private void CheckCapacity()
        {
            if (items == null || count == items.Length)
            {
                T[] newItems = new T[(count + 1) * 2];
                if (items != null)
                    Array.Copy(items, newItems, count);
                items = newItems;
            }
        }

        private T[] items;
        private int count;
    }
}
