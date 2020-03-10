//Rx implementation of Publisher/Subscriber
//Here, we have a single thread handling jobs, even if it’s for 2 different handlers.
//We used .Publish() in _jobs.ObserveOn(Scheduler.Default).Publish() to create a connectable observable that shares a single subscription in the sequence. 
//Without this addition, each call to .Subscribe would create its own sequence with the scheduler creating their own pooled thread for each job type. In other words, we would have 2 different threads handling the queue, which would break the FIFO order.


using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;


public class RxQueuePubSub
{
    Subject<IJob> _jobs = new Subject<IJob>();
    private IConnectableObservable<IJob> _connectableObservable;
 
    public RxQueuePubSub()
    {
        _connectableObservable = _jobs.ObserveOn(Scheduler.Default).Publish();
        _connectableObservable.Connect();
    }
 
    public void Enqueue(IJob job)
    {
        _jobs.OnNext(job);
    }
 
    public void RegisterHandler<T>(Action<T> handleAction) where T : IJob
    {
        _connectableObservable.OfType<T>().Subscribe(handleAction);
    }
}