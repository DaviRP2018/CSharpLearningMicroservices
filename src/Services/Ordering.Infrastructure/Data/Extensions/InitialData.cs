namespace Ordering.Infrastructure.Data.Extensions;

public class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("9dbc33d1-2398-4b96-adc4-2752846e5b90")),
                "harry", "harry@gemail.com.br"),
            Customer.Create(CustomerId.Of(new Guid("68a41639-2658-4839-a201-aa1968ae8e3e")),
                "john",
                "john@gemail.com.br")
        };
}