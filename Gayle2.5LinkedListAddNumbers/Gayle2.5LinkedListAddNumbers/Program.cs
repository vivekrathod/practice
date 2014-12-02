using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListAddNumbers
{
    /// <summary>
    /// adds two given numbers represented by linked lists 2->3->4 + 4->5->6 = 6->9->0
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Node n1 = new Node { data = 2 };
            n1.next = new Node { data = 3 };
            n1.next.next = new Node { data = 4 };

            Node n2 = new Node { data = 4 };
            n2.next = new Node { data = 5 };
            n2.next.next = new Node { data = 6 };

            Node ans = new Node();

            int carry = Add(n1, n2, ans);

            if (carry == 1)
            {
                Node carryNode = new Node();
                carryNode.data = carry;
                carryNode.next = ans;
                ans = carryNode;
            }

            // print answer
            Node print = ans;
            while (print != null)
            {
                Console.Write("{0}->", print.data);
                print = print.next;
            }
        }

        static int Add(Node n1, Node n2, Node ans)
        {
            if (n1 == null && n2 == null)
            {
                ans = null;
                return 0;
            }
            
            if (n1.next != null || n2.next != null) ans.next = new Node();
            int carry = Add(n1.next, n2.next, ans.next);

            if (n1 == null)
            {
                ans.data = n2.data + carry;
            }
            else if (n2 == null)
            {
                ans.data = n1.data + carry;
            }
            else
            {
                ans.data = n1.data + n2.data + carry;
            }

            if (ans.data >= 10)
            {
                ans.data = 10 - ans.data;
                carry = 1;
            }
            else
            {
                carry = 0;
            }

            return carry;
        }
    }

    class Node
    {
        public int data;
        public Node next;
    }
}
