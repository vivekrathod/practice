using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;

namespace RemotingLifetimeExample
{
    public class Server
    {
        public static void Main()
        {
            LifetimeServices.LeaseTime = TimeSpan.FromSeconds(5);
            LifetimeServices.LeaseManagerPollTime = TimeSpan.FromSeconds(3);
            LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(2);
            LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(1);

            ChannelServices.RegisterChannel(new TcpChannel(8081), false);
            RemotingConfiguration.ApplicationName = "MyServer";
            RemotingConfiguration.RegisterActivatedServiceType(typeof(ClientActivatedType));

            Console.WriteLine("The server is listening. Press Enter to exit....");
            Console.ReadLine();
        }
    }
}
