namespace Tracer;

public class TraceResult
{
    public bool IsReady { get; }
    public int Tid { get; }
    public long Time { get; }
    public string Method { get; }
    public string ClassName { get; }
    public IReadOnlyList<TraceResult> Children { get; }

    public TraceResult(string method, string className, bool isReady, int tid, long time, IReadOnlyList<TraceResult> children)
    {
        Method = method;
        Tid = tid;
        Time = time;
        Children = children;
        IsReady = isReady;
        ClassName = className;
    }
    
    public TraceResult()
    {
        Method = "";
        IsReady = false;
        Tid = 0;
        Time = 0;
        Children = new List<TraceResult>();
        ClassName = "";
    }
}