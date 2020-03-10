using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue
{
    // multiple handlers means multi thread de-queue jobs from the job queue
    public class TPLDataflowMultipleHandlers
    {
        private ActionBlock<string> _jobs;

        public TPLDataflowMultipleHandlers()
        {
            ExecutionDataflowBlockOptions executionDataflowBlockOptions = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 2,
            };

            _jobs = new ActionBlock<string>((job) =>
            {
                //execute after _jobs.Post(job)
                Thread.Sleep(10);
                // following is just for example's sake
                Console.WriteLine($"job: {job} , thread: { Thread.CurrentThread.ManagedThreadId }" );
            }, executionDataflowBlockOptions);
        }

        public void Enqueue(string job)
        {
            _jobs.Post(job);
        }
    }
}
