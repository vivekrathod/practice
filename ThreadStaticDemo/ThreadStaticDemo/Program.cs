using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStaticDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // contruct on this thread
            ThreadStaticDemo demoObject = new ThreadStaticDemo();
            demoObject.PrintCounterValue();
            demoObject.IncrementCounter();
            demoObject.PrintCounterValue();

            // lets pass this demoObject to another thread - and then try to IncrementCounter
            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(state =>
            {
                // use the previously created demo object
                ((ThreadStaticDemo)state).PrintCounterValue();
                ((ThreadStaticDemo) state).IncrementCounter();
                ((ThreadStaticDemo) state).PrintCounterValue();

                //create a new one
                var demo = new ThreadStaticDemo();
                demo.IncrementCounter();
                demo.PrintCounterValue();
                resetEvent.Set();
            }, demoObject);

            resetEvent.WaitOne();
        }
    }

    public class ThreadStaticDemo
    {
        [ThreadStatic] public static int Counter;

        public void IncrementCounter()
        {
            Counter++;
        }

        public void PrintCounterValue()
        {
            Console.WriteLine("Thread: {0}, Counter: {1}", Thread.CurrentThread.ManagedThreadId, Counter);
        }
    }
}
