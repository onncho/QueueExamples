
//.NET Framework 4 introduced ConcurrentQueue, which is exactly the data structure we need. 
//It’s thread-safe and also optimized for Queue’s Enqueue and Dequeue operations.
using System;
using System.Collections.Concurrent;
using System.Threading;

public class BitBetterQueue
{
    private ConcurrentQueue<object> _jobs = new ConcurrentQueue<object>();
 
    public BitBetterQueue()
    {
        var thread = new Thread(new ThreadStart(OnStart));
        thread.IsBackground = true;
        thread.Start();
    }
 
    public void Enqueue(object job)
    {
        _jobs.Enqueue(job);
    }
 
    private void OnStart()
    {
        while (true)
        {
            if (_jobs.TryDequeue(out object result))
            {
                Console.WriteLine(result);
            }
        }
    }
}