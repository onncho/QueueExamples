using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue
{    
    public class TPLDataflowSimpleQueue
    {
        /// <summary>
        /// An ActionBlock is one kind of Block in TPL Dataflow. 
        /// It acts as an ITargetBlock, so you can send messages to it. 
        /// But not as an ISourceBlock, so it can’t propagate messages to other blocks. 
        /// It has the ability to invoke a delegate for each data element received.
        /// </summary>
        private ActionBlock<string> _jobs;

        public TPLDataflowSimpleQueue()
        {
            _jobs = new ActionBlock<string>((job) =>
            {
                Console.WriteLine(job);
            });
        }

        public void Enqueue(string job)
        {
            _jobs.Post(job);
        }
    }
}
