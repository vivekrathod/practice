using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _16RotateMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            // rotate array
            int[] array = {1, 7, 2, 4, 5};
            Rotate(array, 3);
            Console.WriteLine("Rotated array: ");
            foreach (var i in array)
            {
                Console.Write("{0}, ", i);
            }
            Console.WriteLine();

            // rotate matrix
            int[,] matrix = {{3, 7, 2, 5}, {1, 5, 4, 7}, {5, 0, 2, 6}, {4, 5, 6, 8}};
            int dimLength = matrix.GetLength(0);
            Console.WriteLine("Rotated {0}x{1} matrix: ", dimLength, dimLength);
            //Rotate(matrix);
            RotateSimpler(matrix);
            for (int i = 0; i < dimLength; i++)
            {
                for (int j = 0; j < dimLength; j++)
                {
                    Console.Write("{0}, ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        // assumes NxN matrix
        static void Rotate(int[,] matrix)
        {
            int N = matrix.GetLength(0);
            // iterate over each layer along the periphery
            for (int i = 0; i < N/2; i++)
            {
                for (int j = i; j < N - 1 - i; j++)
                {
                    int temp = matrix[i, j]; // save top-left
                    matrix[i, j] = matrix[N - 1 - j, i]; // top-left = bottom-left
                    matrix[N - 1 - j, i] = matrix[N - 1 - i, N - 1 - j]; // bottom-left = bottom-right
                    matrix[N - 1 - i, N - 1 - j] = matrix[j, N - 1 - i]; // bottom-right = top-right
                    matrix[j, N - 1 - i] = temp; // top-right = top-left
                }
            }
        }

        static void RotateSimpler(int[,] matrix)
        {
            int N = matrix.GetLength(0);

            // iterate over each layer along the periphery
            for (int layer = 0; layer < N / 2; layer++)
            {
                int first = layer;
                int last = N - 1 - layer;
                for (int i = first; i < last; i++)
                {
                    int offset = i - first;
                    int temp = matrix[first, first + offset]; // save top-left
                    matrix[first, first + offset] = matrix[last - offset, first]; // top-left = bottom-left
                    matrix[last - offset, first] = matrix[last, last - offset]; // bottom-left = bottom-right
                    matrix[last, last - offset] = matrix[first + offset, last]; // bottom-right = top-right
                    matrix[first + offset, last] = temp; // top-right = top-left
                }
            }
        }

        static void Rotate(int[] array, int offset)
        {
            int[] temp = new int[offset];
            int len = array.Length;

            for (int i = len-1; i >= 0; i--)
            {
                if (i > len-1-offset)
                {
                    temp[offset - 1 - (len - 1 - i)] = array[i];
                }

                if (i >= offset)
                {
                    array[i] = array[i - offset];
                }
                else
                {
                    array[i] = temp[i];
                }
            }
        }
    }
}
