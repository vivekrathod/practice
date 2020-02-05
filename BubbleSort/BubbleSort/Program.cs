using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] list = new [] { 34,22,21,10,6,4,3,1 };

            Console.WriteLine("Bubble sorting..");
            for (int k = 0; k < list.Length-1; k++)
            {
                for (int i = 0; i < list.Length - 1 - k; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
                PrintArray(list);
            }
            Console.ReadKey();

            Console.WriteLine("Bubble sorting with early exit..");
            while (true)
            {
                bool swap = false;
                for (int i = 0; i < list.Length-1; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        var temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        swap = true;
                    }
                    Console.Write(string.Format("{0}, ",list[i]));
                }
                Console.Write(list[list.Length - 1]);
                Console.WriteLine();
                if (!swap) break;
            }
            Console.ReadKey();
        }

        static void PrintArray(int[] list)
        {
            foreach (var i in list)
            {
                Console.Write(i+" ");
            }
            Console.WriteLine();
        }
    }
}
