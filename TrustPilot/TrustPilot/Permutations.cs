using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrustPilot
{
    class Permutations
    {
        static void FollowTheWhiteRabbit()
        {
            string testCase = "poultry outwits ants";
            //string testCase = "poultry";
            double permsCount = Factorial(testCase.Length);
            Console.WriteLine("Possible permutations {0}", permsCount);

            double currentCount = 1;
            //string anagram = "youtlrp";
            //string targetHashString = "79137408f50745bc14acbb7713d00d69";
            string targetHashString = "4624d200580677270a54ccff86b9610e";
            using (MD5 md5 = MD5.Create())
            {
                DateTime startTime = DateTime.Now;
                bool exitFlag = false;
                StringBuilder hashStringBuilder = new StringBuilder();
                CreatePermutations(testCase.ToCharArray(), 0, testCase.Length - 1, (permutation) =>
                {
                    currentCount++;
                    byte[] bytes = Encoding.UTF8.GetBytes(permutation);
                    byte[] hash = md5.ComputeHash(bytes);
                    foreach (var b in hash)
                    {
                        hashStringBuilder.Append(b.ToString("x2"));
                    }
                    if (targetHashString == hashStringBuilder.ToString())
                    {
                        Console.WriteLine("found the anagram {0}", new string(permutation));
                        return true;
                    }
                    hashStringBuilder.Clear();

                    const double displayCountInterval = 1E9;
                    if (currentCount % displayCountInterval == 0)
                    {
                        Console.WriteLine("Current count {0}", currentCount);
                        Console.WriteLine("Percent complete {0}", currentCount * 100 / permsCount);
                        TimeSpan timeSinceStart = DateTime.Now - startTime;
                        Console.WriteLine("Time elapsed since the start {0}", timeSinceStart);
                        double estimatedTimeRemaining = (permsCount - currentCount) * timeSinceStart.TotalHours / displayCountInterval;
                        Console.WriteLine("Estimated time remaining in hours {0}", estimatedTimeRemaining);
                    }

                    return false;
                }, ref exitFlag);
            }
        }

        static void CreatePermutations(char[] permutation, int startIndex, int endIndex, Func<char[], bool> func, ref bool exitFlag)
        {
            if (exitFlag)
                return;

            if (startIndex == endIndex)
            {
                if (func(permutation))
                    exitFlag = true;
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(permutation, startIndex, i);
                    CreatePermutations(permutation, startIndex + 1, endIndex, func, ref exitFlag);
                    Swap(permutation, startIndex, i);
                }
            }
        }

        static void CreatePermutations(List<string> permutations, int currentIndex, string originalString)
        {
            if (currentIndex >= originalString.Length)
                return;

            List<string> newPermutations = new List<string>();
            foreach (string permutation in permutations)
            {
                for (int i = 0; i < currentIndex; i++)
                {
                    // skip the case where the chars to be swapped are the same
                    if (permutation[i] != permutation[currentIndex])
                    {
                        char[] newPermutation = permutation.ToCharArray();
                        Swap(newPermutation, i, currentIndex);
                        newPermutations.Add(new string(newPermutation));
                    }
                }
            }

            permutations.AddRange(newPermutations);
            currentIndex++;

            CreatePermutations(permutations, currentIndex, originalString);
        }

        static void Swap(char[] permutation, int srcIndex, int destIndex)
        {
            char temp = permutation[srcIndex];
            permutation[srcIndex] = permutation[destIndex];
            permutation[destIndex] = temp;
        }

        static void TestDuplicatePerms_UniqueChars()
        {
            string[] testCases = { "1234", "as cg", "a", "a b" };
            foreach (string testCase in testCases)
            {
                List<string> perms = new List<string>();
                bool exitFlag = false;
                CreatePermutations(testCase.ToCharArray(), 0, testCase.Length - 1, (perm) =>
                {
                    perms.Add(new string(perm));
                    return false;
                }, ref exitFlag);

                if (perms.Distinct().Count() != perms.Count)
                    throw new Exception(string.Format(
                        "For test case {0} with all unique characters, the computed permutations contains duplicate entries",
                        testCase));
            }
        }

        private static void TestPermsCount_UniqueChars()
        {
            string[] testCases = { "1234", "as cg", "a", "a b" };
            foreach (string testCase in testCases)
            {
                double permsCount = 0;
                bool exitFlag = false;
                CreatePermutations(testCase.ToCharArray(), 0, testCase.Length - 1, (perm) =>
                {
                    permsCount++;
                    return false;
                }, ref exitFlag);

                // expected count is n!
                double expectedCount = Factorial(testCase.Length);


                if (permsCount != expectedCount)
                    throw new Exception(
                        string.Format(
                            "For test case {0}, the actual count of permutations {1} does not match the expected count {2}",
                            testCase, permsCount, expectedCount));

            }
        }

        private static double Factorial(int n)
        {
            double factorial = 1;
            for (int i = 2; i <= n; i++)
            {
                factorial = factorial * i;
            }
            return factorial;
        }
    }
}
