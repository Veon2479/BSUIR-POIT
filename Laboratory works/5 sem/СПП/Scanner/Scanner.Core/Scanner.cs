using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Scanner.Types;

namespace Scanner;

public class Scanner
{
    public Scanner()
    {
        _nodeResolver = new Thread(ResolveNodeQueue);
        _source = new CancellationTokenSource();
    }
    
    private MutableScannerResult _result = new MutableScannerResult();
    private readonly object _resultLocker = new();
    
    private bool _isRunning = false;
    private readonly object _isRunningLocker = new();

    public bool IsChanged;
    
    
    public bool IsRunning()
    {
        bool res = false;
        lock (QueueEmptyLocker)
        {
            res = !_isQueueEmpty;
        }
        lock (_isRunningLocker)
        {
            return res | _isRunning;
        }
    }
    public ScannerResult GetResult()
    {
        lock (_resultLocker)
        {
            IsChanged = false;
            return _result.GetScannerResult();
        }
    }
    
    private TaskQueue _taskQueue = new(5);


    private CancellationTokenSource _source;
    private CancellationToken _token;
    private void ScanningTask(string path)
    {
        if (_isRunning && !_token.IsCancellationRequested)
        {
            var file = new FileInfo(path);
        
            FileType type = FileType.File;
            if (Directory.Exists(path))
                type = FileType.Directory;
            else if (file.LinkTarget != null)
                type = FileType.Link;

            long size = 0;
            if (type != FileType.Directory)
                size = file.Length;
            var node = new MutableNode(type, size, 0, file.Name, path);
            _nodeQueue.Enqueue(node);

            if (_isRunning && type == FileType.Directory && !_token.IsCancellationRequested)
            {
                var dir = new DirectoryInfo(path);
                foreach (var f in dir.GetFiles())
                {
                    _taskQueue.EnqueueTask(ScanningTask, f.FullName);
                }
                foreach (var d in dir.GetDirectories())
                {
                    _taskQueue.EnqueueTask(ScanningTask, d.FullName);
                }
            }
        }
        
    }

    private string _path = "";
    public bool ScanDirectory(string path)
    {
        lock (_resultLocker)
        {
            _result = new MutableScannerResult();
            _path = Path.GetFullPath(path);

        }
        if (!Directory.Exists(path))
        {
            return false;
        }
        lock (_isRunningLocker)
        {
            if (_isRunning)
            {
                return false;
            }
            _isRunning = true;
            _isNodeResolveRunning = true;
        }

        IsChanged = false;
        _source = new CancellationTokenSource();
        _token = _source.Token;
        
        _taskQueue.EnqueueTask(ScanningTask, _path);
        
        _nodeResolver = new Thread(ResolveNodeQueue);
        _nodeResolver.Start();
        return true;
    }
    public void AbortScanning()
    {
        lock (_isRunningLocker)
        {
            _isRunning = false;
            _isNodeResolveRunning = false;
        }
        _source.Cancel();
        _nodeResolver.Join();
    }

    // private readonly ConcurrentQueue<Task> ThreadQueue = new();
    // private readonly int maxThreadCount = 5;
    // private bool _isThreadResolveRunning = false;
    // private readonly Thread _threadResolver; 
    // private void ResolveThreadQueue()
    // {
    //     while (_isThreadResolveRunning) // & _isRunning? or another flag specially for aborting
    //     {
    //         //TODO: write this shitty code:
    //         //while (running tasks count < max)
    //         //  try to start new task, running task counter++
    //         //check, if first task finished
    //         //  true: remove it from queue, running task counter--, wait this task?
    //         
    //         //if queue is empty, wait a bit? if queue is still empty - stop resolving
    //         
    //     }
    //     //if unfinished started tasks exists - wait them
    //         //clear queue
    // }

    private Thread _nodeResolver;

    private readonly ConcurrentQueue<MutableNode?> _nodeQueue = new();
    private bool _isNodeResolveRunning = false;

