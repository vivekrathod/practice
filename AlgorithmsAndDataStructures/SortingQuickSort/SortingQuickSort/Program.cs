using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingQuickSort
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] array = { 1, 2, 3 };
            //int[] array = { 3, 2, 1 };
            //int[] array = { 1, 2, 1 };
            //int[] array = {  2, 1 };
            
            //int[] array = { 3, 1, 2 };
            //int[] array = { 4, -1, 1, 0, 2, 3, 7 };
            //int[] array = { 4, 5, 1, 0, 2, 3, 7 };
            //int[] array = { 7,3,7,8,9 };
            int[] array = { 7, 3, 7, 4, 5 };
            //QuickSort(array, 0, array.Length - 1);
            Console.WriteLine("partition point: {0}", PartitionGayle(array, 0, array.Length - 1));
            //QuickSortGayle(array, 0, array.Length - 1);
            foreach (var item in array)
            {
                Console.Write("{0}, ", item);
            }
        }

        static void QuickSort(int[] array, int start, int end)
        {
	        if (start == end)
		        return;
		
	        int l = start;
	        int r = end;
	        int m = l + (r-l)/2;
	
	        // split left and right such that left side elements are smaller than mid element and right side are greater
	        // numbers before l are always less than or equal to m, and numbers after r are always greater than or equal to m
	        while(l <= r)
	        {
		        int mid = array[m];
		        int left = array[l];
		        int right = array[r];
		        if (l < m && m < r)
		        {
			        if (left > mid)
			        {
				        swap(array, l, r);
				        r--;
			        }
			        else
				        l++;
		        }
		        else if (l == m)
			        l++;
		        else if (m == r)
			        r--;
		        else if (l == r)
		        {
			        if ((m > l && mid < left) || (m < l && mid > left))
			        {
				        swap(array, l, m);
			        }
                    l++; r--;
		        }
		        else if (m < l && m < r)
		        {
			        if (left < mid)
			        {
				        swap(array, l, m);
				        m = l;
			        }
			        l++;
		        }
		        else // (m > l && m > r) 
		        {
			        if (mid < left)
			        {
				        swap(array, l, m);
				        m = l;
			        }
			        l++;
		        }
	        }

            QuickSort(array, start, m);
            QuickSort(array, m+1, end);
        }

        
        static void swap(int[] array, int l, int r)
        {
            int temp = array[l];
            array[l] = array[r];
            array[r] = temp;
        }


        static void QuickSortGayle(int[] arr, int left, int right)
        {
            int index = PartitionGayle(arr, left, right);
            if (left < index - 1)
                QuickSortGayle(arr, left, index - 1);
            if (index < right)
                QuickSortGayle(arr, index, right);
        }

        static int PartitionGayle(int[] arr, int left, int right)
        {
            int pivot = arr[(left + right) / 2];
            while (left <= right)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;

                if (left <= right)
                {
                    swap(arr, left, right);
                    left++;
                    right--;
                }
            }

            return left;
        }
    }
}
