namespace Tracer;

public class InternalTraceResult
{
    public bool IsReady;
    public int Pid;
    public long Time;
    public string Method;
    public string ClassName;
    public List<InternalTraceResult> Children;

    public InternalTraceResult(string method, string className, bool isReady, int pid, long time, List<InternalTraceResult> children)
    {
        Method = method;
        IsReady = isReady;
        Pid = pid;
        Time = time;
        Children = children;
        ClassName = className;
    }

    public InternalTraceResult()
    {
        Method = "";
        IsReady = false;
        Pid = 0;
        Time = 0;
        Children = new List<InternalTraceResult>();
        ClassName = "";
    }
}