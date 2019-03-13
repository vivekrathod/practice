using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_To_Many
{
    public class Work
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Author> Authors { get; set; }

        public virtual void AddAuthor(Author author)
        {
            Authors.Add(author);
            author.Works.Add(this);
        }
    }
}
