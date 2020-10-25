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

                var name = "Blog Init Name";
                Console.WriteLine($"Create Blog {name}.");
                var originalBlog = new Blog { Name = name };

                Console.WriteLine($"Add Blog {originalBlog.Name} into db.");
                db.Blogs.Add(originalBlog);

                Console.WriteLine($"Save Blog {originalBlog.Name} into db.");
                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("All blogs in the database:");
                blogs = db.Blogs.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"Blog ID: {blog.BlogId}.\tBlog NAME: {blog.Name}.");
                }
                Console.WriteLine();

                Console.WriteLine($"Get Blog where Name is {name}.");
                var oldBlog = db.Blogs.Where(b => b.Name == name).FirstOrDefault();
                Console.WriteLine($"Blog ID is : {oldBlog.BlogId}.");

                oldBlog.Name = "The new Blog";
                Console.WriteLine($"Change Name for {oldBlog.Name}.");

                Console.WriteLine($"Insert or update the new blog version.");
                db.InsertOrUpdate(oldBlog);

                Console.WriteLine($"Save Blog {oldBlog.Name} into db.");
                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("All blogs in the database:");
                blogs = db.Blogs.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"Blog ID: {blog.BlogId}.\tBlog NAME: {blog.Name}.");
                }
                Console.WriteLine();

                var firstOfTheRange = db.Blogs.Where(b => b.BlogId == originalBlog.BlogId).Single();
                firstOfTheRange.Name = "Jamais deux sans trois";

                var rangeTestBlogs = new List<Blog>() {
                firstOfTheRange,
                new Blog(){ Name = "Blog 2"},
                new Blog(){ Name = "Blog 3"},
                new Blog(){ Name = "Blog 4"}
                };

                Console.WriteLine($"Insert or update Blogs from RangeTestBlogs into db.");
                db.InsertOrUpdateRange(rangeTestBlogs);

                Console.WriteLine($"Save Blogs from RangeTestBlogs into db.");
                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("All blogs in the database:");
                blogs = db.Blogs.ToList();
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"Blog ID: {blog.BlogId}.\tBlog NAME: {blog.Name}.");
                }
                Console.WriteLine();

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
