using Marten.Schema;

namespace Catalog.Api.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        // Marten UPSERT will cater for existing records
        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>
        {
            new()
            {
                Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                Name = "IPhone X",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years.",
                ImageFile = "product-1.png",
                Price = 1000.00m,
                Category = ["Phone", "Smartphone"]
            },
            new()
            {
                Id = new Guid("c67d6323-e8b1-4229-9778-0cc05257425e"),
                Name = "Samsung Galaxy S21",
                Description = "The Galaxy S21 is a mid-range smartphone from Samsung.",
                ImageFile = "product-2.png",
                Price = 1200.00m,
                Category = ["Phone", "Smartphone"]
            },
            new()
            {
                Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                Name = "Sony WH-1000XM4",
                Description =
                    "Industry-leading wireless noise canceling headphones with exceptional sound quality.",
                ImageFile = "product-3.png",
                Price = 349.99m,
                Category = ["Audio", "Headphones", "Electronics"]
            },
            new()
            {
                Id = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"),
                Name = "Dell XPS 13",
                Description =
                    "Ultra-thin 13-inch laptop with Intel Core i7 processor and 16GB RAM.",
                ImageFile = "product-4.png",
                Price = 1299.99m,
                Category = ["Laptop", "Computer", "Electronics"]
            },
            new()
            {
                Id = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8"),
                Name = "Nike Air Max 270",
                Description =
                    "Comfortable running shoes with Air Max technology for superior cushioning.",
                ImageFile = "product-5.png",
                Price = 159.99m,
                Category = ["Footwear", "Sports", "Shoes"]
            },
            new()
            {
                Id = new Guid("550e8400-e29b-41d4-a716-446655440000"),
                Name = "Instant Pot Duo 7-in-1",
                Description =
                    "Multi-functional electric pressure cooker that replaces 7 kitchen appliances.",
                ImageFile = "product-6.png",
                Price = 89.95m,
                Category = ["Kitchen", "Appliance", "Home"]
            },
            new()
            {
                Id = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8"),
                Name = "The Art of Clean Code",
                Description =
                    "Essential programming book covering best practices for writing maintainable code.",
                ImageFile = "product-7.png",
                Price = 29.99m,
                Category = ["Book", "Programming", "Education"]
            }
        };
    }
}