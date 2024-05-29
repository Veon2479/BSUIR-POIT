using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.Caching
{
    public class LogBuffer<TItem> 
        where TItem: class
    {
        public delegate void LogItemsSavingCallback(List<TItem> logItems);

        public LogBuffer(int cacheLimit, int flushPeriodInSeconds, 
            LogItemsSavingCallback logItemsSaving)
        {
            this.cacheLimit = cacheLimit;
            isEnabled = true;
            flushTimerPeriod = flushPeriodInSeconds * 1000;
            LogItemsSaving = logItemsSaving;
            cacheMutex = new object();
            flushingCacheMutex = new object();
            cache = new List<TItem>();
            flushingCache = new List<TItem>();
            flushTimer = new Timer(FlushTimerTick, null, flushTimerPeriod, -1);
            flushingCacheThread = new Thread(FlushWorker);
            flushingCacheThread.IsBackground = true;
            flushingCacheThread.Start(null);
        }

        public void Close()
        {
            lock (cacheMutex)
            {
                lock (flushingCacheMutex)
                {
                    if (isEnabled)
                    {
                        flushTimer.Change(-1, -1);
                        FlushCache();
                        isEnabled = false;
                        Monitor.PulseAll(flushingCacheMutex);
                    }
                }
            }
            flushingCacheThread.Join();
        }

        public bool Add(TItem item)
        {
            lock (cacheMutex)
            {
                if (isEnabled)
                {
                    cache.Add(item);
                    if (cache.Count >= cacheLimit)
                        FlushCacheAsync();
                }
                return isEnabled;
            }
        }

        public void FlushCacheAsync()
        {
            lock (cacheMutex)
            {
                flushTimer.Change(-1, -1);
                try
                {
                    if (cache.Count > 0)
                    {
                        lock (flushingCacheMutex)
                        {
                            while (flushingCache.Count > 0)
                                Monitor.Wait(flushingCacheMutex);
                            List<TItem> temp = cache;
                            cache = flushingCache;
                            flushingCache = temp;
                            Monitor.PulseAll(flushingCacheMutex);
                        }
                    }
                }
                finally
                {
                    flushTimer.Change(flushTimerPeriod, -1);
                }
            }
        }

        public void FlushCache()
        {
            lock (cacheMutex)
            {
                FlushCacheAsync();
                lock (flushingCacheMutex)
                {
                    while (flushingCache.Count > 0)
                        Monitor.Wait(flushingCacheMutex);
                }
            }
        }

        public event LogItemsSavingCallback LogItemsSaving;

        private void FlushTimerTick(object state)
        {
            FlushCacheAsync();
        }

        private void FlushWorker(object state)
        {
            try
            {
                lock (flushingCacheMutex)
                {
                    while (isEnabled)
                    {
                        while (isEnabled && flushingCache.Count == 0)
                            Monitor.Wait(flushingCacheMutex);
                        if (flushingCache.Count > 0)
                        {
                            if (LogItemsSaving != null)
                                LogItemsSaving(flushingCache);
                            flushingCache.Clear();
                            Monitor.PulseAll(flushingCacheMutex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (Log.IsErrorEnabled)
                        Log.Error("Log cache flushing thread FAILED; ", ex);
                }
                catch
                {
                }
            }
        }

        private bool isEnabled;
        private int cacheLimit;
        private int flushTimerPeriod;
        private List<TItem> cache;
        private List<TItem> flushingCache;
        private Timer flushTimer;
        private object cacheMutex;
        private object flushingCacheMutex;
        private Thread flushingCacheThread;
    }
}
