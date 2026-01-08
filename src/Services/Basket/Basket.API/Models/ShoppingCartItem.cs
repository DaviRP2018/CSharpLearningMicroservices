namespace Basket.API.Models;

public class ShoppingCartItem
{
    public ShoppingCartItem()
    {
    }

    public ShoppingCartItem(int quantity, string color, decimal price, Guid productId,
        string productName)
    {
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductId = productId;
        ProductName = productName;
    }

    public int Quantity { get; set; } = 0;
    public string Color { get; set; } = null!;
    public decimal Price { get; set; } = 0;
    public Guid ProductId { get; set; } = Guid.Empty;
    public string ProductName { get; set; } = null!;
}