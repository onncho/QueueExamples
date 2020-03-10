using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace app
{
    public class ManyJobsBenchmark
    {
        private AutoResetEvent _autoResetEvent;

        public ManyJobsBenchmark()
        {
            _autoResetEvent = new AutoResetEvent(false);
        }

        [Benchmark]
        public void BlockingCollectionQueue()
        {
            DoManyJobs(new BlockingCollectionQueue());
        }
        [Benchmark]
        public void NoDedicatedThreadQueue()
        {
            DoManyJobs(new NoDedicatedThreadQueue());
        }
        [Benchmark]
        public void RxQueue()
        {
            DoManyJobs(new RxQueueWithScheduler());
        }
        [Benchmark]
        public void ChannelsQueue()
        {
            DoManyJobs(new ChannelsQueue());
        }
        [Benchmark]
        public void TPLDataflowQueue()
        {
            DoManyJobs(new TPLDataflowSimpleQueue());
        }

        private void DoManyJobs(IJobQueue<Action> jobQueue)
        {
            int jobs = 100000;
            for (int i = 0; i < jobs - 1; i++)
            {
                jobQueue.Enqueue(() => { });
            }
            jobQueue.Enqueue(() => _autoResetEvent.Set());
            _autoResetEvent.WaitOne();
            jobQueue.Stop();
        }
    }
}
