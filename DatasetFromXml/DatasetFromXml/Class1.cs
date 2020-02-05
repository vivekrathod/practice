using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetFromXml
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            DataSet ds = new DataSet();
            ds.ReadXml("ADKnowledgebase.xml");
            ds.WriteXmlSchema(@"ADKnowledgebase.xsd");
        }
    }
}
