using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppNoDeadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            Thread.Sleep(10000);
        }

        private static async Task DelayAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
            });
        }

        // This method causes a deadlock when called in a GUI or ASP.NET context.
        public static async void Test()
        {
            // Start the delay.
            var delayTask = DelayAsync();
            // Wait for the delay to complete.
            delayTask.Wait();

            // await DelayAsync();
        }
    }
}
