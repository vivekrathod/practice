using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _45IsBinarySearchTree
{
    public class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node root = new Node
            {
                Value = 8,
                Left =
                    new Node
                    {
                        Value = 2,
                        Right = new Node { Value = 5, Left = new Node { Value = 3 }, Right = new Node { Value = 9 } }
                    }
            };
   
            if (IsBST(root))
                Console.WriteLine("Its BST");
            else
                Console.WriteLine("Its not BST");
        }

        static bool IsBST(Node root)
        {
            List<int> list = new List<int>();
            
            InOrder(root, list);
            int prev = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] < prev)
                    return false;

                prev = list[i];
            } 
   
            return true;
        }

        static void InOrder(Node n, List<int> list)
        {
            if (n == null) return;

            if (n.Left != null)
            {
                InOrder(n.Left, list);
            }

            list.Add(n.Value);
            
            if (n.Right!=null)
            {
                InOrder(n.Right, list);
            }
        }
    }
}
