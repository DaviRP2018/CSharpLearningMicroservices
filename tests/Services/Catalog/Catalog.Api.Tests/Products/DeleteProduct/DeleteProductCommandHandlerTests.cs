using Catalog.Api.Models;
using Catalog.Api.Products.DeleteProduct;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.Api.Tests.Products.DeleteProduct;

public class DeleteProductCommandHandlerTests
{
    private readonly DeleteProductCommandHandler _handler;
    private readonly Mock<ILogger<DeleteProductCommandHandler>> _loggerMock;
    private readonly Mock<IDocumentSession> _sessionMock;

    public DeleteProductCommandHandlerTests()
    {
        _sessionMock = new Mock<IDocumentSession>();
        _loggerMock = new Mock<ILogger<DeleteProductCommandHandler>>();
        _handler = new DeleteProductCommandHandler(_sessionMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldDeleteProductAndReturnSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _sessionMock.Verify(s => s.Delete<Product>(productId), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogInformation()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!.Contains("DeleteProductCommandHandler.Handle called")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}