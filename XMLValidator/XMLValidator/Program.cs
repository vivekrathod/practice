using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XMLValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string xsdMarkup =
                @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                   <xsd:element name='Root'>
                    <xsd:complexType>
                     <xsd:choice maxOccurs='unbounded'>
                      <xsd:element name='Child1' minOccurs='0' maxOccurs='unbounded'/>
                      <xsd:element name='Child2' minOccurs='0' maxOccurs='unbounded'/>
                     </xsd:choice>
                    </xsd:complexType>
                   </xsd:element>
                  </xsd:schema>";

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

            XDocument doc1 = new XDocument(
                new XElement("Root",
                    new XElement("Child1", "content1"),
                    new XElement("Child2", "content1")
                )
            );

            Console.WriteLine("Validating doc1");
            bool errors = false;
            doc1.Validate(schemas, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });
            Console.WriteLine("doc1 {0}", errors ? "did not validate" : "validated");
        }
    }
}
