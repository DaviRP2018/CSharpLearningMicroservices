using Basket.API.Basket.GetBasket;
using Basket.API.Data;
using Basket.API.Exceptions;
using Basket.API.Models;
using JetBrains.Annotations;
using Moq;

namespace Basket.API.Tests.Basket.GetBasket;

[TestSubject(typeof(GetBasketQueryHandler))]
public class GetBasketQueryHandlerTests
{
    private readonly GetBasketQueryHandler _handler;
    private readonly Mock<IBasketRepository> _repositoryMock;

    public GetBasketQueryHandlerTests()
    {
        _repositoryMock = new Mock<IBasketRepository>();
        _handler = new GetBasketQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsGetBasketResult_WhenBasketExists()
    {
        // Arrange
        var userName = "testuser";
        var cart = new ShoppingCart(userName);
        var query = new GetBasketQuery(userName);

        _repositoryMock.Setup(r => r.GetBasket(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cart, result.Cart);
        _repositoryMock.Verify(r => r.GetBasket(userName, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsBasketNotFoundException_WhenBasketDoesNotExist()
    {
        // Arrange
        var userName = "nonexistent";
        var query = new GetBasketQuery(userName);

        _repositoryMock.Setup(r => r.GetBasket(userName, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BasketNotFoundException(userName));

        // Act & Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Handle_HandlesEdgeCaseUserNames_WhenRepositoryAcceptsThem(string? userName)
    {
        // Arrange
        var cart = new ShoppingCart(userName!);
        var query = new GetBasketQuery(userName!);

        _repositoryMock.Setup(r => r.GetBasket(userName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.Cart.UserName);
    }

    [Fact]
    public async Task Handle_PropagatesException_WhenRepositoryThrowsGeneralException()
    {
        // Arrange
        var userName = "testuser";
        var query = new GetBasketQuery(userName);

        _repositoryMock.Setup(r => r.GetBasket(userName, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(query, CancellationToken.None));
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        var userName = "testuser";
        var query = new GetBasketQuery(userName);
        using var cts = new CancellationTokenSource();

        _repositoryMock.Setup(r => r.GetBasket(userName, cts.Token))
            .ReturnsAsync(new ShoppingCart(userName));

        // Act
        await _handler.Handle(query, cts.Token);

        // Assert
        _repositoryMock.Verify(r => r.GetBasket(userName, cts.Token), Times.Once);
    }

    [Theory]
    [InlineData(
        "a_very_long_user_name_that_might_cause_issues_in_some_systems_but_should_be_fine_here_1234567890")]
    [InlineData("!@#$%^&*()_+{}[]|\\:;\"'<>,.?/")]
    [InlineData("用户名称")] // Unicode
    [InlineData("   testuser   ")] // Whitespace
    public async Task Handle_FuzzAndGorilla_WithDiverseUserNames(string userName)
    {
        // Arrange
        var cart = new ShoppingCart(userName);
        var query = new GetBasketQuery(userName);

        _repositoryMock.Setup(r => r.GetBasket(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.Cart.UserName);
        _repositoryMock.Verify(r => r.GetBasket(userName, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}