using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue
{
    /// <summary>
    /// 
    /// Note that BoundedCapacity has to be 1. Otherwise, the jobs will immediately move from the BlockingCollection to the ActionBlock, disabling any prioritization.
    /// You might be asking why even to use TPL Dataflow instead of BlockingCollection solutions(shown in Part 1). Well, you can combine the prioritization with other Dataflow features like Controlling Parallelism level and publisher/subscriber patterns.
    /// 
    /// </summary>
    public class TPLDataflowPriority
    {
        private ActionBlock<string> _actionBlock;
        private BlockingCollection<string> _jobs;

        public TPLDataflowPriority()
        {
            _actionBlock = new ActionBlock<string>(
                (job) => Console.WriteLine(job),
                // BoundedCapacity must be 1
                new ExecutionDataflowBlockOptions() { BoundedCapacity = 1 });

            _jobs = new BlockingCollection<string>(GetPriorityQueue());

            Task.Run(async () =>
            {
                foreach (var job in _jobs.GetConsumingEnumerable())
                {
                    await _actionBlock.SendAsync(job);
                }
            });
        }

        private IProducerConsumerCollection<string> GetPriorityQueue()
        {
            // your priority queue here

            return null;
        }

        public void Enqueue(string job)
        {
            _jobs.Add(job);
        }
    }
}
