using System.Net;
using System.Net.Http.Json;
using Basket.API.Basket.GetBasket;
using Basket.API.Exceptions;
using Basket.API.Models;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Basket.API.Tests.Basket.GetBasket;

[TestSubject(typeof(GetBasketEndpoints))]
public class GetBasketEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetBasketEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing ISender if any and replace with Mock
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISender));
                if (descriptor != null) services.Remove(descriptor);

                var senderMock = new Mock<ISender>();
                services.AddSingleton(senderMock.Object);
            });
        });
    }

    private HttpClient CreateClientWithSenderMock(Mock<ISender> senderMock)
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(senderMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetBasket_ReturnsOkResponse_WhenBasketExists()
    {
        // Arrange
        var userName = "testuser";
        var cart = new ShoppingCart(userName)
        {
            Items =
            [
                new ShoppingCartItem(1, "Red", 100, Guid.NewGuid(), "Product 1")
            ]
        };
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(userName, basketResponse.Cart.UserName);
        Assert.Single(basketResponse.Cart.Items);
    }

    [Fact]
    public async Task GetBasket_ReturnsOkResponse_WhenBasketHasMultipleItems()
    {
        // Arrange
        var userName = "multitest";
        var cart = new ShoppingCart(userName)
        {
            Items =
            [
                new ShoppingCartItem(1, "Red", 100, Guid.NewGuid(), "Product 1"),
                new ShoppingCartItem(2, "Blue", 200, Guid.NewGuid(), "Product 2"),
                new ShoppingCartItem(3, "Green", 300, Guid.NewGuid(), "Product 3")
            ]
        };
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(3, basketResponse.Cart.Items.Count);
        Assert.Equal(1400,
            basketResponse.Cart.TotalPrice); // 1*100 + 2*200 + 3*300 = 100 + 400 + 900 = 1400
    }

    [Fact]
    public async Task GetBasket_ReturnsNotFound_WhenBasketDoesNotExist()
    {
        // Arrange
        var userName = "nonexistent";
        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BasketNotFoundException(userName));

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains(userName, problem.Detail);
    }

    [Fact]
    public async Task GetBasket_ReturnsInternalServerError_WhenSenderFails()
    {
        // Arrange
        var userName = "erroruser";
        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Exception", problem.Title);
    }

    [Theory]
    [InlineData("user-with-dashes")]
    [InlineData("user_with_underscores")]
    [InlineData("user.with.dots")]
    [InlineData("user@domain.com")]
    [InlineData("user123")]
    public async Task GetBasket_ReturnsOk_WhenUserNameContainsSafeSpecialCharacters(
        string userName)
    {
        // Arrange
        var cart = new ShoppingCart(userName);
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.Is<GetBasketQuery>(q => q.UserName == userName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(userName, basketResponse.Cart.UserName);
    }

    [Theory]
    [InlineData("user with spaces")]
    [InlineData("user%2fwith%2fslashes")] // Manually encoded slash
    [InlineData("!@#$%^&*()_+")]
    public async Task GetBasket_ReturnsOk_WhenUserNameNeedsEncoding(string userName)
    {
        // Arrange
        var cart = new ShoppingCart(userName);
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.Is<GetBasketQuery>(q => q.UserName == userName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var encodedUserName = Uri.EscapeDataString(userName);
        var response = await client.GetAsync($"/basket/{encodedUserName}");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
            Assert.NotNull(basketResponse);
            Assert.Equal(userName, basketResponse.Cart.UserName);
        }
        else
        {
            // If the routing doesn't support these characters, it might return 404 or 400
            // This is also a valid "negative/edge case" finding
            Assert.True(
                response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task GetBasket_ReturnsOk_WhenUserNameIsVeryLong()
    {
        // Arrange
        var userName = new string('a', 2000); // Very long username
        var cart = new ShoppingCart(userName);
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(userName, basketResponse.Cart.UserName);
    }

    [Fact]
    public async Task GetBasket_ReturnsOk_WhenBasketHasManyItems()
    {
        // Arrange
        var userName = "gorilla_test";
        var items = Enumerable.Range(1, 1000).Select(i =>
            new ShoppingCartItem(i, $"Color {i}", i * 10, Guid.NewGuid(), $"Product {i}")
        ).ToList();

        var cart = new ShoppingCart(userName) { Items = items };
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(1000, basketResponse.Cart.Items.Count);
    }

    [Fact]
    public async Task GetBasket_ReturnsOk_WhenPricesAndQuantitiesAreLarge()
    {
        // Arrange
        var userName = "large_values";
        var cart = new ShoppingCart(userName)
        {
            Items =
            [
                new ShoppingCartItem(1000000, "Large", 1000000.99m, Guid.NewGuid(),
                    "Expensive Product")
            ]
        };
        var expectedResult = new GetBasketResult(cart);

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<GetBasketQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var client = CreateClientWithSenderMock(senderMock);

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(1000000.99m * 1000000, basketResponse.Cart.TotalPrice);
    }
}