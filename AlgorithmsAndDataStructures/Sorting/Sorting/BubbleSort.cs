using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sorting
{
    public class BubbleSort : ISortIntArray
    {
        /// <summary>
        /// The idea is to iterate over the numbers and swap adjacent numbers if one is smaller than the next number.
        /// After the first pass, the highest number will 'bubble' up to the end. The same operation is repeated over the remaining unsorted
        /// numbers.
        /// </summary>
        /// <param name="array"></param>
        public void Sort(int[] array)
        {
            // iterate n times in an outer loop 
            for (int i = 0; i < array.Length -1; i++)
            {
                bool swapped = false;
                // for each outer iteration, iterate (n - i) times where i is the number of outer iteration
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    // exchange adjacent numbers if needed
                    if (array[j] > array[j + 1])
                    {
                        int save = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = save;
                        swapped = true;
                    }
                }

                // if there were no swaps then it would mean the array is now sorted
                if (!swapped)
                    break;
            }
        }
    }
}
