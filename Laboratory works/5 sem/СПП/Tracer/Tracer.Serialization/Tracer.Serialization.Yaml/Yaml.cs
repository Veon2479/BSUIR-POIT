using Serialization.Abstractions;
using YamlDotNet.Serialization;

namespace Tracer.Serialization.Yaml;

public class Yaml : ITraceResultSerializer
{
    public Yaml()
    {
        Format = "Yaml";
    }

    public string Format { get; }
    public void Serialize(TraceResult traceResult, Stream to)
    {
        var serializer = new SerializerBuilder().Build();
        string result = "Trees:";
        foreach (var i in traceResult.Children)
        {
            try
            {
                result += "\n" + serializer.Serialize(i);

            }
            catch
            {
            }
        }

        int j = 0;
        while (j < result.Length)
        {
            if (j>1)
                if (result[j - 1] == '\n')
                {
                    if (result[j] == '-')
                    {
                        result = result.Insert(j, " -");
                    }
                    if (result[j] == 'I')
                    {
                        result = result.Insert(j, "-");
                    }
                    else
                    {
                        result = result.Insert(j, " ");
                    }
                }
                
            

            j++;
        }
        using (StreamWriter writer = new(to))
        {
            writer.WriteLine(result);
        }
    }
}