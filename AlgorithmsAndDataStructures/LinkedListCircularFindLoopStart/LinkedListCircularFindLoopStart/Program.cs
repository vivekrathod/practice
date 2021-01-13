using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedListCircularFindLoopStart
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList list = new MyList();

            // example 1
            //Node findMe = new Node { Value = 'C' };
            //list.Add(findMe);
            //list.Add(new Node { Value = 'E' });
            //list.Add(new Node { Value = 'D' });
            
            //list.Add(findMe);
            //list.Add(new Node { Value = 'B' });
            //list.Add(new Node { Value = 'A' });

            // example 2
            //Node findMe = new Node { Value = 'A' };
            //list.Add(findMe);
            //list.Add(new Node { Value = 'B' });
            //list.Add(findMe);

            // example 3
            Node findMe = new Node { Value = 'B' };
            list.Add(findMe);
            list.Add(new Node { Value = 'C' });
            list.Add(findMe);
            list.Add(new Node { Value = 'A' });

            Console.WriteLine(DetectStartOfLoop(list.Head).Value);
        }


        static Node DetectStartOfLoop(Node head)
        {
            Node current = head;
            int counter = 0;
            while (true)
            {
                current = current.Next;
                counter++;
                if (DuplicateExists(head, current, counter))
                {
                    return current;
                }
            }
        }

        static bool DuplicateExists(Node head, Node node, int counter)
        {
            Node current = head;
            while (counter != 0)
            {
                if (current == node)
                    return true;
                current = current.Next;
                counter--;

            }
            return false;
        }
    }

    public class Node
    {
        public char Value { get; set; }
        public Node Next { get; set; }
    }

    public class MyList
    {
        public Node Head { get; private set; }

        public void Add(Node node)
        {
            if (Head == null)
                Head = node;
            else
            {
                node.Next = Head;
                Head = node;
            }
        }


        
    }
}
