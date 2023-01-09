namespace Scanner;

public class TaskQueue
{
    private readonly List<Thread> _threads;
    private readonly Queue<Tuple<Action<string>, string>> _tasks;

    public TaskQueue(int threadCount)
    {
        _tasks = new Queue<Tuple<Action<string>, string>>();
        _threads = new List<Thread>();
        for (int i = 0; i < threadCount; i++)
        {
            var t = new Thread(DoThreadWork);
            _threads.Add(t);
            t.IsBackground = true;
            t.Start();
        }
    }

    public int ThreadCount
    {
        get { return _threads.Count; }
    }

    public void EnqueueTask(Action<string> task, string path)
    {
        lock (_countLocker)
        {
            _taskCounter++;
        }
        lock (_tasks)
        {
            _tasks.Enqueue(new Tuple<Action<string>, string>(task, path));
            Monitor.Pulse(_tasks);
        }
    }

    private Tuple<Action<string>, string> DequeueTask()
    {
        lock (_tasks)
        {
            while (_tasks.Count == 0)
                Monitor.Wait(_tasks);
            return _tasks.Dequeue();
        }
    }

    private void DoThreadWork()
    {
        bool flag = true;
        while (flag)
        {
            
            {
                var task = DequeueTask();
                try
                {
                    
                    task.Item1(task.Item2);
                    lock (_countLocker)
                    {
                        _taskCounter--;
                    }
                    
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }


    private object _countLocker = new();
    private long _taskCounter = 0;
    public long GetRemainingTaskCount()
    {
        lock (_countLocker)
        {
            return _taskCounter;
        }
    }


}