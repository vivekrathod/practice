using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphImplAdjMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            // visual representation of the graph in these tests
            //  
            //  
            //  |---->2--->4    5--->6 
            //  |     |
            //  1<----|---->3
            Graph graph = new Graph();
            Node node1 = new Node {Data = 1};
            Node node2 = new Node { Data = 2 };
            Node node3 = new Node { Data = 3 };
            Node node4 = new Node { Data = 4 };
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);

            graph.AddEdge(node1, node2);
            graph.AddEdge(node2, node3);
            graph.AddEdge(node2, node4);

            // add a circular ref
            graph.AddEdge(node2, node1);

            Console.WriteLine("Adj Matrix");
            graph.PrintAdjMatrix();

            Console.WriteLine();
            Console.WriteLine("Neighbors for {0}: ", node3.Data);
            foreach (Node neighbor in graph.GetNeighbors(node3))
            {
                Console.Write("{0} ,", neighbor.Data);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("(Using DFS)Is node {0} connected to {1}: {2}", node1.Data, node3.Data,
                graph.AreConnectedDFS(node1, node3));
            Console.WriteLine("(Using BFS)Is node {0} connected to {1}: {2}", node1.Data, node3.Data,
                graph.AreConnectedBFS(node1, node3));

            Console.WriteLine("(Using DFS)Is node {0} connected to {1}: {2}", node3.Data, node4.Data,
                graph.AreConnectedDFS(node3, node4));
            Console.WriteLine("(Using BFS)Is node {0} connected to {1}: {2}", node3.Data, node4.Data,
                graph.AreConnectedBFS(node3, node4));



            Node node5 = new Node { Data = 5 };
            Node node6 = new Node { Data = 6 };
            graph.AddNode(node5);
            graph.AddNode(node6);
            graph.AddEdge(node5, node6);
            Console.WriteLine("(Using DFS) Is node {0} connected to {1}: {2}", node5.Data, node6.Data,
                graph.AreConnectedDFS(node5, node6));
            Console.WriteLine("(Using BFS) Is node {0} connected to {1}: {2}", node5.Data, node6.Data,
                graph.AreConnectedBFS(node5, node6));

            Console.WriteLine("(Using DFS) Is node {0} connected to {1}: {2}", node1.Data, node6.Data,
                graph.AreConnectedDFS(node1, node6));
            Console.WriteLine("(Using BFS) Is node {0} connected to {1}: {2}", node1.Data, node6.Data,
                graph.AreConnectedBFS(node1, node6));


            Console.WriteLine();
            graph.RemoveNode(node3);
            Console.WriteLine("Adj matrix after removing node {0}", node3.Data);
            graph.PrintAdjMatrix();

            Console.WriteLine();
            graph.RemoveNode(node1);
            Console.WriteLine("Adj matrix after removing node {0}", node1.Data);
            graph.PrintAdjMatrix();

        }
    }
}
