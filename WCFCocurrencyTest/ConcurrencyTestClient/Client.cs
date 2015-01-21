using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using Service1;
using Service1Host;

namespace ConcurrencyTestClient
{
    class Client
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: ConcurrencyTestClient <no of calls> <no of sessions> \nFor example, to execute 100 calls over 20 sessions use \nUserRightsClient 100 20");
                return;
            }

            string address = "http://localhost:6123/svc/SampleService/1.0";
            TimeSpan timeout = TimeSpan.FromMinutes(10);
            Binding binding = new WSHttpBinding(SecurityMode.None)
                {
                    ReceiveTimeout = timeout,
                    SendTimeout = timeout,
                    OpenTimeout = timeout,
                    CloseTimeout = timeout
                };

            TestConcurrency<ISampleService>(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), address, binding);
        }

        static void TestConcurrency<T>(int calls, int sessions, string address, Binding binding) where T : ISampleService  
        {
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);

            int callsRunning = 0;
            int callsCompleted = 0;

            //reset counts on the service
            ISampleService client = factory.CreateChannel();
            client.ResetTimedOperationsCount();
            ((ICommunicationObject)client).Close();

            for (int j = 0; j < sessions; j++)
            {
                ThreadPool.QueueUserWorkItem(arg1 =>
                {
                    client = factory.CreateChannel();
                    for (int i = 0; i < calls / sessions; i++)
                    {
                        ThreadPool.QueueUserWorkItem(arg2 =>
                        {
                            try
                            {
                                Console.WriteLine("time: {0} call no: {1}", DateTime.Now.TimeOfDay, Interlocked.Increment(ref callsRunning));
                                ((ISampleService)arg2).TimedOperation(20);
                            }
                            catch (TimeoutException ex)
                            {
                                Console.WriteLine("time: {0} timeout: {1}", DateTime.Now.TimeOfDay, ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("time: {0} exception: {1}", DateTime.Now.TimeOfDay, ex.Message);
                            }
                            finally
                            {
                                Interlocked.Increment(ref callsCompleted);
                            }
                        }, client);
                    }
                });
            }
            while (callsCompleted < calls)
            {
                Thread.Sleep(1000);
            }
            Console.WriteLine("finished all calls");
        }
    }
}
