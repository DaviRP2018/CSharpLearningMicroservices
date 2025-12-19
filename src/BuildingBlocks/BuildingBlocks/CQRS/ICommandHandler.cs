using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
///     Defines a contract for a handler that processes a specific type of <see cref="ICommand" />
///     that returns no value.
///     This interface follows the Single Responsibility Principle by ensuring each command has a
///     dedicated processor.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
public interface ICommandHandler<in TCommand>
    : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>;

/// <summary>
///     Defines a contract for a handler that processes a specific type of
///     <see cref="ICommand{TResponse}" />.
///     This interface follows the Single Responsibility Principle by ensuring each command has a
///     dedicated processor.
///     It integrates with MediatR's <see cref="IRequestHandler{TRequest, TResponse}" />.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
/// <typeparam name="TResponse">The type of the result produced by the command handler.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull;