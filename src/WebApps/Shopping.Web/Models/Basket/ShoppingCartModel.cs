namespace Shopping.Web.Models.Basket;

public class ShoppingCartModel
{
    public string UserName { get; set; } = null!;
    public List<ShoppingCartItemModel> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}

public class ShoppingCartItemModel
{
    public int Quantity { get; set; } = 0;
    public string Color { get; set; } = null!;
    public decimal Price { get; set; } = 0;
    public Guid ProductId { get; set; } = Guid.Empty;
    public string ProductName { get; set; } = null!;
}

// wrapper classes
public record GetBasketResponse(ShoppingCartModel Cart);

public record StoreBasketRequest(ShoppingCartModel Cart);

public record StoreBasketResponse(string UserName);

public record DeleteBasketResponse(bool IsSuccess);