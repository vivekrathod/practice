using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_To_Many_ExtraInfo
{
    public class Address
    {
        public virtual int Id { get; set; }

        public virtual string AddressString { get; set; }

        //public virtual IList<Person> Persons { get; set; }

        public virtual int PersonId { get; set; }

        public virtual bool IsDefault { get; set; }
    }
}
