using System;

namespace GoF.Singleton
{
    // MSDN about Singleton: http://msdn.microsoft.com/en-us/library/ee817670.aspx
    public class Config
    {
        private static int Attempts = 0;
        private static readonly object _syncRoot = new object();
        private static volatile Config _instance = null; // MSDN promises thread safety when using "volatile" here

        private Config() 
        {
            Console.WriteLine("***** Calling constructor. Attempt {0}", Attempts + 1);
            if (Attempts == 0)
            {
                ++Attempts;
                throw new Exception("...some service is not available now, for example"); // let it fails first time
            }
        }

        public static Config Instance()
        {
            if (_instance == null)
            {
                lock (_syncRoot) // read notes about locks below
                {
                    if (_instance == null)
                    {
                        _instance = new Config();
                    }
                }
            }
            return _instance;
        }
        
        public string GetValue(string key)
        {
            return string.Format("Value for [{0}]", key);
        }
    }

    public static class Sample
    {
        public static void Start()
        {
            // Now Config instance is available from any point of your program

            try
            {
                Console.WriteLine(Config.Instance().GetValue("primary key")); 
            }
            catch
            {
                Console.WriteLine(Config.Instance().GetValue("failover key"));
            }
        }
    }

    // MSDN about locks:

    // In general, avoid locking on a public type, or instances beyond your code's control. 
    // The common constructs lock (this), lock (typeof (MyType)), and lock ("myLock") violate this guideline:

    // 1. lock (this) is a problem if the instance can be accessed publicly.
    // 2. lock (typeof (MyType)) is a problem if MyType is publicly accessible.
    // 3. lock("myLock") is a problem because any other code in the process using the same string, will share the same lock.
    
    // Best practice is to define a private object to lock on, or a private static object variable to protect data common to all instances.
}
