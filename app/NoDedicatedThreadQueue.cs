//In some scenarios, the Queue will be empty most of the time so it might make more sense to use a temporary pooled thread.

using System;
using System.Collections.Generic;
using System.Threading;

namespace app
{
    public class NoDedicatedThreadQueue : IJobQueue<Action>
    {
        private Queue<Action> _jobs = new Queue<Action>();
        private bool _delegateQueuedOrRunning = false;

        public void Enqueue(Action job)
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
                Action job;
                lock (_jobs)
                {
                    if (_jobs.Count == 0)
                    {
                        _delegateQueuedOrRunning = false;
                        break;
                    }

                    job = _jobs.Dequeue();
                }

                try
                {
                    //do job
                    job.Invoke();
                    //Console.WriteLine(job.Method);
                }
                catch
                {
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, null);
                    throw;
                }
            }
        }
        public void Stop()
        {
        }

    }
}