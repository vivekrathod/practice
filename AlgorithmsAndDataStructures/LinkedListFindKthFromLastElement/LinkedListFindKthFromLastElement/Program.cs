using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListFindKthFromLastElement
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList mylist = new MyList();
            mylist.Add(2);
            mylist.Add(4);
            mylist.Add(8);
            mylist.Add(3);
            mylist.Add(0);
            mylist.Display();

            int k = 2;
            Console.WriteLine("Using recursion, {0}th to last element is {1}", k, Recurse(mylist.Head, ref k).Value);

            k = 2;
            Console.WriteLine("Using iteration, {0}th to last element is {1}", k, FindKthFromLastUsingIteration(mylist.Head, k).Value);
        }

        public static Node Recurse(Node n, ref int k)
        {
	        if (n.Next == null)
		        return n;
	
	        Node node = Recurse(n.Next, ref k);
	        k--;
            if (k == 0)
                return n.Next;
            else
                return node;
        }

        public static Node FindKthFromLastUsingIteration(Node head, int k)
        {
            Node current = head;
            int counter = 0;
            Node lag = null;

            while (current != null)
            {
                counter++;
                if (counter == k)
                {
                    lag = head;
                }

                if (counter > k)
                {
                    lag = lag.Next;
                }

                current = current.Next;
            }

            return lag;
        }
    }

    public class Node
    {
        public Node Next {get;set;}
        public int Value { get; set; }
    }

    public class MyList
    {
        Node head;

        public Node Head { get {return head; }}

        public void Add(int value)
        {
            Node next = head;
            head = new Node();
            head.Value = value;
            head.Next = next;
        }

        public void Display()
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.Value);
                current = current.Next;
            }
        }
    }
}
