using System;
using System.Data.Entity;
using refactor_me.Models;

namespace refactor_me.Repository
{
    public interface IProductDbContext : IDisposable
    {
        DbSet<Product> Products { get; }
        DbSet<ProductOption> ProductOptions { get; }
        int SaveChanges();

        void SetEntityModified<T>(T entity) where T : class;
        bool ProductExists(Guid id);
        bool ProductOptionExists(Guid id);
    }
}
