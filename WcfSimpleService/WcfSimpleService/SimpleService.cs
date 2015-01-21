using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WcfSimpleService
{
    class SimpleService : ISimpleService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
