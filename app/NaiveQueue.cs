//This queue is not thread-safe. That’s because we’re using List<T>, which is not a thread-safe collection
//The List<T> collection will provide terrible performance for this usage. 
//It’s using a vector under the hood, which is essentially a dynamic size array. 
//An array is great for direct access operations, but not so great for adding and removing items.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NaiveQueue
{
    private List<string> _jobs = new List<string>();
 
    public NaiveQueue()
    {
        Task.Run(() => { OnStart(); });
    }
 
    public void Enqueue(string job)
    {
        _jobs.Add(job);
    }
 
    private void OnStart()
    {
        while (true)
        {
            if (_jobs.Count > 0)
            {
                var job = _jobs.First();
                _jobs.RemoveAt(0);
                Console.WriteLine(job);
            }
        }
    }
}