using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenCombinations
{
    class Program
    {
        static void Main(string[] args)
        {
            Test(new List<int>(new int[]{0,1,2,3}), 3);
        }

        public static List<List<int>> GenCombinations(List<int> list, int size)
        {
            List<List<int>> combinations = new List<List<int>>();

            if (size == 1)
            {
                foreach (var item in list)
                {
                    List<int> combination = new List<int>();
                    combination.Add(item);
                    combinations.Add(combination);
                }
                return combinations;
            }

            List<int> listCopy = new List<int>(list);
            foreach (int item in list)
            {
                listCopy.Remove(item);

                if (listCopy.Count < (size - 1))
                    break;

                foreach (List<int> combination in GenCombinations(listCopy, size - 1))
                {
                    combination.Add(item);
                    combinations.Add(combination);
                }
            }

            return combinations;
        }

        public static List<List<int>> GenAllCombinations(List<int> list, int size)
        {
            List<List<int>> combinations = new List<List<int>>();
            
            if (size == 1)
            {
                foreach (var item in list)
                {
                    List<int> combination = new List<int>();
                    combination.Add(item);
                    combinations.Add(combination);
                    Console.WriteLine(item);
                }
                return combinations;
            }

            List<int> listCopy = new List<int>(list);
            foreach (int item in list)
            {
                listCopy.Remove(item);

                if (listCopy.Count < (size - 1))
                    break;

                foreach (List<int> combination in GenAllCombinations(listCopy, size - 1))
                {
                    combination.Add(item);
                    combinations.Add(combination);
                    combination.ForEach(e => Console.Write("{0}, ", e));
                    Console.WriteLine();
                }

                listCopy.Add(item);
            }

            return combinations;
        }

        public static void Test(List<int> list, int size)
        {
            GenAllCombinations(list, size);
            //foreach (List<int> comb in GenCombinations(list, size))
            //{
            //    foreach (int item in comb)
            //    {
            //        System.Console.Write("{0}, ", item);
            //    }
            //    System.Console.WriteLine();
            //}
        }
    }
}
