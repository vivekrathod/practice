using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphImplAdjList
{
    class Program
    {
        static void Main(string[] args)
        {
            // visual representation of the graph in these tests
            //  
            //  
            //  |---->2---->4    5--->6 
            //  |     |
            //  1<----|---->3
            Graph graph = new Graph();
            Node node1 = new Node(1);
            Node node2 = new Node(2);
            Node node3 = new Node(3);
            Node node4 = new Node(4);
            Node node5 = new Node(5);
            Node node6 = new Node(6);

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);
            graph.AddNode(node5);
            graph.AddNode(node6);

            graph.AddEdge(node1, node2);
            // add a circular ref
            graph.AddEdge(node2, node1);
            graph.AddEdge(node2, node3);
            graph.AddEdge(node2, node4);
            graph.AddEdge(node5, node6);

            

            Console.WriteLine();
            Console.WriteLine("Neighbors for {0}: ", node3.Data);
            foreach (Node neighbor in graph.GetNeighbors(node3))
            {
                Console.Write("{0} ,", neighbor.Data);
            }
            Console.WriteLine();
            Console.WriteLine("Neighbors for {0}: ", node2.Data);
            foreach (Node neighbor in graph.GetNeighbors(node2))
            {
                Console.Write("{0} ,", neighbor.Data);
            }
            Console.WriteLine();
            Console.WriteLine("Neighbors for {0}: ", node5.Data);
            foreach (Node neighbor in graph.GetNeighbors(node5))
            {
                Console.Write("{0} ,", neighbor.Data);
            }

            Console.WriteLine();
            Console.WriteLine("Adj list for the graph");
            graph.PrintAdjList();

            Console.WriteLine();
            Console.WriteLine("Removing node {0}: ", node3.Data);
            graph.RemoveNode(node3);

            Console.WriteLine();
            Console.WriteLine("Adj list for the graph");
            graph.PrintAdjList();

            Console.WriteLine();
            Console.WriteLine("Removing node {0}: ", node5.Data);
            graph.RemoveNode(node5);

            Console.WriteLine();
            Console.WriteLine("Adj list for the graph");
            graph.PrintAdjList();


            Console.WriteLine();
            Console.WriteLine("Adding node {0}: ", node3.Data);
            graph.AddNode(node3);
            graph.AddEdge(node2, node3);

            Console.WriteLine();
            Console.WriteLine("Adj list for the graph");
            graph.PrintAdjList();


            Console.WriteLine();
            Console.WriteLine("Adding node {0}: ", node5.Data);
            graph.AddNode(node5);
            graph.AddEdge(node5, node6);

            Console.WriteLine();
            Console.WriteLine("Adj list for the graph");
            graph.PrintAdjList();
        }
    }
}
