using System;
using System.Collections.Generic;
using System.Text;

namespace app.TPL_DataflowQueue.PoisonQueue
{
    public class MyPoisonQueue : IJobQueue<string>
    {
        public void Enqueue(string str)
        {
            // do something
        }
    }
}
