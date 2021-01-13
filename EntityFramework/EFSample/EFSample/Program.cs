using EFSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var db = new BlogContext())
            {
                // Create
                //Console.WriteLine("Inserting a new blog");
                //db.Blogs.Add(new Blog { Name = "My Blog!", Url = "http://blog.vivekrathod.com/" });
                //db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    //.Include("Posts")
                    .OrderBy(b => b.Id)
                    .First();

                //All Posts
                //var posts = blog.Posts.Where(p => p.Name == "Hello World 2").ToList();
                var posts = db.Blogs
                    .Where(b=>b.Posts.Any(p=>p.Name == "Hello World 2"))
                    //.SelectMany(b => b.Posts)
                    ;
                foreach (var post in posts)
                {
                    Console.WriteLine($"Post: {post.Name}");
                }

                // Update
                //Console.WriteLine("Updating the blog and adding a post");
                //blog.Url = "https://blog.vivekrathod.com/";
                //blog.Posts.Add(
                //    new Post
                //    {
                //        Name = "Hello World",
                //        Content = "I wrote an app using EF Core!"
                //    });
                //db.SaveChanges();

                // Delete
                //Console.WriteLine("Delete the blog");
                //db.Blogs.Remove(blog);
                //db.SaveChanges();
            }
        }
    }
}
