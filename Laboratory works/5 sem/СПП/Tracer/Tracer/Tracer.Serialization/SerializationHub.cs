using System.Reflection;
using Serialization.Abstractions;

namespace Tracer.Serialization;

public class SerializationHub
{

    private Dictionary<string, ITraceResultSerializer> _plugins = new();
    private string _pluginPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Plugins"); 
    private SerializationHub()
    {
        DirectoryInfo pluginDir = new DirectoryInfo(_pluginPath);
        if (!pluginDir.Exists)
            pluginDir.Create();
        var pluginFiles = Directory.GetFiles(_pluginPath, "*.dll");
        foreach (var file in pluginFiles)
        {
            Assembly asm = Assembly.LoadFrom(file);
            
            var types = asm.GetTypes().
                Where(t => t.GetInterfaces().Any(i => i.FullName == typeof(ITraceResultSerializer).FullName));
            foreach (var type in types)
            {
                var plugin = asm.CreateInstance(type.FullName!) as ITraceResultSerializer;
                _plugins.Add(plugin!.Format, plugin);
            }
        }
        if (_plugins.Count == 0)
            Console.WriteLine("ERR: no plugins are detected!");
    }

    private static SerializationHub? _instance = null;
    private static readonly object Locker = new();
    public static SerializationHub GetInstance()
    {
        lock (Locker)
        {
            if (_instance == null)
                lock (Locker)
                {
                    _instance = new SerializationHub();
                }
        }

        return _instance;
    }
    
    public void Serialize(TraceResult head, string type)
    {
        ITraceResultSerializer? serializer = null;
        if (_plugins.ContainsKey(type))
            serializer = _plugins[type];
        else if (_plugins.Count != 0)
            serializer = _plugins.First().Value;
        if (serializer != null)
        {
            var file = serializer.Format + ".txt";
            File.WriteAllText(file, String.Empty);
            using (FileStream fs = File.OpenWrite(file))
            {
                serializer.Serialize(head, fs);
            }
            
        }

    }

    public List<string> GetMethodNames()
    {
        List<string> result = new();
        foreach (var i in _plugins)
        {
            result.Add(i.Key);
        }

        return result;
    }
}