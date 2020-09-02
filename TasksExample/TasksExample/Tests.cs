using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TasksExample
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Class1 class1 = new Class1();
            var task = class1.Method4Async();
            Console.WriteLine("Doing work while the async method continues to wait...");
            task.Wait();


        }


    }
}
