using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using refactor_me.Models;

namespace refactor_me.Repository
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public ProductDbContext()
            : base("name=ProductDbContext")
        {
        }

        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Product> Products { get; set; }

        public void SetEntityModified<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public bool ProductExists(Guid id)
        {
            return Products.Any(e => e.Id == id);
        }

        public bool ProductOptionExists(Guid id)
        {
            return ProductOptions.Any(e => e.Id == id);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //turn off database initializer to work with existing database
            Database.SetInitializer<ProductDbContext>(null);
            //table names are singular
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
