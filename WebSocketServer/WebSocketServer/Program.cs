using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServer
{
    public class Laputa : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data == "BALUS"
                ? "I've been balused already..."
                : "I'm not available now.";

            Send(msg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketSharp.Server.WebSocketServer("ws://127.0.0.1");
            wssv.AddWebSocketService<Laputa>("/Laputa");
            wssv.Start();
            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}
