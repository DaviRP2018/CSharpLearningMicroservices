using Basket.API.Models;

namespace Basket.API.Tests.Models;

public class ShoppingCartItemTests
{
    [Fact]
    public void ShoppingCartItem_Initialization_ShouldSetProperties()
    {
        // Arrange
        var quantity = 2;
        var color = "White";
        var price = 100m;
        var productId = Guid.NewGuid();
        var productName = "Test Product";

        // Act
        var shoppingCartItem = new ShoppingCartItem
        {
            Quantity = quantity,
            Color = color,
            Price = price,
            ProductId = productId,
            ProductName = productName
        };

        // Assert
        Assert.Equal(quantity, shoppingCartItem.Quantity);
        Assert.Equal(color, shoppingCartItem.Color);
        Assert.Equal(price, shoppingCartItem.Price);
        Assert.Equal(productId, shoppingCartItem.ProductId);
        Assert.Equal(productName, shoppingCartItem.ProductName);
    }

    [Fact]
    public void ShoppingCartItem_Initialization_ShouldHaveDefaultValues()
    {
        // Arrange
        var quantity = 0;
        var price = 0m;
        var productId = Guid.Empty;

        // Act
        var shoppingCartItem = new ShoppingCartItem();

        // Assert
        Assert.Equal(quantity, shoppingCartItem.Quantity);
        Assert.Null(shoppingCartItem.Color);
        Assert.Equal(price, shoppingCartItem.Price);
        Assert.Equal(productId, shoppingCartItem.ProductId);
        Assert.Null(shoppingCartItem.ProductName);
    }
}