using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Products.CreateProduct;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Catalog.Api.Tests.Products.CreateProduct;

public class CreateProductEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ISender> _senderMock;

    public CreateProductEndpointTests(WebApplicationFactory<Program> factory)
    {
        _senderMock = new Mock<ISender>();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_senderMock.Object);
            });
        });
    }

    [Fact]
    public async Task CreateProduct_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateProductRequest(
            "Test Product",
            ["Test Category"],
            "Test Description",
            "test.png",
            100.00m
        );

        var productId = Guid.NewGuid();
        _senderMock
            .Setup(s => s.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateProductResult(productId));

        // Act
        var response = await client.PostAsJsonAsync("/products", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"/products/{productId}");

        var result = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(productId);
    }

    [Fact]
    public async Task
        CreateProduct_WithInvalidRequest_ShouldReturnInternalServerError_WhenHandlerThrowsValidationExceptionAndNoGlobalHandler()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateProductRequest(
            "", // Invalid
            ["Test Category"],
            "Test Description",
            "test.png",
            100.00m
        );

        _senderMock
            .Setup(s => s.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException("Name is required"));

        // Act
        var response = await client.PostAsJsonAsync("/products", request);

        // Assert
        // Currently, without a global exception handler, this returns 500.
        // This test documents the current behavior and ensures the endpoint is reachable.
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}