using System.Net.Http.Json;
using Basket.API.Basket.GetBasket;
using Basket.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Basket.API.Tests.Basket.GetBasket;

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

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(senderMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync($"/basket/{userName}");

        // Assert
        response.EnsureSuccessStatusCode();
        var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();
        Assert.NotNull(basketResponse);
        Assert.Equal(userName, basketResponse.Cart.UserName);
        Assert.Single(basketResponse.Cart.Items);
    }
}