using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBinaryBFS
{
    class Program
    {
        static void Main(string[] args)
        {
            //Node root = new Node
            //{
            //    Value = 9,
            //    Left = new Node
            //    {
            //        Value = 7,
            //        Left = new Node
            //        {
            //            Value = 6,
            //            Left = new Node { Value = 5 },
            //        },
            //        Right = new Node { Value = 8 }
            //    },
            //    Right = new Node
            //    {
            //        Value = 10,
            //        //Left = new Node { Value = 8 },
            //        Right = new Node
            //        {
            //            Value = 11,
            //            Left = new Node { Value = 12 },
            //        }
            //    }
            //};

            Node root = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };

            Node node = FindNodeBFS(root, 8);
            if (node == null)
                Console.WriteLine("Matching node not found");
            else
                Console.WriteLine("Found matching node, node.Left={0}, node.Right={1}", node.Left == null ? -1 : node.Left.Value, node.Right == null ? -1 : node.Right.Value);
        }

        static Node FindNodeBFS(Node root, int value)
        {
            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(root);

            while (nodes.Count > 0)
            {
                Node node = nodes.Dequeue();
                // visit each node and its siblings before you move down to visiting the children

                if (node.Value == value)
                    return node;
                // queue all children
                if (node.Left != null)
                    nodes.Enqueue(node.Left);
                if (node.Right != null)
                    nodes.Enqueue(node.Right);
            }

            return null;
        }


    }

    public class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
    }
}
