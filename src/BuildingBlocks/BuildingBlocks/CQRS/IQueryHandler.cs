using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
///     Defines a contract for a handler that processes a specific type of
///     <see cref="IQuery{TResponse}" />.
///     This interface follows the Single Responsibility Principle by ensuring each query has a
///     dedicated processor.
///     It integrates with MediatR's <see cref="IRequestHandler{TRequest,TResponse}" />.
/// </summary>
/// <typeparam name="TQuery">The type of query to be handled.</typeparam>
/// <typeparam name="TResponse">The type of the result produced by the query handler.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull;