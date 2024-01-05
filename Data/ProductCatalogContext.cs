using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;

namespace ProductCatalog.Data
{
    public class ProductCatalogContext : DbContext
    {
        public ProductCatalogContext (DbContextOptions<ProductCatalogContext> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; } = default!;
        public DbSet<Article> articles { get; set; }
        
    }
}
