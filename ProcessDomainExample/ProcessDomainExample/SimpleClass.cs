using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using log4net;

namespace ProcessDomainExample
{
    //[Serializable]
    //[DataContract]
    public class SimpleClass
    {
        public SimpleClass()
        {
            SimpleArgumentClassProperty = new SimpleArgumentClass() {Name = "asasa"};
        }

        public void SimpleMethod()
        {
            System.Diagnostics.Debugger.Break();
        }

        //[DataMember]
        public string Name { get; set; }

        public SimpleArgumentClass SimpleArgumentClassProperty { get; set; }
    }

    [Serializable]
    public class DerivedSimpleClass : SimpleClass
    {
        
    }

    //[Serializable]
    public class SimpleClassWrapper : MarshalByRefObject
    {
        //public SimpleClass SimpleClassProperty { get; set; }

        //public void MethodWithArgument(SimpleArgumentClass simpleArgument)
        //{}

        //public SimpleArgumentClass MethodReturnsArgument()
        //{
        //    return new SimpleArgumentClass();
        //}

        private SimpleArgumentClass _simpleArgumentClass = new SimpleArgumentClass(); 
        //public SimpleArgumentClass SimpleArgumentClassProperty { get; set; }

        //public SimpleClass GetSimpleClass()
        //{
        //    return new SimpleClass();
        //}

        //public void TestArgument(SimpleArgumentClass arg)
        //{
        //    arg.TestNestedArg(new NestedArgument());
        //}

        //public NestedArgument NestedArgumentProperty { get; set; }

        //public ISampleInterface SampleInterfaceField { get; set; }
        //public void TestInterface()
        //{
        //    SampleInterfaceField = new SampleImplementation();
        //}

    }


    //[Serializable]
    public class SimpleArgumentClass // : MarshalByRefObject
    {
        public string Name { get; set; }
        public SimpleArgumentClass()
        {
            //SampleInterfaceField = new SampleImplementation();  
        }

        public void TestNestedArg(NestedArgument arg1)
        {
            //arg1.Name = "asa";
            //SampleInterfaceField = new SampleImplementation();  
        }

        public NestedArgument NestedArgumentProperty { get { return _nestedArgument; } }
        
        static private NestedArgument _nestedArgument = new NestedArgument(){Name = "blah"};
        //private ILog _logger = LogManager.GetLogger(typeof (SimpleArgumentClass));

        //public ISampleInterface SampleInterfaceField {get;set;}
    }

    //[Serializable]
    public class NestedArgument
    {
        public string Name { get; set; }
    }

    public interface ISampleInterface
    {
        string Name { get; set; }
    }

    //[Serializable]
    public class SampleImplementation : ISampleInterface
    {
        public string Name { get; set; }
    }
}
