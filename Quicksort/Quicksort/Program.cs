using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quicksort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 4, 3, 7, 8, 9, 2 };
            quicksort(array, 0, array.Length-1);

            foreach (var item in array)
            {
                Console.Write("{0}, ", item);
            }
        }

        static void quicksort(int[] array, int left, int right)
        {
            int index = partition(array, left, right);
            
            if (left < index -1)
                quicksort(array, left, index-1);

            if (index < right)
                quicksort(array, index, right);
        }

        static int partition(int[] array, int left, int right)
        {
            int pivot = array[(left + right) / 2];

            while (left <= right)
            {
                // find elements that should be on the right of the pivot
                while (array[left] < pivot) left++;

                // find elements that should on the left of the pivot
                while (array[right] > pivot) right--;

                if (left <= right)
                {
                    swap(array, left, right);
                    left++;
                    right--;
                }
            }

            return left;
        }

        static void swap(int[] array, int left, int right)
        {
            int temp = array[left];
            array[left] = array[right];
            array[right] = temp;
        }
    }
}
