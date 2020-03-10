

//We can make that last implementation even nicer by utilizing another class introduced along with ConcurrentQueue – BlockingCollection. 
//This class is optimized for the Producer/Consumer pattern:
// BlockingCollection does is provide Blocking capabilities.

// the following are the same
//BlockingCollection<object> _jobs = new BlockingCollection<object>();
//BlockingCollection<object> _jobs = new BlockingCollection<object>(new ConcurrentQueue<object>());

using System;
using System.Collections.Concurrent;
using System.Threading;

public class BlockingCollectionQueue
{
    private BlockingCollection<object> _jobs = new BlockingCollection<object>();

    public BlockingCollectionQueue()
    {
        var thread = new Thread(new ThreadStart(OnStart));
        thread.IsBackground = true;
        thread.Start();
    }

    public void Enqueue(object job)
    {
        _jobs.Add(job);
    }


//GetConsumingEnumerable method. When called, it will either Take the next item in the collection or Block until such an item exists. 
//In other words, it will stop the thread until a new item is added to the collection. 
//With this method, we don’t have to write that annoying infinite loop while(true){}.

    private void OnStart()
    {
        foreach (var job in _jobs.GetConsumingEnumerable(CancellationToken.None))
        {
            Console.WriteLine(job);
        }
    }
}