using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBinaryIsBalanced
{
    class Program
    {
        static void Main(string[] args)
        {
            // build a tree

            // balanced
            //Node root = new Node { Value = 9, Left = new Node { Value = 8 } };

            // un-balanced
            //Node root = new Node { Value = 9, Left = new Node { Value = 8, Left = new Node { Value = 6 } } };

            // un-balanced
            //Node root = new Node { Value = 9, Left = new Node { Value = 8, Right = new Node { Value = 6 } } };

            //un-balanced
            //Node root = new Node { Value = 9, Right = new Node { Value = 8, Right = new Node { Value = 6 } } };

            // balanced
            //Node root = new Node { Value = 9, Left = new Node { Value = 8 }, Right = new Node { Value = 10 } };
            
            //un-balanced
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
            //        //Right = new Node { Value = 10 }
            //    },
            //    Right = new Node { Value = 10 }
            //};

            //balanced
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
            //        Left = new Node
            //        {
            //            Value = 11,
            //            Left = new Node { Value = 12 },
            //        },
            //        Right = new Node { Value = 8 }
            //    }
            //};

            //un-balanced
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

            //un-balanced
            Node root = new Node
            {
                Value = 9,
                Left = new Node
                {
                    Value = 7,
                    Left = new Node { Value = 6 },
                    Right = new Node { Value = 8, Left = new Node { Value = 12 } }
                },
                Right = new Node { Value = 10 }
            };

            Console.WriteLine(IsTreeBalanced(root));
        }

        static bool IsTreeBalanced(Node node)
        {
            if (node == null)
                return true;

            int leftHt = node.Left == null ? 0 : FindHt(node.Left) + 1;
            int rightHt = node.Right == null ? 0 : FindHt(node.Right) + 1;
            if (rightHt > leftHt ? rightHt - leftHt > 1 : leftHt - rightHt > 1)
                return false;

            if (!IsTreeBalanced(node.Left))
                return false;

            return IsTreeBalanced(node.Right);
        }

        static int FindHt(Node node)
        {
            if (node == null)
                return 0;

            return Math.Max(FindHt(node.Left), FindHt(node.Right)) + 1;
        }
    }

    public class Node
    {
	    public int Value;
	    public Node Left;
	    public Node Right;
    }

    
}
