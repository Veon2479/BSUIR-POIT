using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WeakReferences
{
    class Program
    {
        static void Main(string[] args)
        {
            Object o = File.ReadAllLines(@"C:\MyTestLog.txt");
            
            // Работа с o

            WeakReference wr = new WeakReference(o);
            o = null;

            // Работа с другими данными

            // По возможности наболее полная сборка мусора
            GC.Collect(
                generation: GC.MaxGeneration, 
                mode: GCCollectionMode.Forced, 
                blocking: true, 
                compacting: true);
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Object o2 = wr.Target;
            if (o2 == null)
            {
                // Памяти не хватило, объект был утилизирован
                o2 = File.ReadAllLines(@"C:\MyTestLog.txt");
                wr.Target = o2;
            }

            // Работа с o2

            o2 = null;

        }
    }
}
