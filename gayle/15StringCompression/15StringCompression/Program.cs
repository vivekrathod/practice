using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15StringCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            string my = "abccc";
            Console.WriteLine("compressed: {0}", CompressString(my));
        }

        static string CompressString(string my)
        {
            if (my.Length < 1)
                return my;

            StringBuilder sb = new StringBuilder();

            int prev = 0;
            int count = 1;

            for (int i = 1; i < my.Length; i++)
            {
                if (my[i] == my[prev])
                {
                    count++;
                }
                else
                {
                    sb.Append(my[prev]);
                    sb.Append(count);
                    count = 1;
                    prev = i;
                }
            }

            sb.Append(my[prev]);
            sb.Append(count);

            return sb.ToString().Length < my.Length ? sb.ToString() : my;
        }
    }
}
