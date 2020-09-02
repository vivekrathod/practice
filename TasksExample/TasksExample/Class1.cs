using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksExample
{
    public class Class1
    {
        public async Task<int> Method1Async()
        {
            Console.WriteLine("Inside Method1");

            //await Method2Async();
            await Task.Run(
                () =>
                    Thread.Sleep(10000)
                );

            Console.WriteLine("Exiting Method1");

            return 0;
        }

        public void Method2()
        {
            Console.WriteLine("Inside Method2");

            Thread.Sleep(1000);

            Console.WriteLine("Exiting Method2");
        }


        public Task Method3NotAsync()
        {
            Console.WriteLine("Inside Method3");

            Task innerTask = Task.Factory.StartNew(()=>Thread.Sleep(10000));
            Console.WriteLine("Inner task started");
            
            Console.WriteLine("Exiting Method3");
            return innerTask;
        }

        public async Task Method4Async()
        {
            Console.WriteLine("Inside Method4");
            var testList = new List<int>{1,2,3};
            var allTasks = 
                testList
                    .Select(async item => { Console.WriteLine($"Inside inner select Method {item}");  await Task.Delay(5000); Console.WriteLine($"Exiting inner select Method {item}"); }
                    );
            await Task.WhenAll(allTasks);
            Console.WriteLine($"Exiting Method4");
        }
    }
}
