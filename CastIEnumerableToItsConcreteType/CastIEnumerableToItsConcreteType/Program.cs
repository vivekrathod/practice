using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastIEnumerableToItsConcreteType
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>();
            var enumList = list.Select(i => i);
            //var enumList = list as IEnumerable<string>;
            //var originalList = (List<string>) enumList;
            Console.WriteLine(enumList.GetType().FullName);
        }
    }
}
