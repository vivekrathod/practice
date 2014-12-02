using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBinaryDFS
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

            //Node root = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };

            //Node root = new Node { Value = 9 };

            Node root = new Node
            {
                Value = 9,
                Left = new Node
                {
                    Value = 7,
                    Left = new Node
                    {
                        Value = 6,
                        Left = new Node
                        {
                            Value = 6,
                            Left = new Node { Value = 5 },
                            Right = new Node
                            {
                                Value = 6,
                                Left = new Node
                                {
                                    Value = 6,
                                    Left = new Node { Value = 5 },
                                    Right = new Node { Value = 777 }
                                },
                            }
                        },
                    },
                    Right = new Node { Value = 8 }
                },
                Right = new Node
                {
                    Value = 10,
                    Left = new Node { Value = 8 },
                    Right = new Node
                    {
                        Value = 11,
                        Left = new Node { Value = 12 },
                    }
                }
            };

            //Node node = FindMatchingNode(root, 777);
            //Node node = FindNodeDFS(root, 8);
            //Node node = FindNodeSimple(root, 10);
            Node node = SimpleDFSRecursive(root, 777);
            if (node == null)
                Console.WriteLine("Matching node not found");
            else
                Console.WriteLine("Found matching node, node.Left={0}, node.Right={1}", node.Left == null ? -1 : node.Left.Value, node.Right == null ? -1 : node.Right.Value);
        }

        // using the backtrack flag
        static Node FindMatchingNode(Node root, int value)
        {
            Node current = root;
            bool backtrack = false;
            Stack<Node> nodes = new Stack<Node>();
            
            while (current != null)
            {
                if (backtrack)
                {
                    current = current.Right;
                    backtrack = false;
                }
                else
                {
                    if (current.Value == value)
                        return current;
                    nodes.Push(current);
                    current = current.Left;
                }

                if (current == null)
                {
                    if (nodes.Count == 0)
                        break;
                    
                    do
                    {
                        current = nodes.Pop();
                    }
                    while (current.Right == null);

                    backtrack = true;
                }
            }

            return null;
        }

        // w/o using the backtrack flag
        static Node FindNodeDFS(Node root, int value)
        {
            Node node = root;
            Stack<Node> nodes = new Stack<Node>();

            while (node != null)
            {
                if (node.Value == value)
                    return node;

                // 1. push on the stack if the current node's left is not null		
                if (node.Left != null)
                {
                    nodes.Push(node);
                    node = node.Left;
                }
                // 2. if left is null, simply assign the right node to current node 
                // (no need to push current node on stack - we will not need to visit it 
                // again since we are already traversing its right side)
                else if (node.Right != null)
                {
                    node = node.Right;
                }
                // 3. if the current node's Left and Right both are null then pop a node from the stack and assign it to current node
                else
                {
                    if (nodes.Count == 0)
                        break; // no match found

                    do
                    {
                        node = nodes.Pop().Right;
                    }
                    while (nodes.Count > 0 && node == null);
                }
            }

            // matching node was not found
            return null;
        }


        // simple and best solution so far!
        static Node FindNodeSimple(Node root, int value)
        {
            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);

            while (stack.Count != 0)
            {
                Node node = stack.Pop();
                if (node.Value == value)
                    return node;

                if (node.Right != null)
                {
                    stack.Push(node.Right);
                }

                if (node.Left != null)
                {
                    stack.Push(node.Left);
                }
            }

            // nothing found
            throw new Exception("Value not found");
        }

        
        static Node SimpleDFSRecursive(Node root, int value)
        {
            if (root == null)
                return null;

            if (root.Value == value)
                return root;

            return SimpleDFSRecursive(root.Left, value) ?? SimpleDFSRecursive(root.Right, value);
        }
    }

    
    public class Node
    {
	    public int Value;
	    public Node Left;
	    public Node Right;
    }
}
