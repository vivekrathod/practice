using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SemaphoresTest
{
    public class Program
    {
        static readonly int semSize = 10;
        static Semaphore sem = new Semaphore(0, semSize);

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
                            sem.WaitOne();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Caught exceptions {0}", ex);
                        }
                    });
                threads[i].Start();
            }

            Console.WriteLine("Calling Release on the semaphore {0} times", noOfThreads);
            // calling more than the semSize will throw and does not actually release *any* wiaitng (WaitOne) threads
            try { sem.Release(noOfThreads); }
            catch (SemaphoreFullException) { Console.WriteLine("Caught SemaphoreFullException"); }
            
            // calling Close/Dispose does not seem to release any waiting threads either
            Console.WriteLine("Calling Close on the semaphore");
            sem.Close();
            sem.Dispose();

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
