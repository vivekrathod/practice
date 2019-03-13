using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTransactions.Model
{
    public class TestEntity1
    {
        public virtual Guid Id { get; set; }
        public virtual int TestProp1 { get; set; }
        public virtual string TestProp2 { get; set; }
    }
}
