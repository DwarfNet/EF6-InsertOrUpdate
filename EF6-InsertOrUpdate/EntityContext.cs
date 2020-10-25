using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Design;

namespace EF6_InsertOrUpdate
{
    public class EntityContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void InsertOrUpdate<T>(T entity) where T : class
        {
            DbSet<T> dbSet = this.Set<T>();
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached || entry.State == EntityState.Added)
                dbSet.Add(entity);
        }

        public void InsertOrUpdateRange<T>(List<T> entities) where T : class
        {
            DbSet<T> dbSet = this.Set<T>();

            foreach (var entity in entities)
            {
                var entry = this.Entry(entity);

                if (entry.State == EntityState.Detached || entry.State == EntityState.Added)
                    dbSet.Add(entity);
            }
        }


    }
}
