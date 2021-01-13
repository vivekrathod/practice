using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _49BinaryTreePathsToSum
{
    class Program
    {
        static void Main(string[] args)
        {
            Node root = new Node
            {
                Value = 1,
                Left = new Node
                {
                    Value = -2, 
                    Left = new Node {Value = 5}, 
                    Right = new Node {Value = -2}
                },
                Right = new Node
                {
                    Value = 4,
                    Left = new Node { Value = 3 },
                    Right = new Node { Value = -2 }
                }
            };
            PreOrder(root, 4, 0, new List<Node>());
        }

        static void PrintPath(List<Node> path, int index, Node node)
        {
            for (int i = index; i < path.Count; i++)
            {
                Console.Write("{0}, ", path[i].Value);
            }

            Console.WriteLine(node.Value);
        }

        static void PreOrder(Node node, int sum, int currentSum, List<Node> path)
        {
            currentSum += node.Value;
            CheckSum(node, sum, currentSum, path);
            
            path.Add(node);

            if (node.Left != null)
                PreOrder(node.Left, sum, currentSum, path);

            if (node.Right != null)
                PreOrder(node.Right, sum, currentSum, path);

            path.Remove(node);
        }

        static void CheckSum(Node node, int sum, int currentSum, List<Node> path)
        {
            if (node.Value == sum) // just print the node itself
                PrintPath(new List<Node>(), 0, node);

            for (int i = 0; i < path.Count; i++)
            {
                if (currentSum == sum)
                    PrintPath(path, i, node);
                currentSum -= path[i].Value;
            }
        }
    }

    public class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
    }
}
