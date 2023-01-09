using Serialization.Abstractions;
using System.Text.Json;

namespace Tracer.Serialization.Json;

public class Json : ITraceResultSerializer
{
    public Json()
    {
        Format = "Json";
    }

    public string Format { get; }
    public void Serialize(TraceResult traceResult, Stream to)
    {
        string result = "";
        var conf = new JsonSerializerOptions();
        conf.WriteIndented = true;
        conf.IgnoreReadOnlyProperties = false;
        result += '[';
        foreach (var i in traceResult.Children)
        {
            try
            {
                result += '\n' + JsonSerializer.Serialize<TraceResult>(i, conf);
                if (i != traceResult.Children[traceResult.Children.Count - 1])
                    result += ',';
            }
            catch
            {
                
            }
        }

        result += "\n]";
        using (StreamWriter writer = new(to))
        {
            writer.WriteLine(result);
        }
    }
}