namespace Catalog.Api.Products.GetProductById;

/// <summary>
///     Represents a query to retrieve a single product from the catalog by its unique identifier.
///     This is defined as a record to ensure immutability and value-based equality, which are
///     ideal characteristics for Data Transfer Objects (DTOs) in a CQRS pattern.
/// </summary>
/// <param name="Id">The unique identifier (GUID) of the product to be retrieved.</param>
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

/// <summary>
///     Represents the result of a <see cref="GetProductByIdQuery" />.
///     Using a record here provides a clean, concise way to wrap the returned product data,
///     ensuring the handler returns a structured object rather than a raw entity.
/// </summary>
/// <param name="Product">The product found in the catalog.</param>
public record GetProductByIdResult(Product Product);

/// <summary>
///     The handler responsible for processing the <see cref="GetProductByIdQuery" />.
///     This class encapsulates the business logic for finding a product by ID,
///     keeping it separate from the API endpoint, and promoting the Single Responsibility Principle.
///     It uses MediatR (via IQueryHandler) to decouple the request from its execution.
/// </summary>
/// <param name="session">Marten IDocumentSession used to interact with the underlying database.</param>
/// <param name="logger">The logger instance used for recording diagnostic information.</param>
internal class GetProductByIdQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null) throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}