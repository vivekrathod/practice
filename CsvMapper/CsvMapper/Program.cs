using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //CsvConfiguration csvConfiguration = new CsvConfiguration();
            //csvConfiguration.RegisterClassMap<PersistToCsvMap>();
            //csvConfiguration.RegisterClassMap<ParentMap>();
            //csvConfiguration.RegisterClassMap<ChildMap>();

            PersistToCsv persistToCsv = new PersistToCsv {Parent = new Child {Name = "John", PetName = "Papa"}};
            //Child child1 = new Child {Name = "John", PetName = "Papa"};

            using (FileStream fileStream = File.Open("family.csv", FileMode.OpenOrCreate))
            using (TextWriter textWriter = new StreamWriter(fileStream))
            using (CsvWriter csvWriter = new CsvWriter(textWriter))
            //using (CsvWriter csvWriter = new CsvWriter(textWriter, csvConfiguration))
            {
                csvWriter.WriteRecord(persistToCsv);
                //csvWriter.WriteRecord(child1);
            }

        }
    }

    //public abstract class Parent
    public class Parent
    {
        public string Name { get; set; }
    }

    public class Child : Parent
    {
        public string PetName { get; set; }
    }

    public class PersistToCsv
    {
        public Parent Parent { get; set; }
    }

    public class PersistToCsvMap : CsvClassMap<PersistToCsv>
    {
        public PersistToCsvMap()
        {
            References<ParentMap>(e => e.Parent);
        }
    }

    public class ParentMap<T> : CsvClassMap<T> where T : Parent
    {
        public ParentMap()
        {
            Map(e => e.Name);

        }
    }

    public class ParentMap : CsvClassMap<Parent>
    {
        public ParentMap()
        {
            Map(e => e.Name);
        }
    }

    public class ChildMap : CsvClassMap<Child>
    {
        public ChildMap()
        {
            Map(e => e.Name);
            Map(e => e.PetName);
        }
    }


}
