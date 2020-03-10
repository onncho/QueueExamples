

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

public class ChannelsQueue
{
    private ChannelWriter<string> _writer;
 
    public ChannelsQueue()
    {
        var channel = Channel.CreateUnbounded<string>();
        var reader = channel.Reader;
        _writer = channel.Writer;
 
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
 

    //It also has Bound capabilities, where the channel has a limit. 
    //When the limit is reached, the WriteAsync task waits until the channel can add the given job. That’s why Write is a Task.
    public async Task Enqueue(string job)
    {
        await _writer.WriteAsync(job);
    }
 
    public void Stop()
    {
        _writer.Complete();
    }
}