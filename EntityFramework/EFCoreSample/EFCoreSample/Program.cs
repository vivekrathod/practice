using EFCoreSample.Models;
using System;
using System.Linq;

namespace EFCoreSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var db = new BlogContext())
            {
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new Blog { Name = "My Blog!", Url = "http://blog.vivekrathod.com/" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    .OrderBy(b => b.Id)
                    .First();
                
                // Update
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://blog.vivekrathod.com/";
                blog.Posts.Add(
                    new Post
                    {
                        Name = "Hello World",
                        Content = "I wrote an app using EF Core!"
                    });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(blog);
                db.SaveChanges();
            }
        }
    }
}
