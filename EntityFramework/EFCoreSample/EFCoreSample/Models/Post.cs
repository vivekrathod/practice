using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSample.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
