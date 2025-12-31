namespace Ordering.Domain.ValueObjects;

public record Payment
{
    protected Payment()
    {
    }

    private Payment(
        string cardName, string cardNumber, string expiration, string cvv,
        int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        Cvv = cvv;
        PaymentMethod = paymentMethod;
    }

    public string? CardName { get; } = null!;
    public string CardNumber { get; } = null!;
    public string Expiration { get; } = null!;
    public string Cvv { get; } = null!;
    public int PaymentMethod { get; } = 0;

    public static Payment Of(
        string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new Payment(
            cardName, cardNumber, expiration,
            cvv, paymentMethod);
    }
}