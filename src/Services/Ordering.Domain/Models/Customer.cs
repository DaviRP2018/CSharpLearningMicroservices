namespace Ordering.Domain.Models;

public class Customer : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
}