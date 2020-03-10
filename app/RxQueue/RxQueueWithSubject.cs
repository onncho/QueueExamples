
// 1. We want to have an Enqueue method to add jobs.
// 2. Each job will execute as soon as possible
// 3. The jobs will execute one after the other in a FIFO order
// 4. We want the jobs to execute in a background single thread.
// 5. For simplicity, our jobs will be strings printed to Console.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class RxQueueWithScheduler
{
    Subject<string> _jobs = new Subject<string>();
 
    public RxQueueWithScheduler()
    {
        _jobs.ObserveOn(Scheduler.Default)
        .Subscribe(job =>
        {
            Console.WriteLine(job);
        });
    }
 
    public void Enqueue(string job)
    {
        _jobs.OnNext(job);
    }
}