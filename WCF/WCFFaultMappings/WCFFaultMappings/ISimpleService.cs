using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace WCFFaultMappings
{
    [ServiceContract]
    public interface ISimpleService
    {
        [OperationContract]
        [FaultContract(typeof(SimpleFault))]
        int Add(int num1, int num2);
    }
}
