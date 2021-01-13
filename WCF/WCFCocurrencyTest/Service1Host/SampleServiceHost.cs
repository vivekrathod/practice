using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using Service1;

namespace Service1Host
{
    public class SampleServiceHost
    {
        static readonly string[] Addresses = {
                            "http://localhost:20025/SampleService",
                            "net.tcp://localhost:20025/SampleService",
                            "net.pipe://localhost/SampleService" };
        static readonly Binding[] Bindings = { new WSHttpBinding(SecurityMode.None), new NetTcpBinding(), new NetNamedPipeBinding() };

        public static string Address = Addresses[0];
        public static Binding Binding = Bindings[0];

        /// <summary>
        /// usage: Service1Host.exe [concurrent calls] [concurent sessions] [concurrent instances]
        /// e.g. to have 2 instances, 5 sessions and 25 calls invoke as: Service1Host.exe 25 5 2
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            
            ServiceHost host = new ServiceHost(typeof(SampleService));
            host.AddServiceEndpoint(typeof(ISampleService), Binding, Address);
            host.Description.Behaviors.Add(new ServiceThrottlingBehavior
            {
                MaxConcurrentCalls = Convert.ToInt32(args[0]),
                MaxConcurrentSessions = Convert.ToInt32(args[1]),
                MaxConcurrentInstances = Convert.ToInt32(args[2])
            });
            host.Open();
            Console.ReadKey();
        }
    }
}
