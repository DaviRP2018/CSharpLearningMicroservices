namespace Ordering.Domain.Models;

public class Product : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; } = 0;
}