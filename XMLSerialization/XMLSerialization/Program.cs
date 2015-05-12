using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream stream = File.Create("serialized.txt"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Class1));

                Class1 class1 = new Class1 {Prop1 = 1, Prop2 = "abc"};
                serializer.Serialize(stream, class1);

                Class1Child child1 = new Class1Child {Prop1 = 1, Prop2 = "abc", Prop1Child = "child"};
                serializer.Serialize(stream, child1);
            }
            
        }
    }
}
