// using BuildingBlocks.Behaviors;
// using MediatR;
// using Microsoft.Extensions.Logging;
// using Moq;
// using System.Diagnostics;
// using FluentAssertions;
//
// namespace BuildingBlocks.Tests.Behaviors;
//
// public class LoggingBehaviorTests
// {
//     private readonly Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>> _loggerMock;
//     private readonly LoggingBehavior<TestRequest, TestResponse> _behavior;
//
//     public LoggingBehaviorTests()
//     {
//         _loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
//         _behavior = new LoggingBehavior<TestRequest, TestResponse>(_loggerMock.Object);
//     }
//
//     [Fact]
//     public async Task Handle_ShouldLogStartAndEndMessages_WhenCalled()
//     {
//         // Arrange
//         var request = new TestRequest();
//         var response = new TestResponse();
//
//         // Act
//         await _behavior.Handle(request, () => Task.FromResult(response), CancellationToken.None);
//
//         // Assert
//         _loggerMock.Verify(
//             x => x.Log(
//                 LogLevel.Information,
//                 It.IsAny<EventId>(),
//                 It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("[START]")),
//                 It.IsAny<Exception>(),
//                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
//             Times.Once);
//
//         _loggerMock.Verify(
//             x => x.Log(
//                 LogLevel.Information,
//                 It.IsAny<EventId>(),
//                 It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("[END]")),
//                 It.IsAny<Exception>(),
//                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
//             Times.Once);
//     }
//
//     [Fact]
//     public async Task Handle_ShouldReturnResponse_WhenCalled()
//     {
//         // Arrange
//         var request = new TestRequest();
//         var response = new TestResponse();
//
//         // Act
//         var result = await _behavior.Handle(request, () => Task.FromResult(response), CancellationToken.None);
//
//         // Assert
//         result.Should().Be(response);
//     }
//
//     [Fact]
//     public async Task Handle_ShouldLogPerformanceWarning_WhenExecutionTakesMoreThanThreeSeconds()
//     {
//         // Arrange
//         var request = new TestRequest();
//         var response = new TestResponse();
//
//         // Act
//         await _behavior.Handle(request, async () =>
//         {
//             await Task.Delay(3500); // More than 3 seconds
//             return response;
//         }, CancellationToken.None);
//
//         // Assert
//         _loggerMock.Verify(
//             x => x.Log(
//                 LogLevel.Warning,
//                 It.IsAny<EventId>(),
//                 It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("[PERFORMANCE]")),
//                 It.IsAny<Exception>(),
//                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
//             Times.Once);
//     }
// }
//
// public record TestRequest : IRequest<TestResponse>;
// public record TestResponse;
