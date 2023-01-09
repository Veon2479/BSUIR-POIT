namespace Faker.Core;

public class RecursionGuardian
{
    private readonly Dictionary<int, List<Type>> _stacks = new();
    private readonly object _stackLocker = new();
    private readonly int _maxRecursiveCount;
    private readonly long _fakerId;

    public RecursionGuardian(Faker faker, int maxRecursiveCount)
    {
        _fakerId = faker.FakerId;
        if (1 <= maxRecursiveCount)
            _maxRecursiveCount = maxRecursiveCount;
        else
            _maxRecursiveCount = 5;

    }
    
    public bool AddInstance(Type type, Faker faker)
    {
        bool res = false;
        if (faker.FakerId != _fakerId)
            throw new FakerException("Incorrect faker instance used!");
        if (CanBeAdded(type))
        {
            var curThread = Thread.CurrentThread.ManagedThreadId;
            lock (_stackLocker)
            {
                if (!_stacks.ContainsKey(curThread))
                    _stacks.Add(curThread, new List<Type>());
                _stacks[curThread].Add(type);
            }
            res = true;
        }
        return res;
    }

    private bool CanBeAdded(Type type)
    {
        bool res = true;
        var curThread = Thread.CurrentThread.ManagedThreadId;
        lock (_stackLocker)
        {
            if (res && _stacks.ContainsKey(curThread))
            {
                var list = _stacks[curThread];
                int count = 0, i = 0;
                while (i < list.Count && count <= _maxRecursiveCount)
                {
                    if (list[i] == type)
                        count++;
                    i++;
                }

                if (count > _maxRecursiveCount)
                    res = false;
            }
        }
        return res;
    }
    
    public bool RemoveInstance(Type type, Faker faker)
    {
        var curThread = Thread.CurrentThread.ManagedThreadId;
        bool res = (faker.FakerId != _fakerId);
        if (res)
            throw new FakerException("Incorrect faker instance used!");
        lock (_stackLocker)
        {
            if (!res && _stacks.ContainsKey(curThread))
            {
                var list = _stacks[curThread];
                if (list.Count != 0 && list.Last() == type)
                {
                    list.RemoveAt(list.Count - 1);
                    if (list.Count == 0)
                    {
                        _stacks.Remove(curThread);
                    }

                    res = true;
                }
                else
                {
                    res = false;
                }
            }
        }
        return res;
    }

}