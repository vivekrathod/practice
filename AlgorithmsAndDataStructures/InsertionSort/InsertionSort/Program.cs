using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertionSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] list = { 3, 1, 2 };
            int length = list.Length;
            for (int i = 1; i < length; i++)
            {
                int valueToInsert = list[i];
                for (int j = i-1; j >= 0; j--)
                {
                    if (valueToInsert < list[j])
                    {
                        // shift right to create a hole
                        list[j+1] = list[j];
                        if (j == 0)
                            list[j] = valueToInsert;
                    }
                    else
                    {
                        list[j+1] = valueToInsert;
                        break;
                    }
                }
            }

            foreach (var item in list)
            {
                Console.Write("{0} ", item);
            }
            Console.ReadKey();
        }
    }
}
