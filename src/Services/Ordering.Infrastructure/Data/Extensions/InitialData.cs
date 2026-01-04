namespace Ordering.Infrastructure.Data.Extensions;

public class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("9dbc33d1-2398-4b96-adc4-2752846e5b90")),
                "john", "john@gemail.com.br"),
            Customer.Create(CustomerId.Of(new Guid("68a41639-2658-4839-a201-aa1968ae8e3e")),
                "harry",
                "hcanalha@gemail.com.br")
        };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("f3647b18-0801-4c00-824e-f76dacd11e68")),
                "IPhoneX", 100000),
            Product.Create(ProductId.Of(new Guid("199655c3-0a38-4fa7-9faa-1ff4fb49adaa")),
                "Samsung 22", 5000),
            Product.Create(ProductId.Of(new Guid("5f458f08-a5fe-4fdc-907e-c3e8636fefce")),
                "Lixo", 2),
            Product.Create(ProductId.Of(new Guid("8525fc11-3ffc-4de5-abec-e8141eddb718")),
                "Geladeira", 3000)
        };

    public static IEnumerable<Order> OrderWithItems
    {
        get
        {
            var address1 = Address.Of("john", "bow jones", "john@gemail.com.br", "Rua tal N:4",
                "Brazil", "São Paulo", "14096100");
            var address2 = Address.Of("harry", "canalha", "hcanalha@gemail.com.br", "Rua lixo " +
                "N:1", "US", "California", "08050");

            var payment1 = Payment.Of("john", "1234567809876543", "12/28", "355", 1);
            var payment2 = Payment.Of("harry", "1111222233334444", "12/99", "999", 2);

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("9dbc33d1-2398-4b96-adc4-2752846e5b90")),
                OrderName.Of("Order 1"),
                address1,
                address1,
                payment1
            );
            order1.Add(ProductId.Of(new Guid("f3647b18-0801-4c00-824e-f76dacd11e68")), 2, 100000);
            order1.Add(ProductId.Of(new Guid("199655c3-0a38-4fa7-9faa-1ff4fb49adaa")), 1, 5000);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("68a41639-2658-4839-a201-aa1968ae8e3e")),
                OrderName.Of("Order 2"),
                address2,
                address2,
                payment2
            );
            order2.Add(ProductId.Of(new Guid("5f458f08-a5fe-4fdc-907e-c3e8636fefce")), 4, 2);
            order2.Add(ProductId.Of(new Guid("8525fc11-3ffc-4de5-abec-e8141eddb718")), 10, 3000);

            return new List<Order> { order1, order2 };
        }
    }
}