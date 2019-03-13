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
            Console.WriteLine("Passing the 'reference' to the string \"old name\" object by value");
            UpdateNamePassByValue(name);
            Console.WriteLine(name);

            Console.WriteLine("Again, passing the 'reference' to the List object by value");
            List<string> names = new List<string>();
            UpdateNamePassByValue(names);
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Passing the 'referencee' the string \"old name\" object by reference");
            UpdateNamePassByRef(ref name);
            Console.WriteLine(name);
        }

        public static void UpdateNamePassByValue(string name)
        {
            Console.WriteLine("Original string passed by value: {0}", name);
            // here we are assigning a new object to the passed in reference
            name = "New Name";
        }

        public static void UpdateNamePassByValue(List<string> names)
        {
            names.Add("New Name");
        }

        public static void UpdateNamePassByRef(ref string name)
        {
            // here we are assigning a new object to the passed in reference
            name = "New Name";
        }
    }


}
