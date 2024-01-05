using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;

namespace ProductCatalog.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ProductCatalogContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ProductCatalogContext>>()))
        {
            // Look for any users.
            if (context.users.Any())
            {
                return;   // DB has been seeded
            }

            var testUser = new User
            {
                Name = "Max Seller",
                Login = "msell",
                Password = "top seller",
                DateOfBirth = new DateTime(1985, 5, 14)
            };
            context.Add(testUser);

            context.SaveChanges();
        }
    }
}