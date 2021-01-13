using System;

namespace Sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            BubbleSort bs = new BubbleSort();
            var array = new[] {2, 4, 3, 5, 1};
            bs.Sort(array);
            PrintArray(array);
        }

        static void PrintArray(int[] array)
        {
            foreach (var i in array)
            {
                Console.Write(i + " ");
            }
        }

    }
}
