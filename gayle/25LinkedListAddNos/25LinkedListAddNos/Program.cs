using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _25LinkedListAddNos
{
    class Program
    {
        static void Main(string[] args)
        {
            Node h1 = new Node {Value = 6, Next = new Node {Value = 1, Next = new Node {Value = 5}}};
            Node h2 = new Node {Value = 9, Next = new Node {Value = 3}};
            Node result = Add(h1, h2);
        }

        public class Node
        {
            public Node Next;
            public int Value;
        }

        static Node Add(Node h1, Node h2)
        {
            Node result = null;
            Node current = null;
            Node prev = null;
            Node n1 = h1;
            Node n2 = h2;
            int carry = 0;

            while (n1 != null || n2 != null)
            {
                int ans = carry;
                if (n1 != null)
                    ans += n1.Value;
                if (n2 != null)
                    ans += n2.Value;
                if (ans >= 10)
                {
                    ans = ans % 10;
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }

                current = new Node { Value = ans };

                if (prev == null)
                {
                    result = current;
                }
                else
                {
                    prev.Next = current;
                }

                prev = current;

                if (n1 != null) n1 = n1.Next;
                if (n2 != null) n2 = n2.Next;

            }

            return result;
        }
    }
}
