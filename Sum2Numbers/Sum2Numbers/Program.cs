using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sum2Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            // basic method
            // basic method
            int[] nums = {2, 3, 6, 1};
            for (int i = 0; i < nums.Length-1; i++)
            {
                for (int j = i+1; j < nums.Length; j++)
                {
                    if (nums[i]+nums[j] == 8)
                        Console.WriteLine($"Found the pair that adds to 8 at index {i}, {j}");
                }
            }
        }
    }
}
