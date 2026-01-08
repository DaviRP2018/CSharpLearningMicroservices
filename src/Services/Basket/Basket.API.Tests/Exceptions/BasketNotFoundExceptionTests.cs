using Basket.API.Exceptions;
using BuildingBlocks.Exceptions;

namespace Basket.API.Tests.Exceptions;

public class BasketNotFoundExceptionTests
{
    [Fact]
    public void BasketNotFoundException_ShouldSetMessageCorrectly()
    {
        // Arrange
        var userName = "testuser";
        var expectedMessage = $"Entity \"Basket\" ({userName}) was not found.";

        // Act
        var exception = new BasketNotFoundException(userName);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void BasketNotFoundException_ShouldInheritFromNotFoundException()
    {
        // Arrange
        var userName = "testuser";

        // Act
        var exception = new BasketNotFoundException(userName);

        // Assert
        Assert.IsAssignableFrom<NotFoundException>(exception);
        Assert.IsAssignableFrom<Exception>(exception);
    }
}