using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrustPilot
{
    class Program
    {
        static void Main(string[] args)
        {
            string originalString = "poultry outwits ants";
            HashSet<char> originalStringChars = new HashSet<char>();
            foreach (char c in originalString)
            {
                if (originalStringChars.Contains(c))
                    continue;
                originalStringChars.Add(c);
            }

            ICollection<string> words = ReadWordList("wordlist");
            HashSet<string> matchingUniqueWords = FindMatchingWords(words, originalStringChars);

            FreqMatch(matchingUniqueWords, originalString);
            
            //var wordsBySize = WordsBySize(matchingUniqueWords);

            string targetHashString = "4624d200580677270a54ccff86b9610e";
            FollowTheWhiteRabbit(originalString, matchingUniqueWords.ToArray(), targetHashString);
            //Combinations.CreateCombinations();
        }

        static void FollowTheWhiteRabbit(string originalString, string[] words, string targetHashString)
        {
            int countOfChars = originalString.Replace(" ", "").Length;
            Console.WriteLine("Target length {0}", countOfChars);

            double currentCount = 1;
            StringBuilder hashStringBuilder = new StringBuilder();
            StringBuilder targetStringBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                // generate combinations
                for (int k = 1; k < countOfChars; k++)
                {
                    string[] combi = new string[k];
                    Combinations.CreateCombinations(words, 0, k, 0, combi,
                        (combination) =>
                        {
                            currentCount++;
                            //MatchTheHash(combination, targetStringBuilder, md5, hashStringBuilder, targetHashString);
                            //Console.WriteLine(sb.ToString());
                            targetStringBuilder.Clear();
                            hashStringBuilder.Clear();
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
                            return charCount < countOfChars;
                        });
                }
            }
        }

        static void MatchTheHash(string[] combination, StringBuilder targetStringBuilder, MD5 md5, StringBuilder hashStringBuilder, string targetHashString)
        {
            for (int i = 0; i < combination.Length; i++)
            {
                targetStringBuilder.Append(combination[i]);
                if (i < combination.Length - 1)
                    targetStringBuilder.Append(" ");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(targetStringBuilder.ToString());
            byte[] hash = md5.ComputeHash(bytes);
            foreach (var b in hash)
            {
                hashStringBuilder.Append(b.ToString("x2"));
            }
            if (targetHashString == hashStringBuilder.ToString())
            {
                Console.WriteLine("found the anagram {0}", targetStringBuilder.ToString());
            }
        }
        static ICollection<string> ReadWordList(string filePath)
        {
            string[] words = File.ReadAllLines(filePath);
            return words;
        }

        /// <summary>
        /// finds words that have the same set of chars as the original string
        /// </summary>
        /// <returns></returns>
        static HashSet<string> FindMatchingWords(ICollection<string> words, HashSet<char> originalStringChars)
        {
            HashSet<string> matchingWords = new HashSet<string>();
            foreach (string word in words)
            {
                // ignore dupes
                if (matchingWords.Contains(word))
                    continue;

                bool valid = true;
                foreach (char c in word)
                {
                    if (!originalStringChars.Contains(c))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        /// <summary>
        /// remove words from the specified collection who's char freq for any char in the word is more than the same char's freq in the specified original string
        /// </summary>
        /// <param name="words"></param>
        /// <param name="originalString"></param>
        static void FreqMatch(ICollection<string> words, string originalString)
        {
            Dictionary<char, int> originalStringCharFreq = new Dictionary<char, int>();
            foreach (char c in originalString)
            {
                if (!originalStringCharFreq.ContainsKey(c))
                    originalStringCharFreq[c] = 0;
                originalStringCharFreq[c]++;
            }

            ICollection<string> wordsToRemove = new List<string>();
            foreach (string word in words)
            {
                Dictionary<char, int> wordCharFreq = new Dictionary<char, int>();
                foreach (char c in word)
                {
                    if (!wordCharFreq.ContainsKey(c))
                        wordCharFreq[c] = 0;
                    wordCharFreq[c]++;
                }
                if (!CompareCharFreq(originalStringCharFreq, wordCharFreq))
                {
                    wordsToRemove.Add(word);
                }
            }

            foreach (string wordToRemove in wordsToRemove)
            {
                words.Remove(wordToRemove);
            }
        }

        static bool CompareCharFreq(Dictionary<char, int> originalStringCharFreq, Dictionary<char, int> wordCharFreq)
        {
            foreach (char c in wordCharFreq.Keys)
            {
                if (wordCharFreq[c] > originalStringCharFreq[c])
                    return false;
            }
            return true;
        }

        static Dictionary<int, ICollection<string>> WordsBySize(ICollection<string> words)
        {
            Dictionary<int, ICollection<string>> wordsBySizeDictionary = new Dictionary<int, ICollection<string>>();
            foreach (string word in words)
            {
                int size = word.Length;
                if (!wordsBySizeDictionary.ContainsKey(size))
                {
                    wordsBySizeDictionary[size] = new List<string>();
                }

                wordsBySizeDictionary[size].Add(word);
            }

            return wordsBySizeDictionary;
        }

        static void FindMatchingCombinations(ICollection<string> words, ICollection<ICollection<string>> combinationsList, ref int currentSum, int originalStringSize)
        {
           
        }
    }
}
