using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Design;

namespace EF6_InsertOrUpdate
{
    public class EntityContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().Property(b => b.BlogId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
        public void InsertOrUpdate<T>(T entity) where T : class
        {
            DbSet<T> dbSet = this.Set<T>();
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached || entry.State == EntityState.Added)
                AddEntity(dbSet, entry, entity);
            else
                UpdateEntity(dbSet, entry, entity);
        }

        public void InsertOrUpdateRange<T>(List<T> entities) where T : class
        {
            DbSet<T> dbSet = this.Set<T>();

            foreach (var entity in entities)
            {
                var entry = this.Entry(entity);

                if (entry.State == EntityState.Detached || entry.State == EntityState.Added)
                    AddEntity(dbSet, entry, entity);
                else
                    UpdateEntity(dbSet, entry, entity);
            }
        }

        private void UpdateEntity<T>(DbSet<T> dbSet, System.Data.Entity.Infrastructure.DbEntityEntry<T> entry, T entity) where T : class
        {
            //entry.Attach(entity);
            //entry.State = EntityState.Modified;
            this.Entry(entity).CurrentValues.SetValues(entity);
            //this.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        private void AddEntity<T>(DbSet<T> dbSet, System.Data.Entity.Infrastructure.DbEntityEntry<T> entry, T entity) where T : class
        {
            dbSet.Add(entity);
        }
    }
}
