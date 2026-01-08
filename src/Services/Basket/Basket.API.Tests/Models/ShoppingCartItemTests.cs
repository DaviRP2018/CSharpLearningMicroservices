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

    [Fact]
    public void ShoppingCartItem_ParameterizedConstructor_ShouldSetProperties()
    {
        // Arrange
        var quantity = 5;
        var color = "Blue";
        var price = 50.5m;
        var productId = Guid.NewGuid();
        var productName = "Product B";

        // Act
        var shoppingCartItem =
            new ShoppingCartItem(quantity, color, price, productId, productName);

        // Assert
        Assert.Equal(quantity, shoppingCartItem.Quantity);
        Assert.Equal(color, shoppingCartItem.Color);
        Assert.Equal(price, shoppingCartItem.Price);
        Assert.Equal(productId, shoppingCartItem.ProductId);
        Assert.Equal(productName, shoppingCartItem.ProductName);
    }

    [Theory]
    [InlineData(1, "Red", 10.0, "Product 1")]
    [InlineData(100, "Green", 0.99, "Product 2")]
    [InlineData(0, "", 0, "")]
    public void ShoppingCartItem_ValidInputs_ShouldSetPropertiesCorrectly(int quantity,
        string color, decimal price, string productName)
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var item = new ShoppingCartItem
        {
            Quantity = quantity,
            Color = color,
            Price = price,
            ProductId = productId,
            ProductName = productName
        };

        // Assert
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(color, item.Color);
        Assert.Equal(price, item.Price);
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(productName, item.ProductName);
    }

    [Fact]
    public void ShoppingCartItem_NegativeValues_ShouldBeStoredAsProvided()
    {
        // Arrange
        var quantity = -1;
        var price = -100m;

        // Act
        var item = new ShoppingCartItem
        {
            Quantity = quantity,
            Price = price
        };

        // Assert
        // Documenting current behavior: The model does not currently validate against negative values.
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(price, item.Price);
    }

    [Fact]
    public void ShoppingCartItem_RandomizedData_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var random = new Random();
        for (var i = 0; i < 50; i++)
        {
            var quantity = random.Next(-100, 1000);
            var color = Guid.NewGuid().ToString();
            var price = (decimal)random.NextDouble() * 1000;
            var productId = Guid.NewGuid();
            var productName = "Product_" + Guid.NewGuid();

            // Act
            var item = new ShoppingCartItem(quantity, color, price, productId, productName);

            // Assert
            Assert.Equal(quantity, item.Quantity);
            Assert.Equal(color, item.Color);
            Assert.Equal(price, item.Price);
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(productName, item.ProductName);
        }
    }
}