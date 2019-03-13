using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustPilot
{
    public class Combinations
    {
        public static void CreateCombinations()
        {

            //string[] words = {"0", "1", "2", "3", "4", "5"};
            string[] words = { "00", "11", "22", "33", "44", "55" };
            int countOfChars = 4;
            
            for (int k = 1; k < countOfChars; k++)
            {
                string[] combi = new string[k];
                CreateCombinations(words, 0, k, 0, combi,
                    (combination) =>
                    {
                        foreach (string s in combination)
                        {
                            Console.Write(s);
                        }
                        Console.WriteLine();
                    },
                    (combination) =>
                    {
                        int charCount = 0;
                        foreach (string s in combination)
                        {
                            if (string.IsNullOrEmpty(s))
                                return false;
                            charCount += s.Length;
                            if (charCount > countOfChars)
                                return true;
                        }
                        return false;
                    });
            }
        }

        public static void CreateCombinations(string[] words, int startIndex, int k, int wordIndex, string[] combi, Action<string[]> action, Func<string[], bool> exit)
        {
            while (wordIndex <= words.Length - k)
            {
                combi[startIndex] = words[wordIndex];
                wordIndex++;
                if (exit(combi))
                    continue;
                if (k == 1)
                    action(combi);
                else
                    CreateCombinations(words, startIndex + 1, k - 1, wordIndex, combi, action, exit);    
            }
        }
    }
}
