using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EF6_InsertOrUpdate
{
    class Program
    {
        private readonly static int StaticId = 5;
        private static int IncrementedId = 7;

        static void Main(string[] args)
        {
            using (var db = new EntityContext())
            {
                CleanCurrentDb(db);
            }

            for (int i = 0; i < 10; i++)
            {
                Test();
                i++;
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void Test()
        {
            using (var db = new EntityContext())
            {
                var name = $"Blog Init Name { StaticId }";
                Console.WriteLine($"Create Blog {name}.");
                var originalBlog = new Blog { BlogId = StaticId, Name = name };
                SingleChange(db, originalBlog);
                RangeTest(db, originalBlog);
                //TestWithClassNotInOriginalDbSet(db);
            }
        }

        private static void SingleChange(EntityContext db, Blog originalBlog)
        {
            List<Blog> blogs;
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

            Console.WriteLine($"Get Blog where Name is {originalBlog.Name}.");
            var oldBlog = db.Blogs.Where(b => b.BlogId == originalBlog.BlogId).FirstOrDefault();
            Console.WriteLine($"Blog ID is : {oldBlog.BlogId}.");

            oldBlog.Name = $"The new Blog {StaticId}";
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
        }

        private static void RangeTest(EntityContext db, Blog originalBlog)
        {
            List<Blog> blogs;
            var firstOfTheRange = db.Blogs.Where(b => b.BlogId == originalBlog.BlogId).Single();
            firstOfTheRange.Name = $"The new Blog {StaticId}";

            var rangeTestBlogs = new List<Blog>() {
                firstOfTheRange,
                new Blog(){BlogId=IncrementedId++, Name = "Blog 2"},
                new Blog(){BlogId=IncrementedId++, Name = "Blog 3"},
                new Blog(){BlogId=IncrementedId++, Name = "Blog 4"}
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
        }

        private static void TestWithClassNotInOriginalDbSet(EntityContext db)
        {
            var errorItem = new ClassNotInDB()
            {
                Value = 10
            };
            db.InsertOrUpdate(errorItem);
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
