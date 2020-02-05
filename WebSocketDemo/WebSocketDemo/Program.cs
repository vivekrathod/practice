using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebSocketDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocket socket1 = new WebSocket("ws://localhost:10000/WebScanEngine/scan-driver");
            
            var finished = new ManualResetEventSlim(false);
            int numFindings = 0;
            socket1.OnOpen += (sender, eventArgs) =>
            {
                Console.WriteLine("Socket opened...");
                socket1.Send("{ \"commandId\": \"command-id-1\", \"command\": \"CONNECT\", \"clientId\": \"clientId-1\" }");
                Console.WriteLine("Sending CONNECT command");
            };

            socket1.OnMessage += (sender, eventArgs) =>
            {
                // \todo parse json
                if (eventArgs.Data.Contains("WSE_CONNECT_RESPONSE") && eventArgs.Data.Contains("SUCCESS"))
                {
                    Console.WriteLine("Successfully connected...");
                    socket1.Send("{\"commandId\":\"command-id-1-1\",\"command\":\"RUN\",\"fragmentId\":\"fragment-id-1\",\"scanDefinition\":{\"url\":\"http://demo.testfire.net\",\"template\":\"testScanTemplate\",\"blackListUrls\":[],\"whiteListUrls\":[],\"authentication\":{\"username\":\"\",\"password\":\"\"},\"assetReferenceId\":\"\",\"boundaryOption\":\"HOST_AND_PORT\"}}\r\n");
                    Console.WriteLine("Sent start the scan command...");
                }
                else if (eventArgs.Data.Contains("WSE_RUN_RESPONSE") && eventArgs.Data.Contains("SUCCESS"))
                {
                    Console.WriteLine("Successfully started the scan...");
                }
                else if (eventArgs.Data.Contains("WSE_FINDING") || eventArgs.Data.Contains("WSE_PROGRESS_MESSAGE"))
                {
                    if (numFindings == 15)
                    {
                        Console.WriteLine("Sending abort command");
                        socket1.Send("{\"commandId\":\"command-id-1-2\",\"command\":\"ABORT\",\"fragmentId\":\"fragment-id-1\"}");
                    }

                    numFindings++;
                    Console.WriteLine($"Scan finding: {eventArgs.Data}");
                }
                else if (eventArgs.Data.Contains("WSE_SCAN_END") || eventArgs.Data.Contains("WSE_ABORT_RESPONSE"))
                {
                    Console.WriteLine("Scan ended or aborted...");
                    finished.Set(); 
                }
            };

            socket1.Connect();
            finished.Wait();
        }
    }
}
