using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace app.TPL_DataflowQueue.PoisonQueue
{

    /// <summary>
    /// This is a regular Job Queue that executes jobs on a single thread. 
    /// Only that it uses Polly to retry each job 3 times in case it fails. So if GetCustomerById() threw an exception, it will execute it three more times. 
    /// If it failed all 3 times, we will add it to the poison queue.
    /// </summary>
    public class TPLDataflowWithErrorHandling : IJobQueue<Action>
    {
        private ActionBlock<Action> _jobs;

        public TPLDataflowWithErrorHandling(IJobQueue<string> poisonQueue)
        {
            var policy =
                Policy.Handle<Exception>() // on any exception
                .Retry(3); // retry 3 times

            _jobs = new ActionBlock<Action>((job) =>
            {
                try
                {
                    policy.Execute(() =>
                    {
                        int customer = GetCustomerById(job);// possibly throws exception
                        //Console.WriteLine(customer.Name);
                    });
                }
                catch (Exception e)
                {
                    // If policy failed (after 3 retries), move to poison queue
                    poisonQueue.Enqueue(job);
                }
            });
        }

        private int GetCustomerById(Action job)
        {
            throw new NotImplementedException();
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
