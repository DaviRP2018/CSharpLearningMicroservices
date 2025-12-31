namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    private CustomerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CustomerId Of(Guid value)
    {
        // TODO: CA2264: Chamar "ArgumentNullException.ThrowIfNull" e
        //  repassar um valor não anulável é uma operação nula
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
            throw new DomainException("CustomerId can't be empty.");

        return new CustomerId(value);
    }
}