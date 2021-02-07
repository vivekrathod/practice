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
                InitializeData(db, 2, 3);

                // log EF SQL to Console
                db.Database.Log = Console.Write;

                // Read
                Console.WriteLine("Querying for a blog");
                //var blog = db.Blogs
                //    //.Include("Posts") // because Lazy loading is turned ON
                //    .OrderBy(b => b.Id)
                //    .First();

                // example 1
                var posts = db.Blogs.Where(blog => blog.Posts.Where(post => post.Name.ToLower().Contains("hello")).Any()).ToList();

                // example 2
                var blogs2 = db.Blogs
                    .SelectMany(b => b.Posts.Where(p => p.Name.ToLower().Contains("hello")))
                    //.Select(p => new { Blog = p.Blog, Post = p })
                    .Select(a => a.Blog)
                    .ToList();
                
                // example 3
                var blogs3 = db.Blogs
                    .SelectMany(b => b.Posts.Where(p => p.Name.ToLower().Contains("hello")))
                    .Select(p => new { Blog = p.Blog, Post = p })
                    .ToList()
                    .Select(a => a.Blog);

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

                ClearData(db);
            }
        }

        static void InitializeData(BlogContext db, int noOfBlogs, int noOfPosts)
        {
            // Create
            for (int b = 0; b < noOfBlogs; b++)
            {
                Console.WriteLine($"Inserting a blog {b}");
                var blog = new Blog { Name = $"My Blog {b}!", Url = $"http://blog{b}.vivekrathod.com/" };
                for (int p = 0; p < noOfPosts; p++)
                {
                    var post = new Post
                    {
                        Name = $"Hello World {p}",
                        Content = $"I wrote an app using EF Core {p}!"
                    };
                    blog.Posts.Add(post);
                }
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }

        static void ClearData(BlogContext db)
        {
            // Delete
            Console.WriteLine("Delete all blogs and posts");
            db.Database.ExecuteSqlCommand("delete from posts");
            db.Database.ExecuteSqlCommand("delete from blogs");
            db.SaveChanges();
        }
    }
}
