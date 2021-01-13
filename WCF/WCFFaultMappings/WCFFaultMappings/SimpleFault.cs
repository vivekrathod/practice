using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WCFFaultMappings
{
    [DataContract]
    public class SimpleFault
    {
        public SimpleFault() {}
        public SimpleFault(string message)
        {
            Message = message;
        }
        
        [DataMember]
        public string Message { get; set; }
    }
}
