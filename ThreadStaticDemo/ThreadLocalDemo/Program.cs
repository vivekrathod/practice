using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadLocalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // contruct on this thread
            ThreadLocalDemo demoObject = new ThreadLocalDemo();
            demoObject.PrintCounterValue();
            demoObject.IncrementCounter();
            demoObject.PrintCounterValue();

            // lets pass this demoObject to another thread - and then try to IncrementCounter
            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(state =>
            {
                // use the previously created demo object
                ((ThreadLocalDemo)state).PrintCounterValue();
                ((ThreadLocalDemo)state).IncrementCounter();
                ((ThreadLocalDemo)state).PrintCounterValue();

                //create a new one
                var demo = new ThreadLocalDemo();
                demo.IncrementCounter();
                demo.PrintCounterValue();

                resetEvent.Set();
            }, demoObject);

            resetEvent.WaitOne();
        }
    }

    public class ThreadLocalDemo
    {
        private ThreadLocal<int> counter;
        private static ThreadLocal<int> staticCounter;
        private static Guid staticId;
        private Guid id;

        static ThreadLocalDemo()
        {
            staticCounter = new ThreadLocal<int>();
            staticId = Guid.NewGuid();
            Console.WriteLine($"Static constructor:");
            Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}, type id: {staticId}");
        }

        public ThreadLocalDemo()
        {
            counter = new ThreadLocal<int>();
            id = Guid.NewGuid();
            Console.WriteLine($"Constructor: thread id: {Thread.CurrentThread.ManagedThreadId}, object id {id}");
        }

        public void IncrementCounter()
        {
            Console.WriteLine("Incrementing both local and static counters");
            counter.Value++;
            staticCounter.Value++;
        }

        public void PrintCounterValue()
        {
            Console.WriteLine("Printing current values");
            Console.WriteLine($"thread id: {Thread.CurrentThread.ManagedThreadId}, object id {id}, counter: {counter.Value}, static Counter: {staticCounter.Value}");
        }
    }
}
