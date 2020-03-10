using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace app
{
    public class ChannelsQueue : IJobQueue<Action>
    {
        private ChannelWriter<Action> _writer;

        public ChannelsQueue()
        {
            //var channel = Channel.CreateUnbounded<string>();
            var channel = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions() { SingleReader = true });
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
        //public async Task Enqueue(Action job)
        //{
        //    await _writer.WriteAsync(job);
        //}

        public void Enqueue(Action job)
        {
            _writer.TryWrite(job);
        }

        public void Stop()
        {
            _writer.Complete();
        }
    }
}