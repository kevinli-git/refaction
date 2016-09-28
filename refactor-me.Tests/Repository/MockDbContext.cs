using refactor_me.Models;
using refactor_me.Repository;
using System;
using System.Data.Entity;
using System.Linq;

namespace refactor_me.Tests.Repository
{
    public class MockDbContext : IProductDbContext
    {
        public MockDbContext()
        {
            Products = new MockProductDbSet();
            ProductOptions = new MockProductOptionDbSet();
        }

        public DbSet<ProductOption> ProductOptions { get; set; }

        public DbSet<Product> Products { get; set; }

        public void Dispose()
        {
        }

        public bool ProductExists(Guid id)
        {
            return Products.Any(p => p.Id == id);
        }

        public bool ProductOptionExists(Guid id)
        {
            return ProductOptions.Any(po => po.Id == id);
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void SetEntityModified<T>(T entity) where T : class
        {
        }
    }
}
