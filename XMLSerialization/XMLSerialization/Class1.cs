using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSerialization
{
    [XmlInclude(typeof(Class1Child))]
    public class Class1
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
    }
}
