using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemotingLifetimeExample
{
    public class ClientActivatedType : MarshalByRefObject
    {
        int[] _large;
        int _length = 10000;

        public ClientActivatedType()
        {
            _large = new int[_length];

            for (int i = 0; i < _length; i++)
            {
                _large[i] = 0;
            }
        }

        public void Hello()
        {
            Console.WriteLine("Hello from the server");
        }

        public override object InitializeLifetimeService()
        {
            //return base.InitializeLifetimeService();
            return null;
        }

        public void GCCollect()
        {
            Console.WriteLine("GC'ing.");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
