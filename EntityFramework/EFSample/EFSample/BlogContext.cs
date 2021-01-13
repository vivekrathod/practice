using EFSample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace EFSample
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BlogContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
