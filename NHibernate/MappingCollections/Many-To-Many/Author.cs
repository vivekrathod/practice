using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_To_Many
{
    public class Author
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Work> Works { get; set; }

        public virtual void AddWork(Work work)
        {
            Works.Add(work);
            work.Authors.Add(this);
        }
    }
}
