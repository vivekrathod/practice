using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListReverse
{
    class Program
    {
        static void Main(string[] args)
        {

            MyList list = new MyList();
            list.Add(3);
            list.Add(2);
            list.Add(1);
            list.Add(33);
            list.Add(22);
            list.Add(11);

            Console.WriteLine("Before");
            list.display();

            //list.Reverse();
            //Console.WriteLine("After");
            //list.display();

            Node newHead = list.ReverseUsingStack();
            Console.WriteLine("After");
            while (newHead != null)
            {
                Console.WriteLine(newHead.Value);
                newHead = newHead.Next;
            }

        }

        
    }

    public class Node
    {
	    public int Value;
	    public Node Next;
    
    }

    public class MyList
    {
        Node head;

        public void Add(int value)
        {
        
            Node node = new Node() { Value = value };

            if (head == null)
                head = node;
            else
            {
                node.Next = head;
                head = node;
            }
        }

        public void display()
        {
            Node curr = head;
            while (curr != null)
            {
                Console.WriteLine(" " + curr.Value);
                curr = curr.Next;
            }
        }


        public void Reverse()
        {
	        Node current = head;
	        Node prev = null;
	        while(current != null)
	        {
		        Node next = current.Next;
		        current.Next = prev;
		        prev = current;
		        current = next;
	        }
            head = prev;
        }

        public Node ReverseUsingStack()
        {
            Stack<int> stack = new Stack<int>();
            Node current = head;
            while (current != null)
            {
                stack.Push(current.Value);
                current = current.Next;
            }


            Node node = new Node();
            node.Value = stack.Pop();
            Node newHead = node;

            while (stack.Count > 0)
            {
                node.Next = new Node();
                node.Next.Value = stack.Pop();
                node = node.Next;

            }
            return newHead;
        }
    }
}
