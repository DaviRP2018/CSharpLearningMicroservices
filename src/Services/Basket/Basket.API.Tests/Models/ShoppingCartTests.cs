using Basket.API.Models;

namespace Basket.API.Tests.Models;

public class ShoppingCartTests
{
    [Fact]
    public void ShoppingCart_Initialization_ShouldSetProperties()
    {
        // Arrange
        var userName = "Johnny Test";
        var items = new List<ShoppingCartItem>
        {
            new(1, "Black", 100, Guid.NewGuid(),
                "Test Product 1"),
            new(2, "White", 200, Guid.NewGuid(),
                "Test Product 2")
        };

        // Act
        var shoppingCart = new ShoppingCart
        {
            UserName = userName,
            Items = items
        };

        // Assert
        Assert.Equal(userName, shoppingCart.UserName);
        Assert.Equivalent(items, shoppingCart.Items);
        Assert.Collection(shoppingCart.Items,
            item =>
            {
                Assert.Equal("Black", item.Color);
                Assert.Equal(100, item.Price);
            },
            item =>
            {
                Assert.Equal("White", item.Color);
                Assert.Equal(200, item.Price);
            }
        );
        Assert.Equal(500m, shoppingCart.TotalPrice);
    }
}