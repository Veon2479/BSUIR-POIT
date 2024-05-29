using System;

namespace GoF.SingletonWrong
{
    // Look through the code. If you write Singletons that way, be careful!

    public class Config
    {
        private static int Attempts = 0;
        
        // WARNING: CLR initializes static fields only once, so if Config's constructor will throw an exception, there will be no second chance...
        public static readonly Config Instance = new Config();

        private Config() 
        {
            Console.WriteLine("***** Calling constructor. Attempt {0}", Attempts + 1);
            if (Attempts == 0)
            {
                ++Attempts;
                throw new Exception("...some service is not available now, for example"); // let it fails first time
            }
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
            try
            {
                Console.WriteLine(Config.Instance.GetValue("primary key"));
            }
            catch
            {
                Console.WriteLine(Config.Instance.GetValue("failover key"));
            }
        }
    }
}
