using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingCollections
{
    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }

        public virtual int Id { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual IList<OrderLine> OrderLines { get; set; }
    }

    public class SpecialOrder : Order
    {
        public virtual string Special { get; set; }
    }
}
