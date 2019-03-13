using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_To_Many_ExtraInfo
{
    public class Person
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Address> Addresses { get; set; }
    }
}
