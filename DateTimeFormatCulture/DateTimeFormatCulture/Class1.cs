using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DateTimeFormatCulture
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test1()
        {
            var cc = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = 
                //new CultureInfo("ur-IN"); 
                new CultureInfo("fa-IR");
            //var cuc = Thread.CurrentThread.CurrentUICulture;
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ur-IN");

            Console.WriteLine(1);
            int num = 122;
            Console.WriteLine(num);
            Console.WriteLine(DateTime.Now.ToShortDateString());
        }
    }
}
