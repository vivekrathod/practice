using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace MyReplyChannel
{
    public class Listener
    {
        public static string ListenUri = "http://localhost:20025/SampleService";
        public static Binding ListenBinding = new WSHttpBinding(SecurityMode.None);

        private static int _requestCount;
        private static int _responseCount;
        private const int ResponseDelay = 15000; // in millisecondss
        static void Main(string[] args)
        {
            IChannelListener<IReplyChannel> listener = ListenBinding.BuildChannelListener<IReplyChannel>(new Uri(ListenUri));
            listener.Open();
            
            AsynchroRecv(listener);
            //SynchroRecv(listener);
            
            Console.ReadKey();
        }

        static void SynchroRecv(IChannelListener<IReplyChannel> listener)
        {
            while (true)
            {
                IReplyChannel channel = listener.AcceptChannel();
                channel.Open();
                //Console.WriteLine("Accepted channel at {0}", DateTime.Now);
                ThreadPool.QueueUserWorkItem(e =>
                {
                    while (true)
                    {
                        RequestContext rc = ((IReplyChannel)e).ReceiveRequest();
                        Console.WriteLine("{0} {1} Accepted request", DateTime.Now.TimeOfDay, ++_requestCount);

                        // sending a delayed response
                        ThreadPool.QueueUserWorkItem(
                            arg =>
                                {
                                    Thread.Sleep(ResponseDelay);
                                    Console.WriteLine("{0} {1} Sending response", DateTime.Now.TimeOfDay, Interlocked.Increment(ref _responseCount));
                                    rc.Reply(rc.RequestMessage);
                                });
                    }
                }, channel);
            }
        }

        static void AsynchroRecv(IChannelListener<IReplyChannel> listener)
        {
            while (true)
            {
                IReplyChannel channel = listener.AcceptChannel();
                channel.Open();
                //Console.WriteLine("Accepted channel at {0}", DateTime.Now);
                channel.BeginReceiveRequest(ProcessRequest, channel);
            }
        }

        static void ProcessRequest(IAsyncResult ar)
        {
            IReplyChannel channel = ar.AsyncState as IReplyChannel;
            RequestContext rc = channel.EndReceiveRequest(ar);
            Console.WriteLine("{0} {1} Accepted request", DateTime.Now.TimeOfDay, Interlocked.Increment(ref _requestCount));

            // accept more requests
            channel.BeginReceiveRequest(ProcessRequest, channel);

            // sending a delayed response
            ThreadPool.QueueUserWorkItem(e =>
            {
                Thread.Sleep(ResponseDelay);
                Console.WriteLine("{0} {1} Sending response", DateTime.Now.TimeOfDay, Interlocked.Increment(ref _responseCount));
                rc.Reply(rc.RequestMessage);
            });
        }
    }
}
