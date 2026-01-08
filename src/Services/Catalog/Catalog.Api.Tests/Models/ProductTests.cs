using Catalog.Api.Models;

namespace Catalog.Api.Tests.Models;

public class ProductTests
{
    [Fact]
    public void Product_Initialization_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Product";
        var categories = new List<string> { "Category 1", "Category 2" };
        var description = "Test Description";
        var imageFile = "test.png";
        var price = 10.99m;

        // Act
        var product = new Product
        {
            Id = id,
            Name = name,
            Category = categories,
            Description = description,
            ImageFile = imageFile,
            Price = price
        };

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(categories, product.Category);
        Assert.Equal(description, product.Description);
        Assert.Equal(imageFile, product.ImageFile);
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Product_Category_ShouldBeInitializedAsEmptyList()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.NotNull(product.Category);
        Assert.Empty(product.Category);
    }

    [Fact]
    public void Product_Price_ShouldBeAssignable()
    {
        // Arrange
        var product = new Product();
        var price = 100.50m;

        // Act
        product.Price = price;

        // Assert
        Assert.Equal(price, product.Price);
    }
}