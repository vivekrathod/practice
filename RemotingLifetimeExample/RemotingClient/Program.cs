using RemotingLifetimeExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace RemotingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpChannel(), false);
            
            string url = "tcp://localhost:8081/MyServer";
            RemotingConfiguration.RegisterActivatedClientType(typeof(ClientActivatedType), url);

            //object[] urlArray = { new UrlAttribute(url) };
            //ClientActivatedType ca = (ClientActivatedType)Activator.CreateInstance(typeof(ClientActivatedType), null, urlArray);

            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                new ClientActivatedType();    
            }

            Console.WriteLine("Created {0} client-activated-remote objects", count);
            //Console.ReadKey();

            var ca = new ClientActivatedType();
            ca.GCCollect();

            Console.WriteLine("collected GC");
            Console.ReadKey();

        }
    }
}
