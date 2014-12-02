using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveDuplicatesLinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList myList = new MyList();
            myList.Add(5);
            myList.Add(12);
            myList.Add(5);
            myList.Add(66);
            myList.Add(1);
            myList.Add(66);
            Console.WriteLine("Before:");
            myList.display();

            //myList.RemoveDuplicates();
            myList.RemoveDuplicatesNoBuffer();
            Console.WriteLine("After:");
            myList.display();
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

        public void RemoveDuplicates()
        {
            Node prev = null;
            Node current = head;
            HashSet<int> existing = new HashSet<int>();

            while (current != null)
            {
                if (existing.Contains(current.Value))
                {
                    // delete the current node
                    prev.Next = current.Next;

                }
                else
                {
                    prev = current;
                    existing.Add(current.Value);
                }
                current = current.Next;
            }
        }


        public void RemoveDuplicatesNoBuffer()
        {
            Node prev = null;
            Node current = head;

            while (current != null)
            {
                if (alreadyEncountered(head, current))
                {
                    // delete the current node
                    prev.Next = current.Next;

                }
                else
                {
                    prev = current;
                }
                current = current.Next;
            }
        }

        bool alreadyEncountered(Node head, Node now)
        {
            Node current = head;

            while (!current.Equals(now))
            {
                if (current.Value == now.Value)
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }
    }

}