    private bool _isQueueEmpty = true;
    private static readonly object QueueEmptyLocker = new();
    private void ResolveNodeQueue()
    {
        while (_isNodeResolveRunning)
        {
            if (_nodeQueue.Count != 0)
            {
                lock (QueueEmptyLocker)
                {
                    _isQueueEmpty = false;
                }
                MutableNode? node;
                if (_nodeQueue.TryDequeue(out node) && !_token.IsCancellationRequested)
                    if (node != null && !AddNodeToResultTree(node))
                        _nodeQueue.Enqueue(node);
            }
            else
            {
                lock (QueueEmptyLocker)
                {
                    _isQueueEmpty = true;
                }
            }

            if (_taskQueue.GetRemainingTaskCount() == 0)
            {
                lock (QueueEmptyLocker)
                {
                    if (_isQueueEmpty)
                    {
                        lock (_isRunningLocker)
                        {
                            _isRunning = false;
                            _isNodeResolveRunning = false;
                            _source.Dispose();
                        }
                    }
                }
            }
        }
    }
    private bool AddNodeToResultTree(MutableNode node)
    {
        if (_path == node.Path)
        {
            lock (_resultLocker)
            {
                _result.Children.Add(node);
                IsChanged = true;
                return true;
            }
        }
        else
        {
            lock (_resultLocker)
            {
                bool fl = true;
                bool isNewNodeAdded = false;
                List<MutableNode> list = _result.Children;
                while (fl)
                {

                    bool isPathFound = false;
                    int i = 0;
                    while (i < list.Count && !isPathFound)
                    {
                        if (list[i].FileType == FileType.Directory
                            && IsFullSubstring(list[i].Path, node.Path))
                        {
                            isPathFound = true;
                        }
                        else
                            i++;
                    }

                    if (i < 0 || i >= list.Count)
                        Console.WriteLine("AAAA");
                    var oldPath = list[i].Path;                    
                    list = list[i].Children;

                    var pathCur = (oldPath + "/" + node.Name);
                    var pathTarget = node.Path;
                    var delta = pathCur.Length - pathTarget.Length;
                    if ( Math.Abs(delta) <= 1)
                    {
                        list.Add(node);
                        fl = false;
                        i = Int32.MaxValue - 1;
                        isNewNodeAdded = true;
                    }
                   
                }
                if (isNewNodeAdded)
                    ComputeSizes();
                IsChanged = true;
                return !fl;
            }
        }
    }
    private void ComputeSizes()
    {
        long size = 0;
        List<long> sizes = new();
        foreach (var node in _result.Children)
        {
            var d = ComputingSizeWorker(node);
                size += d;
            sizes.Add(d);
        }
        _result.Size = size;
        int i = 0;
        foreach (var node in _result.Children)
        {
            node.RelativeSize = (float)node.AbsoluteSize / (float)sizes[i] * 100;
            i++;
        }
    }
    private long ComputingSizeWorker(MutableNode node)
    {
        long result = 0;
        if (node.Children.Count == 0)
        {
            result = node.AbsoluteSize;
        }
        else
        {
            List<long> sizes = new List<long>();
            int i = 0;
            foreach (var n in node.Children)
            {
                sizes.Add(ComputingSizeWorker(n));
                result += sizes[i];
                i++;
            }

            foreach (var n in node.Children)
            {
                if (result != 0)
                    n.RelativeSize = (float)n.AbsoluteSize / (float)result * 100;
            }
            node.AbsoluteSize = result;
            i = 0;
            foreach (var size in sizes)
            {
                if (result != 0)
                    node.Children[i].RelativeSize = (float)node.Children[i].AbsoluteSize / (float)result * 100;
            }
        }
        
        return result;
    }
    private bool IsFullSubstring(string sub, string src)
    {
        bool res = true;
        if (sub.Length <= src.Length)
        {
            int i = 0;
            while (res && i < sub.Length)
            {
                if (sub[i] != src[i])
                    res = false;
                i++;
            }

            if (i != 0 && res && src[i-1]!='/'&& src[i] != '/')
                res = false;
        }
        else
            res = false;
        return res;
    }
    
}