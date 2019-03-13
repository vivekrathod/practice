using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();

            // testing async and await keywords
            Task task1 = class1.Method1Async();
            Console.WriteLine("Doing work while the async method continues to wait...");
            task1.Wait();


            // testing Task class
            Task task2 = Task.Factory.StartNew(() => class1.Method2());
            //Task task2 = Task.Run(() => class1.Method2());
            Thread.Sleep(100); // give a chance to the task to start executing the method
            Console.WriteLine("Doing work while the sync method continues to wait...");
            task2.Wait();

            Console.WriteLine("Testing if Task.Wait() yields control back to calling method when waiting for the task to finish (like await does)");
            Task innerTask = class1.Method3NotAsync();
            innerTask.Wait();
            Console.WriteLine("Inner task completed"); 
        }


    }
}
