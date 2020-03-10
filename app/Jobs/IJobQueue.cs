using System;
using System.Collections.Generic;
using System.Text;

namespace app
{
    public interface IJobQueue<T>
    {
        void Enqueue(Action T);
        void Stop();
    }
}
