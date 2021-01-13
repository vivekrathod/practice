using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace WCFFaultMappings
{
    public class SEHOperationInvoker : IOperationInvoker
    {
        private IOperationInvoker _originalInvoker;

        public SEHOperationInvoker(IOperationInvoker originalInvoker)
        {
            _originalInvoker = originalInvoker;
        }

        public object[] AllocateInputs()
        {
            return _originalInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            try
            {
                return _originalInvoker.Invoke(instance, inputs, out outputs);
            }
            catch(SEHException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}
