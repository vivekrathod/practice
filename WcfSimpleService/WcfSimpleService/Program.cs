using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WcfSimpleService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(SimpleService));
            host.AddServiceEndpoint(typeof(ISimpleService), new WSHttpBinding(SecurityMode.None), "http://vrathod9:6543/SimpleService");
            host.Description.Behaviors.Add(new ServiceMetadataBehavior());
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "http://vrathod9:6543/SimpleService/mex");
            host.Open();
            Console.ReadKey();
        }
    }
}
