using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeNodesAtSameDepthLinkedList
{
    public class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
    }

    public class NodeLL
    {
        public Node Value;
        public NodeLL Next;
    }

    class Program
    {
        static void Main(string[] args)
        {

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

            // using iteration
            foreach (NodeLL rootLL in NodesAtSameDepth(root))
            {
                NodeLL nodeLL = rootLL;
                while (nodeLL != null)
                {
                    Console.Write("{0} ", nodeLL.Value.Value);
                    nodeLL = nodeLL.Next;
                }
                Console.WriteLine();
            }

            // using recursion
            foreach (NodeLL rootLL in RecursionDriver(root))
            {
                NodeLL nodeLL = rootLL;
                while (nodeLL != null)
                {
                    Console.Write("{0} ", nodeLL.Value.Value);
                    nodeLL = nodeLL.Next;
                }
                Console.WriteLine();
            }
        }

        static NodeLL[] NodesAtSameDepth(Node root)
        {
            // lets try using the DFS
            int maxDepth = 10;
            NodeLL[] nodesAtSameDepth = new NodeLL[maxDepth];

            Stack<Node> stack = new Stack<Node>();
            stack.Push(root);
            Stack<int> depths = new Stack<int>();
            depths.Push(0);

            while (stack.Count != 0)
            {
                Node node = stack.Pop();
                int depth = depths.Pop();

                // store the node at this depth in the corressponding linked list
                nodesAtSameDepth[depth] = InsertNodeAtTop(nodesAtSameDepth[depth], node);

                // increase the depth only once before pushing Right/Left nodes
                depth++;

                if (node.Right != null)
                {
                    stack.Push(node.Right);
                    depths.Push(depth);
                }

                if (node.Left != null)
                {
                    stack.Push(node.Left);
                    depths.Push(depth);
                }
            }

            return nodesAtSameDepth;
        }

        static NodeLL[] RecursionDriver(Node root)
        {
            int maxDepth = 10;
            NodeLL[] nodes = new NodeLL[maxDepth];
            UseRecursion(root, 0, nodes);

            return nodes;
        }

        static void UseRecursion(Node node, int depth, NodeLL[] nodes)
        {
            if (node == null)
                return;

            nodes[depth] = InsertNodeAtTop(nodes[depth], node);

            UseRecursion(node.Left, depth + 1, nodes);
            UseRecursion(node.Right, depth + 1, nodes);
        }

        // inserts node in the linked list pointed to by root node
        // if root node is empty/null then a new list with root at node 'node' is created
        static NodeLL InsertNodeAtTop(NodeLL root, Node node)
        {
            NodeLL nodeLL = new NodeLL();
            nodeLL.Value = node;
            if (root != null)
            {
                nodeLL.Next = root;
            }
            return nodeLL;
        }
    }
}
