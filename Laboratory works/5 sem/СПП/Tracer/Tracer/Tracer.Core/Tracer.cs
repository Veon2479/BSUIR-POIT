using System.Diagnostics;

namespace Tracer;

public class Tracer : ITracer
{
    private static Tracer? _instance;
    private static readonly object TracerInstanceLocker = new();

    private readonly Stopwatch _timer;

    public Tracer()
    {
        _timer = new Stopwatch();
        _timer.Start();
    }

    public static Tracer GetInstance()
    {
        if (_instance == null)
        {
            lock (TracerInstanceLocker)
            {
                _instance ??= new Tracer();
            }
        }

        return _instance;
    }

    
    

    private readonly InternalTraceResult _treeHead = new();
    private static readonly object TreeLocker = new();

    private readonly List<InternalTraceResult> _nodeList = new();
    private static readonly object NodeListLocker = new();
    
    public void StartTrace()
    {
        var trace = new StackTrace();
        var frame = trace.GetFrame(1);
        var node = new InternalTraceResult(frame!.GetMethod()!.Name, frame.GetMethod()!.DeclaringType!.Name, 
            false, GetThreadId(), 0, new List<InternalTraceResult>());
        lock (NodeListLocker)
        {
            InternalTraceResult parent = FindParent(trace);
            lock (TreeLocker)
            {
                if (parent != _treeHead)
                    StopChildrenTracing(parent);
                parent.Children.Add(node);
                node.Time = _timer.ElapsedMilliseconds;
            }
            _nodeList.Add(node);
        }
    }

    private void StopChildrenTracing(InternalTraceResult head)
    {
        foreach (var t in head.Children)
        {
            if (!t.IsReady)
            {
                var time = _timer.ElapsedMilliseconds;
                t.Time = Math.Abs(t.Time - time);
                t.IsReady = true;
                StopChildrenTracing(t);
            }
        }
    }

    
    //if it can't find node - _treeHead is returned
    private InternalTraceResult FindParent(StackTrace trace)
    {
        var node = _treeHead;
        trace.GetFrame(1);
        var parentFrame = trace.GetFrame(2);
        lock (NodeListLocker)
        {
            int i = _nodeList.Count - 1;
            while (node == _treeHead && i >= 0)
            //for (int i = _nodeList.Count - 1; i >= 0; i--)
            {
                //only unfinished (IsReady == false) method can get new child
                if (_nodeList[i].IsReady == false && _nodeList[i].Pid == GetThreadId() &&
                    parentFrame!.GetMethod()!.Name == _nodeList[i].Method && 
                    parentFrame.GetMethod()!.DeclaringType!.Name == _nodeList[i].ClassName)
                {
                    node = _nodeList[i];
                }

                i--;
            }
        }

        return node;
    }

    public void StopTrace()
    {
        var time = _timer.ElapsedMilliseconds;
        var trace = new StackTrace();
        var node = FindNode(trace);
        node.IsReady = true;
        node.Time = Math.Abs(node.Time - time);
        
    }

    
    //if it can't find node - new empty node returned
    private InternalTraceResult FindNode(StackTrace trace)
    {
        var frame = trace.GetFrame(1);
        InternalTraceResult node = _treeHead;
        lock (NodeListLocker)
        {
            int i = _nodeList.Count - 1;
            while (node == _treeHead && i >= 0)
            //for (int i = _nodeList.Count - 1; i >= 0; i--)
            {
                if (_nodeList[i].IsReady == false && _nodeList[i].Pid == GetThreadId() &&
                    frame!.GetMethod()!.Name == _nodeList[i].Method && 
                    frame.GetMethod()!.DeclaringType!.Name == _nodeList[i].ClassName)
                {
                    node = _nodeList[i];
                }

                i--;
            }
        }

        if (node == _treeHead)
            node = new();
        return node;
    }

    public TraceResult GetTraceResult()
    {
        TraceResult result;
        lock (TreeLocker)
        {
            result = MakeResultTree(_treeHead);
        }

        return result;
    }

    private TraceResult MakeResultTree(InternalTraceResult treeHead)
    {
        var children = new List<TraceResult>();
        foreach (var i in treeHead.Children)
        {
            children.Add(MakeResultTree(i));
        }
        var result = new TraceResult(treeHead.Method, treeHead.ClassName, treeHead.IsReady, treeHead.Pid, treeHead.Time, children);
        
        return result;
    }

    private int GetThreadId()
    {
        return Thread.CurrentThread.ManagedThreadId;
    }

    public void ResetTracer()
    {
        lock (NodeListLocker)
        {
            lock (TreeLocker)
            {
                _treeHead.Children.Clear();
                _nodeList.Clear();
            }
        }
    }
}