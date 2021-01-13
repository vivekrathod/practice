using System;
using System.Collections.Generic;
using System.Text;

namespace EFSample.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Post> Posts { get; } = new List<Post>();
    }
}
