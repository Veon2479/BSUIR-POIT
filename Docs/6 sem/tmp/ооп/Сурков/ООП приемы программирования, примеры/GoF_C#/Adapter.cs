using System;
using System.Collections.Generic;

namespace GoF.Adapter
{
    interface IConfig
    {
        string GetValue(string key);
    }

    class RegistryConfig : IConfig
    {
        public string GetValue(string key)
        {
            // Use  System.Win32.Registry to retrieve key's value

            throw new NotImplementedException();
        }
    }

    class DictionaryConfig : IConfig // Adapt dictionary!
    {
        public Dictionary<string, string> _dict;

        public DictionaryConfig(Dictionary<string, string> dict)
        {
            _dict = dict;
        }

        public string GetValue(string key)
        {
            return _dict[key];
        }
    }

	public static class Sample
    {
        public static void Start()
        {
        }
    }

}
