//Implementing publisher/subscriber with System.Threading.Channels


/*
 The Verdict on System.Threading.Channels
I really like this programming model. It’s clean and very straightforward, in contrast to Rx, though maybe not as pretty.

I think the advantages of System.Threading.Channels are its asynchronous features and Bound capabilities. You should use it when:

You want a simple straightforward job queue.
You want to have one or more dedicated threads for handling the queue.
You want to limit the queue for whatever reason. This will provide an effective asynchronous API for that.
And you shouldn’t use it when:

You don’t want dedicated threads for queue handlers.
You need to implement prioritization (in that case, the BlockingCollection implementation from Part 1 is best).
 
 */

using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;


public class ChannelsQueuePubSub
{
    private ChannelWriter<IJob> _writer;
    private Dictionary<Type, Action<IJob>> _handlers = new Dictionary<Type, Action<IJob>>();

    public ChannelsQueuePubSub()
    {
        var channel = Channel.CreateUnbounded<IJob>();
        var reader = channel.Reader;
        _writer = channel.Writer;

        Task.Factory.StartNew(async () =>
        {
            // Wait while channel is not empty and still not completed
            while (await reader.WaitToReadAsync())
            {
                var job = await reader.ReadAsync();
                bool handlerExists =
                    _handlers.TryGetValue(job.GetType(), out Action<IJob> value);
                if (handlerExists)
                {
                    value.Invoke(job);
                }
            }
        }, TaskCreationOptions.LongRunning);
    }

    public void RegisterHandler<T>(Action<T> handleAction) where T : IJob
    {
        Action<IJob> actionWrapper = (job) => handleAction((T)job);
        _handlers.Add(typeof(T), actionWrapper);
    }

    public async Task Enqueue(IJob job)
    {
        await _writer.WriteAsync(job);
    }

    public void Stop()
    {
        _writer.Complete();
    }
}