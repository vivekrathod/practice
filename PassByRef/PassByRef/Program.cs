using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassByRef
{
    class Program
    {
        public static void Main(string[] args)
        {
            string name = "old name";
            UpdateName(name);
            Console.WriteLine(name);
        }

        public static void UpdateName(string name)
        {
            name = "New Name";
        }
    }


}
