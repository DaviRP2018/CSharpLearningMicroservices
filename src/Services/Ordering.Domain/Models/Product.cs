namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; } = 0;
}