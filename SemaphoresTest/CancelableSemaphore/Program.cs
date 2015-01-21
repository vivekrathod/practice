using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CancelableSemaphore
{
    class Program
    {
        static readonly int semSize = 10;
        static CancellationTokenSource cts = new CancellationTokenSource();
        static SemaphoreSlim sem = new SemaphoreSlim(0, semSize);

        static void Main(string[] args)
        {
            int noOfThreads = 20;
            Thread[] threads = new Thread[noOfThreads];
            Console.WriteLine("Calling WaitOne on the semaphore in {0} threads", noOfThreads);
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(e =>
                {
                    try
                    {
                        sem.Wait(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Thread {0} was canceled", (int)e);
                    }
                });
                threads[i].Start(i);
            }


            Console.WriteLine("Calling Release on the semaphore {0} times", noOfThreads);
            // calling more than the semSize will throw and does not actually release *any* wiaitng (WaitOne) threads
            try { sem.Release(noOfThreads); }
            catch (SemaphoreFullException) { Console.WriteLine("Caught SemaphoreFullException"); }

            // calling Cancel on Cancellation Token Source releases any waiting threads either
            Console.WriteLine("Calling Cancel on Cancellation Token Source");
            cts.Cancel();

            Console.WriteLine("Waiting for all threads to join...");
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
                Console.WriteLine("Thread {0} fell through..", i); // you will never see this
            }
            Console.WriteLine("All threads fell through"); // you will never see this

            Console.ReadKey();
        }
    }
}
