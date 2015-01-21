using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WcfSimpleService
{

    [ServiceContract]
    interface ISimpleService
    {
        [OperationContract]
        int Add(int a, int b);
    }
}
