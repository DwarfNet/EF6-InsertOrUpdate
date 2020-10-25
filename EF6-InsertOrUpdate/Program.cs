using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6_InsertOrUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EntityContext())
            {
                List<Blog> blogs;
                CleanCurrentDb(db);

                var name = "BlogTest1";
                Console.WriteLine($"Create Blog {name}.");
                var originalBlog = new Blog { Name = name };

                Console.WriteLine($"Add 'Original' Post into the blog.");
                originalBlog.Posts = new List<Post>()
                {
                    new Post() {
                        Title = "My Original Title",
                        Content = "My original content."
                        }
                };

                Console.WriteLine($"Add Blog {originalBlog.Name} into db.");
                db.Blogs.Add(originalBlog);
                
                Console.WriteLine($"Save Blog {originalBlog.Name} into db.");
                db.SaveChanges();

                Console.WriteLine("All blogs in the database:");
                blogs = db.Blogs.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"Blog ID: {blog.BlogId}.\tBlog NAME: {blog.Name}.");
                }
                
                Console.WriteLine($"Get Blog where Name is {name}.");
                var oldBlog = db.Blogs.Where(b => b.Name == name).FirstOrDefault();
                Console.WriteLine($"Blog ID is : {oldBlog.BlogId}.");
                Console.WriteLine($"Change Name for {oldBlog.Name}.");
                oldBlog.Name = "The new Blog";
                
                Console.WriteLine($"Insert or update the original blog version.");
                db.InsertOrUpdate(originalBlog);
                Console.WriteLine($"Insert or update the new blog version.");
                db.InsertOrUpdate(oldBlog);

                Console.WriteLine($"Save Blog {oldBlog.Name} into db.");
                db.SaveChanges();

                Console.WriteLine("All blogs in the database:");
                blogs = db.Blogs.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"Blog ID: {blog.BlogId}.\tBlog NAME: {blog.Name}.");
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void CleanCurrentDb(EntityContext db)
        {
            foreach (var blog in db.Blogs)
            {
                db.Blogs.Remove(blog);
            }
        }
    }
}
