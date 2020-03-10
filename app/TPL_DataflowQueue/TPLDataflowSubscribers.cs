using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue
{

    /// <summary>
    /// Priority Queue with TPL Dataflow
    /// Sometimes, you will want to have prioritization for your jobs.For example, 
    /// in our call center, medical calls should always be treated first, then criminal calls and fire calls last.
    /// TPL DataFlow is not very well suited for priority queues, but it can be done.
    /// </summary>
    public class TPLDataflowSubscribers
    {
        private BroadcastBlock<IJob> _jobs;

        public TPLDataflowSubscribers()
        {
            _jobs = new BroadcastBlock<IJob>(job => job);
        }

        public void RegisterHandler<T>(Action<T> handleAction) where T : IJob
        {
            // We have to have a wrapper to work with IJob instead of T
            Action<IJob> actionWrapper = (job) => handleAction((T)job);

            // create the action block that executes the handler wrapper
            var actionBlock = new ActionBlock<IJob>((job) => actionWrapper(job));

            // Link with Predicate - only if a job is of type T
            //_jobs.LinkTo(actionBlock, predicate: (job) => job is T);

            _jobs.LinkTo(actionBlock, new DataflowLinkOptions() { PropagateCompletion = true });
        }

        public async Task Enqueue(IJob job)
        {
            await _jobs.SendAsync(job);
        }
    }
}
