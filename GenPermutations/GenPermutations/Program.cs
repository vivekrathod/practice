using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenPermutations
{
    class Program
    {
        static void Main(string[] args)
        {
            Test(new List<int>(new int[] { 0, 1, 2, 3 }), 2);
        }

        public static void Test(List<int> list, int size)
        {
            foreach (List<int> perm in GenPermutations(list, size))
            {
                foreach (int item in perm)
                {
                    System.Console.Write("{0}, ", item);
                }
                System.Console.WriteLine();
            }
        }

        public static List<List<int>> GenPermutations(List<int> items, int size)
        {
            List<List<int>> permutations = new List<List<int>>();

            if (size == 1)
            {
                foreach (int item in items)
                {
                    List<int> permutation = new List<int>();
                    permutation.Add(item);
                    permutations.Add(permutation);
                }

                return permutations;
            }

            List<int> itemsCopy = new List<int>(items);
            foreach (int item in items)
            {
                itemsCopy.Remove(item);
                foreach (List<int> permutation in GenPermutations(itemsCopy, size-1))
                {
                    permutation.Add(item);
                    permutations.Add(permutation);
                }
                itemsCopy.Add(item);
            }

            return permutations;
        }


        public static List<List<int>> GenPerms(List<int> items, int size)
        {
            List<List<int>> permutations = new List<List<int>>();

            if (size == 1)
            {
                foreach (int item in items)
                {
                    List<int> permutation = new List<int>();
                    permutation.Add(item);
                    permutations.Add(permutation);
                }

                return permutations;
            }

            List<int> itemsCopy = new List<int>(items);
            foreach (int item in items)
            {
                itemsCopy.Remove(item);
                foreach (List<int> permutation in GenPerms(itemsCopy, size - 1))
                {
                    permutation.Add(item);
                    permutations.Add(permutation);
                }
                itemsCopy.Add(item);
            }

            return permutations;
        }


    }
}
