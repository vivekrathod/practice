using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxSumOfConsecutiveNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] array = {9, -2, 3, -11, 12, 2, -3, -4, 20};
            //int[] array = { -9, -2, 3, -11, -12, -2, -3, -4, -20 };
            int[] array = { -9, -2, -3, -11, -12, -2, -3, -4, -20 };
            FindMaxSumConsecutiveNumbers(array);
        }

        static void FindMaxSumConsecutiveNumbers(int[] array)
        {
            int start = 0, end = 0;
            int currentMaxSum = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                int currentSum = array[i];
                for (int j = i; j < array.Length; j++)
                {
                    int nextSum = i == j ? currentSum : currentSum + array[j];
                    if (nextSum > currentMaxSum)
                    {
                        currentMaxSum = nextSum;
                        start = i;
                        end = j;
                    }
                    currentSum = nextSum;
                }
            }

            Console.WriteLine("Max sum: {0}, start index: {1}, end index: {2}", currentMaxSum, start, end);
        }
    }
}
