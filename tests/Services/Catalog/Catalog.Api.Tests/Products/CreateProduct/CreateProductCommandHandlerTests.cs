using Catalog.Api.Models;
using Catalog.Api.Products.CreateProduct;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.Api.Tests.Products.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<ILogger<CreateProductCommandHandler>> _loggerMock;
    private readonly CreateProductCommandHandler _handler;
    private readonly Mock<IDocumentSession> _sessionMock;

    public CreateProductCommandHandlerTests()
    {
        _sessionMock = new Mock<IDocumentSession>();
        _loggerMock = new Mock<ILogger<CreateProductCommandHandler>>();
        _handler = new CreateProductCommandHandler(_sessionMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProductAndReturnId()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Test Product",
            ["Test Category"],
            "Test Description",
            "test.png",
            100.00m
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        _sessionMock.Verify(s => s.Store(It.Is<Product>(p =>
            p.Name == command.Name &&
            p.Category == command.Category &&
            p.Description == command.Description &&
            p.ImageFile == command.ImageFile &&
            p.Price == command.Price
        )), Times.Once);

        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /*
     * Note: Validation is handled by ValidationBehavior in the MediatR pipeline,
     * not by the CommandHandler itself.
     * These tests focus on the Handler logic assuming validation has passed.
     */
}