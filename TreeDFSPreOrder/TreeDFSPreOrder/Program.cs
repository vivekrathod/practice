using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDFSPreOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            Node root = new Node
            {
                Value = 9,
                Left =
                    new Node
                    {
                        Value = 6,
                        Right = new Node {Value = 8, Left = new Node {Value = 7}},
                        Left = new Node {Value = 2, Right = new Node {Value = 3}}
                    },
                Right =
                    new Node
                    {
                        Value = 15,
                        Right = new Node {Value = 17, Left = new Node {Value = 16}},
                        Left = new Node {Value = 13, Left = new Node {Value = 12}, Right = new Node {Value = 14}}
                    }

            };

            Console.WriteLine("PreOrder traversal..");
            PreOrder(root);

            Console.WriteLine("InOrder traversal..");
            InOrder(root);

            Console.WriteLine("PostOrder traversal..");
            PostOrder(root);

        }

        static void PreOrder(Node n)
        {
            if (n == null) return;

            // visit the node
            Console.WriteLine(n.Value);

            PreOrder(n.Left);
            PreOrder(n.Right);
        }

        static void InOrder(Node n)
        {
            if (n == null) return;

            InOrder(n.Left);

            //visit the node
            Console.WriteLine(n.Value);

            InOrder(n.Right);
        }

        static void PostOrder(Node n)
        {
            if (n == null) return;

            PostOrder(n.Left);
            PostOrder(n.Right);

            //visit the node
            Console.WriteLine(n.Value);
        }


    }
}
