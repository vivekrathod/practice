using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInRotatedSortedArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 6, 7, 8, 9, 1, 2, 3, 4, 5 };
            Console.WriteLine(BinarySearch(3, array, 0, array.Length - 1));
            Console.WriteLine(BinarySearch(8, array, 0, array.Length - 1));
            Console.WriteLine(BinarySearch(5, array, 0, array.Length - 1));
        }

        static int BinarySearch(int numToSearch, int[] array, int startIndex, int endIndex)
        {
            if (endIndex < startIndex)
                return -1;

            int midIndex = startIndex + (endIndex - startIndex) / 2;
            int mid = array[midIndex];
            int start = array[startIndex];
            int end = array[endIndex];

            if (numToSearch == mid)
                return midIndex;
            else if (start <= mid && mid <= end)
            {
                // normal case of a binary search where all elements are sorted
                if (numToSearch > mid)
                    return BinarySearch(numToSearch, array, midIndex + 1, endIndex);
                else
                    return BinarySearch(numToSearch, array, startIndex, midIndex - 1);
            }
            else if (start <= mid && mid >= end)
            {
                // start to mid is sorted, but mid to end is not
                if (start >= end)
                {
                    if (numToSearch >= start && numToSearch < mid)
                        return BinarySearch(numToSearch, array, startIndex, midIndex - 1);
                    else
                        return BinarySearch(numToSearch, array, midIndex + 1, endIndex);
                }
                else // start < end
                {
                    // this is not a valid case as start < end would mean its a sorted array, but
                    // in that case mid can not be greater than end
                    throw new ArgumentException("start<end; start<mid>end");
                }
            }
            else if (start >= mid && mid <= end)
            {
                // mid to end is sorted, but not start to mid
                if (start >= end)
                {
                    if (numToSearch > mid && numToSearch <= end)
                        return BinarySearch(numToSearch, array, midIndex + 1, endIndex);
                    else
                        return BinarySearch(numToSearch, array, startIndex, midIndex - 1);
                }
                else // start < end
                {
                    // this is not a valid case as start < end would mean its a sorted array, but
                    // in that case mid can not be smaller than start
                    throw new ArgumentException("start<end; start>mid<end");
                }
            }
            else // start > mid && mid > end
            {
                // means the array is sorted in reverse (descending in our case) order -  this case not possible to arrive at by rotating an originally sorted (in ascending order) array
                throw new ArgumentException("start>mid>end");
            }
        }
	


    }


}
