using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingTwoStringsAreAnagrams
{
    class Program
    {
        static void Main(string[] args)
        {
            string string1 = "abac";
            string string2 = "baac";
            if (AreAnagrams(string1, string2))
                Console.WriteLine("String {0} and {1} are anagrams", string1, string2);
            else
                Console.WriteLine("String {0} and {1} are NOT anagrams", string1, string2);
        }

        static bool AreAnagrams(string string1, string string2)
        {
	        // their lengths should match
	        if (string1.Length != string2.Length)
		        return false;
	
	        // assuming that case does not matter
	        string lower1 = string1.ToLower();
	        string lower2 = string2.ToLower();
	
	        Dictionary<char, int> source = new Dictionary<char, int>();
	        foreach (char c in lower1)
	        {
		        if (source.ContainsKey(c))
			        source[c]++;
		        else
			        source.Add(c,1);
	        }
	
	        foreach(char c in lower2)
	        {
		        if (!source.ContainsKey(c))
			        return false;
		        else
		        {
			        if (--source[c] == 0)
				        source.Remove(c);
		        }
	        }
	
	        return source.Count == 0;
        }
    }
}
