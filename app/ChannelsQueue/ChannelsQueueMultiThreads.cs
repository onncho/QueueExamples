//Handle on Multiple Threads

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

public class ChannelsQueueMultiThreads
{
    private ChannelWriter<string> _writer;
 
    public ChannelsQueueMultiThreads(int threads)
    {
        var channel = Channel.CreateUnbounded<string>();
        var reader = channel.Reader;
        _writer = channel.Writer;

        //you can define how many dedicate threads will handle the jobs.
        for (int i = 0; i < threads; i++)
        {
            var threadId = i;
            Task.Factory.StartNew(async () =>
            {
                // Wait while channel is not empty and still not completed
                while (await reader.WaitToReadAsync())
                {
                    var job = await reader.ReadAsync();
                    Console.WriteLine(job);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
 
    public void Enqueue(string job)
    {
        _writer.WriteAsync(job).GetAwaiter().GetResult();
    }
 
    public void Stop()
    {
        _writer.Complete();
    }
}