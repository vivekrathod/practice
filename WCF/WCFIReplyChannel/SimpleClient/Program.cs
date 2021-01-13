using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using MyReplyChannel;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int sessions = 1;
            int calls = 4;
            for (int i = 0; i < sessions; i++)
            {
                ChannelFactory<IRequestChannel> client =
                    new ChannelFactory<IRequestChannel>(Listener.ListenBinding, Listener.ListenUri);
                IRequestChannel channel = client.CreateChannel();
                channel.Open();

                for (int j = 0; j < calls/sessions; j++)
                {
                    ThreadPool.QueueUserWorkItem(
                        e =>
                            {
                                try
                                {
                                    Message response = channel.Request(Message.CreateMessage(MessageVersion.Default, "someaction"));
                                    Console.WriteLine("{0} Received response ", DateTime.Now.TimeOfDay);
                                }
                                catch (TimeoutException ex)
                                {
                                    Console.WriteLine("{0} Timeout {1} ", DateTime.Now.TimeOfDay, ex.Message);
                                }

                            }
                        );
                }
            }

            Console.ReadKey();

        }
    }
}
