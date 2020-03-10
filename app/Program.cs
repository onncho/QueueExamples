using System;
using System.Linq;
using System.Threading.Tasks;
using app.TPL_DataflowQueue;
using Library;

namespace app
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine($"The answer is {new Thing().Get(19, 23)}");      
            ExamplesManager manager = new ExamplesManager();

            //manager.StartReactiveExtensionsQueue();
            //var t1 = manager.StartChannelQueue();
            manager.StartTPLDataFlowMultipleHandlers();
            Console.ReadKey();
        }
    }

    public class ExamplesManager
    {

        public void StartReactiveExtensionsQueue()
        {
            //Reactive Extension Queue
            var q = new RxQueuePubSub();
            q.RegisterHandler<JobA>(j => Console.WriteLine(Global.Counter));
            q.RegisterHandler<JobB>(j => Global.Counter++);
            q.Enqueue(new JobA());//print
            q.Enqueue(new JobB());//add
            q.Enqueue(new JobA());//print
            q.Enqueue(new JobB());//add
            q.Enqueue(new JobB());//add
            q.Enqueue(new JobA());//print
        }


        public async Task StartChannelQueue()
        {
            var q = new ChannelsQueuePubSub();
            q.RegisterHandler<JobA>(j => Console.WriteLine(Global.Counter));
            q.RegisterHandler<JobB>(j => Global.Counter++);

            await q.Enqueue(new JobA());//print
            await q.Enqueue(new JobB());//add
            await q.Enqueue(new JobA());//print
            await q.Enqueue(new JobB());//add
            await q.Enqueue(new JobB());//add
            await q.Enqueue(new JobA());//print
        }

        public void StartTPLDataFlowMultipleHandlers()
        {
            var q = new TPLDataflowMultipleHandlers();
            var numbers = Enumerable.Range(1, 10);
            foreach (var num in numbers)
            {
                q.Enqueue(num.ToString());
            }
        }

    }
}
