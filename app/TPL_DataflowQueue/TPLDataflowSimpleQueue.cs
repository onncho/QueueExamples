using app.TPL_DataflowQueue.PoisonQueue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace app
{    
    public class TPLDataflowSimpleQueue : IJobQueue<Action>
    {
        /// <summary>
        /// An ActionBlock is one kind of Block in TPL Dataflow. 
        /// It acts as an ITargetBlock, so you can send messages to it. 
        /// But not as an ISourceBlock, so it can’t propagate messages to other blocks. 
        /// It has the ability to invoke a delegate for each data element received.
        /// </summary>
        private ActionBlock<Action> _jobs;

        public TPLDataflowSimpleQueue()
        {
            _jobs = new ActionBlock<Action>((job) =>
            {
                job.Invoke();
                //Console.WriteLine(job);
            });
        }

        public void Enqueue(Action job)
        {
            _jobs.Post(job);
        }

        public void Stop()
        {
            _jobs.Complete();
        }
    }
}
