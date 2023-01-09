using Tracer;

namespace Serialization.Abstractions;

public interface ITraceResultSerializer
{
    string Format { get; }
    void Serialize(TraceResult traceResult, Stream to);
}