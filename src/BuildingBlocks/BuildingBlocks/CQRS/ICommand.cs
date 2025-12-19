using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
///     Represents a command in the CQRS (Command Query Responsibility Segregation) pattern.
/// </summary>
/// <remarks>
///     A command is used to encapsulate a request that modifies the state of the system.
///     It follows a command-driven approach and defines the intent explicitly.
/// </remarks>
public interface ICommand : ICommand<Unit>;

/// <summary>
///     Defines a CQRS command that encapsulates a request potentially modifying the system state.
/// </summary>
/// <remarks>
///     Commands are used to express intent in a command-driven architecture
///     to ensure consistent segregation of responsibilities between commands and queries.
/// </remarks>
public interface ICommand<out TResponse> : IRequest<TResponse>;