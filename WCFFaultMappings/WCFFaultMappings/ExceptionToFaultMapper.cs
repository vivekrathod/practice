using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace WCFFaultMappings
{
    class ExceptionToFaultMapper : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            FaultException<SimpleFault> simpleFaultException =
                new FaultException<SimpleFault>(new SimpleFault() {Message = error.Message});
            MessageFault mf = simpleFaultException.CreateMessageFault();
            fault = Message.CreateMessage(version, mf, simpleFaultException.Action);
        }

        public bool HandleError(Exception error)
        {
            return true;
        }
    }
}
