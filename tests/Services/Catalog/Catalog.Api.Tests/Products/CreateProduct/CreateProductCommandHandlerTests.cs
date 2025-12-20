using Catalog.Api.Models;
using Catalog.Api.Products.CreateProduct;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Marten;
using Moq;

namespace Catalog.Api.Tests.Products.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly CreateProductCommandHandler _handler;
    private readonly Mock<IDocumentSession> _sessionMock;
    private readonly Mock<IValidator<CreateProductCommand>> _validatorMock;

    public CreateProductCommandHandlerTests()
    {
        _sessionMock = new Mock<IDocumentSession>();
        _validatorMock = new Mock<IValidator<CreateProductCommand>>();
        _handler = new CreateProductCommandHandler(_sessionMock.Object, _validatorMock.Object);
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

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

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

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateProductCommand(
            "", // Invalid name
            ["Test Category"],
            "Test Description",
            "test.png",
            100.00m
        );

        var validationFailures = new List<ValidationFailure>
        {
            new("Name", "Name is required")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Name is required");

        _sessionMock.Verify(s => s.Store(It.IsAny<Product>()), Times.Never);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}