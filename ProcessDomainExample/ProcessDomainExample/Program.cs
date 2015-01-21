using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AppSecInc.Collections;
using AppSecInc.ProcessDomain;
using test;

namespace ProcessDomainExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessDomainSetup setup = new ProcessDomainSetup
                                               {
                                                   Platform = PlatformTarget.x86,
                                                   TypeFilterLevel =
                                                       System.Runtime.Serialization.Formatters.TypeFilterLevel.Full
                                               };
                ProcessDomain domain = ProcessDomain.CreateDomain("proc domain", setup);

                // test serilizatoin etc across proc domain


                //DerivedSimpleClass derivedSimpleClass =
                //    (DerivedSimpleClass)
                //    domain.CreateInstanceAndUnwrap(typeof(DerivedSimpleClass).Assembly.FullName,typeof(DerivedSimpleClass).FullName);
                //, true,BindingFlags.Default, null, new object[] {new SimpleArgumentClass()},null, null, null);
                //derivedSimpleClass.Name = "";

                Type simpleClassWrapperType = typeof(SimpleClassWrapper);
                Assembly simpleClassWrapperAssembly = simpleClassWrapperType.Assembly;
                SimpleClassWrapper simpleClassWrapper =
                    (SimpleClassWrapper)
                    domain.CreateInstanceAndUnwrap(simpleClassWrapperAssembly.FullName, simpleClassWrapperType.FullName);
                //simpleClassWrapper.MethodWithArgument(new SimpleArgumentClass());
                //simpleClassWrapper.SimpleArgumentClassProperty = new SimpleArgumentClass();
                //SimpleArgumentClass sampleArgumentClass =  simpleClassWrapper.SimpleArgumentClassProperty;
                //simpleClassWrapper.MethodReturnsArgument();
                //SimpleClass simpleClass = simpleClassWrapper.GetSimpleClass();
                //SimpleArgumentClass sampleArgumentClass = new SimpleArgumentClass() { SampleInterfaceField = new SampleImplementation() };
                //simpleClassWrapper.TestArgument(sampleArgumentClass);
                //simpleClassWrapper.SampleInterfaceField = new SampleImplementation();
                //simpleClassWrapper.TestInterface();
                //ISampleInterface sampleInterface = simpleClassWrapper.SampleInterfaceField;
                //simpleClassWrapper.SimpleClassProperty = new SimpleClass();

                //Type type = typeof(LRUCache<,>).MakeGenericType(new[] { typeof(int), typeof(string) });
                //LRUCache<int, string> cache = (LRUCache<int, string>)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

                // testing exceptions across proc domain
                //Type refType = typeof(test.refclass);
                //Assembly refAssembly = refType.Assembly;
                //refclass refclass1 =
                //    (refclass)domain.CreateInstanceAndUnwrap(refAssembly.FullName, refType.FullName);

                //refclass1.invokeStdException();
                //refclass1.invokeAccessViolation();

                // overflows
                //refclass1.invokeBufferOverflow();
                //refclass1.invokeStackOverflow(10);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("does not affect me");

        }
    }
}
