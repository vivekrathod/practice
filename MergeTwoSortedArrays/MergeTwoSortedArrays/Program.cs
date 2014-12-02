using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeTwoSortedArrays
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] array1 = { 5, 6, -1, -1 };
            //int[] array2 = { 2, 8 };
            //Merge(array1, array2, 1);

            //int[] array1 = { 5, 6, 8, -1, -1 };
            //int[] array2 = { 2, 7 };
            //Merge(array1, array2, 2);

            //int[] array1 = { 5, 6, 8, -1 };
            //int[] array2 = { 7 };
            //Merge(array1, array2, 2);


            //int[] array1 = { 5, 6, 8, -1, -1 };
            //int[] array2 = { 2, 4 };
            //Merge(array1, array2, 2);

            int[] array1 = { 5, 6, 8, -1, -1 };
            int[] array2 = { 9, 11 };
            Merge(array1, array2, 2);

            foreach (var item in array1)
            {
                Console.WriteLine(string.Format("{0}, ",item));
            }
        }

        static void Merge(int[] array1, int[] array2, int array1EndIndex)
        {
	        int array2EndIndex = array2.Length -1;
	        int array1ExtraSpaceEndIndex = array1.Length - 1;
	        int array2CurrentIndex = array2EndIndex;
	        int array1CurrentIndex = array1EndIndex;
	        // start iterating backwards from the last element of array2
	        while (true)
	        {
		        // compare the current element in array2 with the element in array1
		        // put the greater of the two at the end of array1
		        // if array2 elem was greater then move on to next index in array2 and repeat until index becomes 0
		        // if array1 elem was greater then move on to next index in array1 and repeat until index becomes 0

		        if (array2[array2CurrentIndex] > array1[array1CurrentIndex])
		        {
			        array1[array1ExtraSpaceEndIndex] = array2[array2CurrentIndex];
			        array2CurrentIndex--;
		        }
		        else
		        {
			        array1[array1ExtraSpaceEndIndex] = array1[array1CurrentIndex];
			        array1CurrentIndex--;
		        }
		        array1ExtraSpaceEndIndex--;
		
		        if (array2CurrentIndex < 0)
			        return;
		
		        if (array1CurrentIndex < 0)
		        {
			        // means we have elements in array2 which need to be put in array1 starting at index 0 
			        for (int i = 0; i <= array2CurrentIndex; i++)
			        {
				        array1[i] = array2[i];
			        }
			        return;
		        }
	        }
        }
    }
}
