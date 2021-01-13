using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;
using System.ServiceModel;

namespace WCFFaultMappings
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof (SimpleService));
            host.AddServiceEndpoint(typeof (ISimpleService), new NetNamedPipeBinding(),
                                    "net.pipe://localhost/SimpleService");
            ServiceMetadataBehavior mdb = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(mdb);
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                                    MetadataExchangeBindings.CreateMexNamedPipeBinding(), "net.pipe://localhost/SimpleService/mex");
            host.Open();
            Console.ReadKey();
            host.Close();
        }
    }
}
