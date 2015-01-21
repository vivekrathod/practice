using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WCFFaultMappings
{
    class ExceptionToFaultBehaviorAttribute : Attribute, IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                (channelDispatcher as ChannelDispatcher).ErrorHandlers.Add(new ExceptionToFaultMapper());
            }
        }
    }
}
