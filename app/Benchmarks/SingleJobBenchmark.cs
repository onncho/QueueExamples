using app.TPL_DataflowQueue.PoisonQueue;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace app
{
    public class SingleJobBenchmark
    {
        private AutoResetEvent _autoResetEvent;

        public SingleJobBenchmark()
        {
            _autoResetEvent = new AutoResetEvent(false);
        }

        [Benchmark]
        public void BlockingCollectionQueue()
        {
            DoOneJob(new BlockingCollectionQueue());
        }

        [Benchmark]
        public void NoDedicatedThreadQueue()
        {
            DoOneJob(new NoDedicatedThreadQueue());
        }

        [Benchmark]
        public void RxQueue()
        {
            DoOneJob(new RxQueueWithScheduler());
        }

        [Benchmark]
        public void ChannelsQueue()
        {
            DoOneJob(new ChannelsQueue());
        }

        [Benchmark]
        public void TPLDataflowQueue()
        {
            DoOneJob(new TPLDataflowSimpleQueue());
        }

        private void DoOneJob(IJobQueue<Action> jobQueue)
        {
            jobQueue.Enqueue(() => _autoResetEvent.Set());
            _autoResetEvent.WaitOne();
            jobQueue.Stop();
        }
    }
}
