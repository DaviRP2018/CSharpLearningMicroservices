using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
///     Defines a contract for a query in the CQRS (Command Query Responsibility Segregation) pattern.
///     A query is a request to retrieve data without modifying the state of the system.
///     It inherits from MediatR's <see cref="IRequest{TResponse}" /> to enable dispatching via a
///     mediator.
/// </summary>
/// <typeparam name="TResponse">The type of the response expected from the query.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull;