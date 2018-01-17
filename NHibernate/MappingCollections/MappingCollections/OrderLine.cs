using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingCollections
{
    public class OrderLine
    {
        public virtual int Id { get; set; }
        public virtual int Amount { get; set; }
        public virtual string ProductName { get; set; }

        public virtual Order Order { get; set; }

        public virtual string MyName()
        {
            return "MyName is OrderLine";
        }
    }

    public class SpecialOrderLine : OrderLine
    {
        public virtual string Special { get; set; }

        public override string MyName()
        {
            return "MyName is SpecialOrderLine";
        }
    }
}
