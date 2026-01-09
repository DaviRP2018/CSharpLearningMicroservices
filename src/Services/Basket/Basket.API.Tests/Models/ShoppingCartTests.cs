using Basket.API.Models;
using JetBrains.Annotations;

namespace Basket.API.Tests.Models;

[TestSubject(typeof(ShoppingCart))]
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

    [Fact]
    public void ShoppingCart_EmptyCart_TotalPriceShouldBeZero()
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");

        // Act & Assert
        Assert.Empty(shoppingCart.Items);
        Assert.Equal(0, shoppingCart.TotalPrice);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10.5)]
    [InlineData(100)]
    public void ShoppingCart_SingleItem_TotalPriceShouldBeCorrect(decimal price)
    {
        // Arrange
        var quantity = 2;
        var shoppingCart = new ShoppingCart("testuser");
        shoppingCart.Items.Add(new ShoppingCartItem(quantity, "Red", price, Guid.NewGuid(),
            "Product"));

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        Assert.Equal(quantity * price, totalPrice);
    }

    [Fact]
    public void ShoppingCart_MultipleItems_TotalPriceShouldBeSumOfAllItems()
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");
        shoppingCart.Items.AddRange(new List<ShoppingCartItem>
        {
            new(2, "Red", 10, Guid.NewGuid(), "P1"),
            new(1, "Blue", 20, Guid.NewGuid(), "P2"),
            new(5, "Green", 5, Guid.NewGuid(), "P3")
        });

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        // (2 * 10) + (1 * 20) + (5 * 5) = 20 + 20 + 25 = 65
        Assert.Equal(65m, totalPrice);
    }

    [Fact]
    public void ShoppingCart_ItemWithZeroQuantity_TotalPriceShouldBeCorrect()
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");
        shoppingCart.Items.Add(new ShoppingCartItem(0, "Red", 100, Guid.NewGuid(), "P1"));

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        Assert.Equal(0, totalPrice);
    }

    [Fact]
    public void ShoppingCart_ItemWithZeroPrice_TotalPriceShouldBeCorrect()
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");
        shoppingCart.Items.Add(new ShoppingCartItem(10, "Red", 0, Guid.NewGuid(), "P1"));

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        Assert.Equal(0, totalPrice);
    }

    [Theory]
    [InlineData(10, 5, 50)]
    [InlineData(0, 5, 0)]
    [InlineData(10, 0, 0)]
    [InlineData(1, 1.25, 1.25)]
    public void ShoppingCart_VariousQuantitiesAndPrices_TotalPriceShouldBeCorrect(int quantity,
        decimal price, decimal expectedTotal)
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");
        shoppingCart.Items.Add(new ShoppingCartItem(quantity, "Color", price, Guid.NewGuid(),
            "Product"));

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        Assert.Equal(expectedTotal, totalPrice);
    }

    [Fact]
    public void ShoppingCart_LargeNumberOfItems_TotalPriceShouldBeCorrect()
    {
        // Arrange
        var shoppingCart = new ShoppingCart("testuser");
        var random = new Random();
        var expectedTotal = 0m;

        for (var i = 0; i < 100; i++)
        {
            var quantity = random.Next(1, 10);
            var price = Math.Round((decimal)(random.NextDouble() * 100), 2);
            shoppingCart.Items.Add(new ShoppingCartItem(quantity, $"Color{i}", price,
                Guid.NewGuid(), $"Product{i}"));
            expectedTotal += quantity * price;
        }

        // Act
        var totalPrice = shoppingCart.TotalPrice;

        // Assert
        Assert.Equal(expectedTotal, totalPrice);
    }
}