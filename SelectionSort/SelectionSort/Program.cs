using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] list = { 12, 33, 32, 55, 22, 88, 5, 4, 8, 3, 9 };

            Console.WriteLine("Selection sort..");
            //choose the lowest no in the list in each inner loop. Choose the next lowest from the remaining in next outer loop iteration");
            for (int i = 0; i < list.Length-1; i++)
            {
                for (int j = i+1; j < list.Length; j++)
                {
                    if (list[i] > list[j] )
                    {
                        int temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
                Console.Write(string.Format("{0}, ", list[i]));
            }
            Console.Write(list[list.Length - 1]);
            Console.ReadKey();
        }
    }
}
