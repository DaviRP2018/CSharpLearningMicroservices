using System.Text.Json;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Exceptions.Handler;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BuildingBlocks.Tests.Exceptions.Handler;

public class CustomExceptionHandlerTests
{
    private readonly CustomExceptionHandler _handler;
    private readonly Mock<ILogger<CustomExceptionHandler>> _loggerMock;

    public CustomExceptionHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CustomExceptionHandler>>();
        _handler = new CustomExceptionHandler(_loggerMock.Object);
    }

    [Fact]
    public async Task TryHandleAsync_InternalServerException_Returns500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var exception = new InternalServerException("Internal error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails =
            await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be(nameof(InternalServerException));
        problemDetails.Detail.Should().Be(exception.Message);
    }

    [Fact]
    public async Task TryHandleAsync_ValidationException_Returns400WithErrors()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var errors = new[] { new ValidationFailure("Prop", "Error") };
        var exception = new ValidationException("Validation failed", errors);

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails =
            await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Title.Should().Be(nameof(ValidationException));

        problemDetails.Extensions.Should().ContainKey("ValidationErrors");
    }

    [Fact]
    public async Task TryHandleAsync_BadRequestException_Returns400()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var exception = new BadRequestException("Bad request");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails =
            await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Title.Should().Be(nameof(BadRequestException));
    }

    [Fact]
    public async Task TryHandleAsync_NotFoundException_Returns404()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var exception = new NotFoundException("Not found");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails =
            await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Title.Should().Be(nameof(NotFoundException));
    }

    [Fact]
    public async Task TryHandleAsync_UnknownException_Returns500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var exception = new Exception("Unknown error");

        // Act
        var result = await _handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var problemDetails =
            await JsonSerializer.DeserializeAsync<ProblemDetails>(context.Response.Body);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be(nameof(Exception));
    }
}