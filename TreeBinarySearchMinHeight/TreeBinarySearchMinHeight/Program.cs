using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBinarySearchMinHeight
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] array = { 4, 7, 8, 9, 10, 12, 14 };

            Node root = GenMinHtTree(array, 0, array.Length - 1);

        }

        static Node GenMinHtTree(int[] array, int start, int end)
        {

            Node node = new Node();
            if (start == end)
            {
                node.Right = null;
                node.Left = null;
                node.Value = array[start];
                return node;
            }
            else if (end == start + 1)
            {
                node.Value = array[end];
                node.Right = null;
                node.Left = new Node();
                node.Left.Value = array[start];
                return node;
            }
            else // end > start + 1
            {
                int mid = (end - start) / 2 + start;
                node.Value = array[mid];
                node.Right = GenMinHtTree(array, mid + 1, end);
                node.Left = GenMinHtTree(array, start, mid - 1);
            }
            return node;
        }
    }

    public class Node
    {
	    public int Value;
	    public Node Left;
	    public Node Right;
    }

}
