using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphImplAdjMatrix
{
    /// <summary>
    /// 1. implements a directed and weighted graph using adjacency matrix
    /// 2. weight has to be a positive integer
    /// 3. the capacity of the graph doubles when full
    /// </summary>
    public class Graph
    {
        private const int DEFAULT_SIZE = 2;
        private int _currentSize = DEFAULT_SIZE;
        private int[,] _matrix = new int[DEFAULT_SIZE, DEFAULT_SIZE];
        private Node[] _nodes = new Node[DEFAULT_SIZE];

        private int _currentIndex = 0;

        public Graph()
        {
            // set the initial weight of all the edges to -1
            // which would mean no edges exists to begin with
            for (int i = 0; i < _nodes.Length; i++)
            {
                for (int j = 0; j < _nodes.Length; j++)
                {
                    _matrix[i, j] = -1;        
                }
            }
        }

        // this is O(1).. unless expansion is needed
        public void AddNode(Node node)
        {
            if (_currentIndex == _currentSize - 1)
                ExpandSize();

            node.Index = _currentIndex;
            _nodes[_currentIndex++] = node;
        }

        // this is O(1)
        public void AddEdge(Node from, Node to, int weight = 0)
        {
            int fromIndex = from.Index;
            int toIndex = to.Index;
           
            if (fromIndex > _currentIndex || toIndex > _currentIndex)
                throw new ArgumentException("either from or to node index is out of bounds");

            _matrix[fromIndex, toIndex] = weight;
        }

        // this is O(N)
        public ICollection<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            int fromIndex = node.Index;
            for (int toIndex = 0; toIndex < _currentIndex; toIndex++)
            {
                if (_matrix[fromIndex, toIndex] > -1)
                {
                    neighbors.Add(_nodes[toIndex]);
                }
            }

            return neighbors;
        }

        // simple DFS search - this is O(N)
        public bool AreConnectedDFS(Node node1, Node node2)
        {
            if (node1 == null || node2 == null)
                throw new ArgumentException("null node");

            if (node1 == node2)
            {
                ResetVisitedStatus();
                return true;
            }

            node1.Visited = true;
            foreach (Node neighbor in GetNeighbors(node1))
            {
                if (neighbor.Visited)
                    continue;

                if (AreConnectedDFS(neighbor, node2))
                {
                    ResetVisitedStatus();
                    return true;
                }
            }

            ResetVisitedStatus();
            return false;
        }

        // BFS search
        public bool AreConnectedBFS(Node node1, Node node2)
        {
            if (node1 == null || node2 == null)
                throw new ArgumentException("null node");

            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(node1);

            while (nodes.Count > 0)
            {
                Node node = nodes.Dequeue();

                if (node == node2)
                {
                    ResetVisitedStatus();
                    return true;
                }

                node.Visited = true;

                foreach (Node neighbor in GetNeighbors(node))
                {
                    if (neighbor.Visited)
                        continue;

                    nodes.Enqueue(neighbor);
                }
            }

            ResetVisitedStatus();
            return false;
        }

        private void ExpandSize()
        {
            int expandedSize = _currentSize*2;

            // expand the nodes array
            Node[] expandedNodes = new Node[expandedSize];
            for (int i = 0; i < _currentSize; i++)
            {
                expandedNodes[i] = _nodes[i];
            }
            _nodes = expandedNodes;

            // expanded the adj matrix
            int[,] expandedMatrix = new int[expandedSize, expandedSize];

            for (int i = 0; i < expandedSize; i++)
            {
                for (int j = 0; j < expandedSize; j++)
                {
                    if (i < _currentSize && j < _currentSize)
                    {
                        expandedMatrix[i, j] = _matrix[i, j];
                    }
                    else // set the weight for remaining edges to -1
                    {
                        expandedMatrix[i, j] = -1;
                    }
                }
            }

            _currentSize = expandedSize;
            _matrix = expandedMatrix;
        }

        public void PrintAdjMatrix()
        {
            // print node data first
            foreach (var node in _nodes)
            {
                Console.Write("{0}, ", node == null ? -1 : node.Data);
            }
            Console.WriteLine();

            // print adj matrix
            for (int i = -1; i < _currentIndex; i++)
            {
                
                for (int j = -1; j < _currentIndex; j++)
                {
                    if (i == -1 && j == -1)
                    {
                        Console.Write("   ");
                        continue;
                    }

                    if (i == -1)
                    {
                        Console.Write("{0,3}", _nodes[j].Data);
                        if (j == _currentIndex - 1)
                            Console.WriteLine();
                        continue;
                    }

                    if (j == -1)
                    {
                        Console.Write("{0,3}", _nodes[i].Data);
                        continue;
                    }

                    Console.Write("{0, 3}", _matrix[i,j]);
                    if (j == _currentIndex - 1)
                        Console.WriteLine();
                }
            }
        }

        private void ResetVisitedStatus()
        {
            foreach (var node in _nodes)
            {
                if (node != null)
                    node.Visited = false;
            }
        }

        public void DeleteEdge(Node node1, Node node2)
        {
            int fromIndex = node1.Index;
            int toIndex = node2.Index;

            // \todo sanity check on the indexes
            _matrix[fromIndex, toIndex] = -1;
        }

        public void RemoveNode(Node node)
        {
            // \todo sanity check for node's existence

            // delete edges which have this node
            for (int i = node.Index; i < _currentSize -1; i++)
            {
                for (int j = 0; j < _currentSize; j++)
                {
                    _matrix[i, j] = _matrix[i+1, j];
                }
            }
            for (int i = 0; i < _currentSize; i++)
            {
                for (int j = node.Index; j < _currentSize -1; j++)
                {
                    _matrix[i, j] = _matrix[i, j + 1];
                }
            }

            // delete the node 
            _nodes[node.Index] = null;
            // shift the array left to fill the hole and reassign indexes
            for (int i = node.Index; i < _nodes.Length - 1; i++)
            {
                _nodes[i] = _nodes[i + 1];
                Node currentNode = _nodes[i];
                if (currentNode != null)
                {
                    currentNode.Index = i;
                }
            }

            _currentIndex--;
            _currentSize--;
        }
    }

    public class Node
    {
        public int Index;
        public int Data;
        public bool Visited;
    }
}
