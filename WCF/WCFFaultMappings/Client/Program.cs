using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using WCFFaultMappings;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ISimpleService> factory = new ChannelFactory<ISimpleService>(new NetNamedPipeBinding(),
                                                                                        "net.pipe://localhost/SimpleService");
            try
            {
                ISimpleService client = factory.CreateChannel();
                Console.WriteLine(client.Add(2, 3));
            }
            catch (FaultException<SimpleFault> ex)
            {
                Console.WriteLine(ex);
                factory.Abort();
            }
            catch (FaultException ex)
            {
                Console.WriteLine(ex);
                factory.Abort();
            }
            factory.Close();
        }
    }
}
