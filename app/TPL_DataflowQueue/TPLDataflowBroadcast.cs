using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue
{
    public class TPLDataflowBroadcast
    {
        private BroadcastBlock<string> _jobs;

        public TPLDataflowBroadcast()
        {
            // The delegate 'job=>job' allows to transform the job, like Select in LINQ
            _jobs = new BroadcastBlock<string>(job => job);


            //var act = new ActionBlock<T>(job => {/*..*/ }, new ExecutionDataflowBlockOptions() {MaxDegreeOfParallelism = 3});
            ActionBlock<string> act1 = new ActionBlock<string>((job) =>
            {
                Console.WriteLine(job);
            });

            ActionBlock<string> act2 = new ActionBlock<string>((job) =>
            {
                LogToFile(job);
            });

            _jobs.LinkTo(act1);
            _jobs.LinkTo(act2);
        }

        private void LogToFile(string job)
        {
            //...
        }

        public void Enqueue(string job)
        {
            _jobs.Post(job);
        }
    }
}