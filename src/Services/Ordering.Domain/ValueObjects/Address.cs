namespace Ordering.Domain.ValueObjects;

public record Address
{
    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public string? EmailAddress { get; } = null!;
    public string AddressLine { get; } = null!;
    public string Country { get; } = null!;
    public string State { get; } = null!;
    public string ZipCode { get; } = null!;
}