using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeTuples
{
    // input = [(1,3), (70, 100), (7,9), (2,8)]
    // output = [(1,9), (70,100)]
    class Program
    {
        static void Main(string[] args)
        {
            IList<Pair> pairs = new List<Pair>();
            pairs.Add(new Pair() { First = 70, Second = 100 });
            pairs.Add(new Pair() {First = 1, Second = 3});
            pairs.Add(new Pair() { First = 7, Second = 9 });
            pairs.Add(new Pair() { First = 2, Second = 8 });
            foreach (Pair pair in MergeOverlappingPairs(pairs))
            {
                Console.Write("({0},{1}), ", pair.First, pair.Second);
            }
        }

        static IList<Pair> MergeOverlappingPairs(IList<Pair> pairs)
        {
            bool atLeastOneMerged = true;
            int i = 0;
            while (atLeastOneMerged && i < pairs.Count)
            {
                atLeastOneMerged = false;
                Pair mergedPair = pairs[i];
                for (int j = i + 1; j < pairs.Count; j++)
                {
                    Pair pair = pairs[j];
                    if (mergedPair.First <= pair.First && mergedPair.Second >= pair.First)
                    {
                        if (mergedPair.Second < pair.Second)
                            mergedPair.Second = pair.Second;
                        pairs.Remove(pair);
                        atLeastOneMerged = true;
                    }
                }
                if (!atLeastOneMerged)
                {
                    i++;
                    atLeastOneMerged = true;
                }
            }

            return pairs;
        }

        class Pair
        {
            public int First { get; set; }
            public int Second { get; set; }
        }
    }
}
