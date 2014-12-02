using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixRotate
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] arr = new int[5][] 
            { 
                new int[5] { 1, 2, 3, 4, 5 },
                new int[5] { 2, 2, 2, 2, 7 }, 
                new int[5] { 3, 3, 3, 3, 8 }, 
                new int[5] { 4, 4, 4, 4, 9 }, 
                new int[5] { 8, 7, 6, 5, 6 }
            };

            PrintMatrix(arr);

            RotateMatrix(arr, arr.GetLength(0));

            PrintMatrix(arr);
        
        }

        static void RotateMatrix(int[][] array, int n)
        {
            int layers = n / 2;
            for (int layer = 0; layer < layers; layer++)
            {
                for (int i = layer; i < n-1-layer; i++)
                {
                    //save top
                    int top = array[layer][i];

                    // left->top
                    array[layer][i] = array[n - 1 - i][layer];

                    // bottom->left
                    array[n - 1 - i][layer] = array[n - 1 - layer][n - 1 - i];

                    // right->bottom
                    array[n - 1 - layer][n - 1 - i] = array[i][n - 1 - layer];

                    // top->right
                    array[i][n - 1 - layer] = top;
                }
            }
        }

        static void PrintMatrix(int[][] arr)
        {
            int rowLength = arr.GetLength(0);
            int colLength = arr[0].Length;

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i][j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.ReadLine();
        }

    }
}
