using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphImplAdjList
{
    /// <summary>
    /// directed and non-weighted graph implementation using adjacency list
    /// </summary>
    public class Graph
    {
        public const int DEFAULT_SIZE = 2;
        private Node[] _nodes = new Node[DEFAULT_SIZE];
        private int _currentIndex = 0;

        public void AddNode(Node node)
        {
            // \todo sanity check for node's existence - it should be an uninitialized node (index should be -1)

            node.Index = _currentIndex;
            _nodes[_currentIndex++] = node;

            if (_currentIndex == _nodes.Length)
                ExpandSize();
        }

        public void RemoveNode(Node node)
        {
            // \todo check for node's existence
            int removeIndex = node.Index;

            // reset the node's adj list
            _nodes[removeIndex].List = new List<Node>();
            // remove the node
            for (int i = removeIndex; i < _nodes.Length - 1; i++)
            {
                // shift left to fill the hole
                _nodes[i] = _nodes[i + 1];
                Node currentNode = _nodes[i];
                if (currentNode == null)
                    continue;
                currentNode.Index = i;
            }

            // remove the node from the adj lists of remaining nodes
            foreach (var currentNode in _nodes)
            {
                if (currentNode != null)
                {
                    if (currentNode.List.Contains(node))
                        currentNode.List.Remove(node);
                }
            }

            _currentIndex--;
        }

        public void AddEdge(Node from, Node to)
        {
            // sanity check for nodes' existence
            if (from.Index == -1 || from.Index >= _currentIndex || to.Index == -1 || to.Index >= _currentIndex)
                throw new ArgumentException("either from or to nodes dont exist");

            // \todo validate that the 'from' node is the same as the Node at _nodes[from.Index]

            _nodes[from.Index].List.Add(to);
        }

        public void RemoveEdge(Node from, Node to)
        {
            // sanity check for node's existence
            if (from.Index == -1 || from.Index >= _currentIndex || to.Index == -1 || to.Index >= _currentIndex)
                throw new ArgumentException("either from or to node doesn't exist");

            // \todo validate that the 'from' node is the same as the Node at _nodes[from.Index]
            
            if (_nodes[from.Index].List.Contains(to))
            {
                _nodes[from.Index].List.Remove(to);
            }
        }

        public ICollection<Node> GetNeighbors(Node node)
        {
            // sanity check for node's existence
            if (node.Index ==-1 || node.Index >= _currentIndex)
                throw new ArgumentException("node doesn't exist");

            return _nodes[node.Index].List;
        }

        private void ExpandSize()
        {
            int expandedSize = _nodes.Length*2;
            Node[] nodes = new Node[expandedSize];
            for (int i = 0; i < _nodes.Length; i++)
            {
                nodes[i] = _nodes[i];
            }

            _nodes = nodes;
        }

        public void PrintAdjList()
        {
            foreach (var node in _nodes)
            {
                if (node == null)
                    return;
                Console.Write("{0}:  ", node.Data);
                foreach (var neighbor in node.List)
                {
                    Console.Write("{0}, ", neighbor.Data);
                }
                Console.WriteLine();
            }
        }
    }

    public class Node
    {
        public int Index = -1;
        public int Data;
        public ICollection<Node> List = new List<Node>();

        public Node(int data)
        {
            Data = data;
        }
    }
}
