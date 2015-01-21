using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Runtime.InteropServices;

namespace WCFFaultMappings
{
    [ExceptionToFaultBehavior]
    class SimpleService : ISimpleService
    {
        [SEHOperationBehavior]
        public int Add(int num1, int num2)
        {
            //SimpleFault sf = new SimpleFault("simple fault exception");
            //throw new FaultException<SimpleFault>(sf);

            //throw new ArgumentException("argument exception..");
            throw new System.Runtime.InteropServices.SEHException();

            //SimpleCLR.SimpleCLRClass clrClass = new SimpleCLR.SimpleCLRClass();
            //try
            //{
            //    clrClass.ThrowAccessViolation();
            //    clrClass.ThrowSEHException();
            //}
            //catch (SEHException ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //clrClass.ThrowDivideByZero();

            
            return num1 + num2;
        }
    }
}
