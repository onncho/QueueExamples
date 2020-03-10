//In some scenarios, the Queue will be empty most of the time so it might make more sense to use a temporary pooled thread.

using System;
using System.Collections.Generic;
using System.Threading;

public class NoDedicatedThreadQueue
{
    private Queue<string> _jobs = new Queue<string>();
    private bool _delegateQueuedOrRunning = false;
 
    public void Enqueue(string job)
    {
        lock (_jobs)
        {
            _jobs.Enqueue(job);
            if (!_delegateQueuedOrRunning)
            {
                _delegateQueuedOrRunning = true;
                ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
            }
        }
    }
 
    private void ProcessQueuedItems(object ignored)
    {
        while (true)
        {
            string item;
            lock (_jobs)
            {
                if (_jobs.Count == 0)
                {
                    _delegateQueuedOrRunning = false;
                    break;
                }
 
                item = _jobs.Dequeue();
            }
 
            try
            {
                //do job
                Console.WriteLine(item);
            }
            catch
            {
                ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                throw;
            }
        }
    }
}