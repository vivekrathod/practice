using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBinaryIsSubtree
{
    class Program
    {
        static void Main(string[] args)
        {
            // subtree
            //Node root1 = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };
            //Node root2 = new Node { Value = 8, Right = new Node { Value = 6 } } ;
            
            //not a subtree
            //Node root1 = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };
            //Node root2 = new Node { Value = 8, Right = new Node { Value = 6 }, Left = new Node { Value = 2 } };

            // not
            //Node root1 = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };
            //Node root2 = new Node { Value = 9, Right = new Node { Value = 6 } };

            //subtree
            //Node root1 = new Node
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
            //    },
            //    Right = new Node { Value = 10 }
            //};
            //Node root2 = new Node
            //{
            //    Value = 7,
            //    Left = new Node
            //    {
            //        Value = 6,
            //        Left = new Node { Value = 5 },
            //    },
            //};

            // not a subtree
            //Node root1 = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };
            //Node root2 = new Node { Value = 9};

            // subtree
            Node root1 = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };
            Node root2 = new Node { Value = 6 };


            if (IsSubtree(root1, root2))
                Console.WriteLine("root2 is a subtree of root1");
            else
                Console.WriteLine("root2 is not a subtree of root1");
        }

        static bool IsSubtree(Node node1, Node node2)
        {
            // determine if the binary tree at root node node2 is a subtree of binary tree at root node node1

            // 1. DFS iterate over node1 tree looking for matching node1 value
            // 2. when found matching node2 (by value) in node1 tree
            // 3. begin DFS over node2 tree and the node1 subtree
            // 4. at each node traversal compare the current node values
            // 5. at any point if a mismatch is found continue search for matching node, and repeat 3,4,5
            // 6. otherwise continue until all nodes are compared and trees are found to be equal

            Node current = node1;
            bool backtrack = false;
            Stack<Node> nodes = new Stack<Node>();
            while (true)
            {
                if (backtrack)
                {
                    current = current.Right;
                    backtrack = false;
                }
                else
                {
                    if (current.Value == node2.Value)
                    {
                        if (TreesAreEqual(current, node2))
                            return true;
                        // else continue searching for next matching node
                    }
                    nodes.Push(current);
                    current = current.Left;
                }

                if (current == null)
                {
                    if (nodes.Count == 0)
                        return false;
                    current = nodes.Pop();
                    backtrack = true;
                }

            }
        }

        // DFS over two trees and compare values at each node
        static bool TreesAreEqual(Node node1, Node node2)
        {
            Stack<Node> nodes1 = new Stack<Node>();
            Stack<Node> nodes2 = new Stack<Node>();
            Node current1 = node1;
            Node current2 = node2;
            bool backtrack = false;
            while (true)
            {
                if (backtrack)
                {
                    current1 = current1.Right;
                    current2 = current2.Right;
                    backtrack = false;
                }
                else
                {
                    if (current1.Value != current2.Value)
                        return false;
                    nodes1.Push(current1);
                    nodes2.Push(current2);
                    current1 = current1.Left;
                    current2 = current2.Left;
                }

                if ((current1 == null && current2 != null)
                     || (current1 != null && current2 == null))
                    return false;

                if (current1 == null) // or current2 == null as both should be null at this point
                {
                    if (nodes1.Count == 0 && nodes2.Count == 0)
                        return true;
                    current1 = nodes1.Pop();
                    current2 = nodes2.Pop();
                    backtrack = true;
                }

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
