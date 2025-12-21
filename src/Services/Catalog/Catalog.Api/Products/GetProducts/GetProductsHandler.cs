namespace Catalog.Api.Products.GetProducts;

/// <summary>
///     Represents a query to retrieve all products from the catalog.
///     This record is used to initiate the "Get All Products" request,
///     following the CQRS pattern where queries are separated from commands.
/// </summary>
public record GetProductsQuery : IQuery<GetProductsResult>;

/// <summary>
///     Represents the result of a <see cref="GetProductsQuery" />.
///     Wraps a collection of <see cref="Product" /> entities to be returned to the caller.
///     Using a dedicated result record allows for adding metadata (like pagination) in the future.
/// </summary>
/// <param name="Products">The list of products retrieved from the catalog.</param>
public record GetProductsResult(
    IEnumerable<Product> Products
);

/// <summary>
///     The handler responsible for processing the <see cref="GetProductsQuery" />.
///     It interacts with the <see cref="IDocumentSession" /> to fetch all products from the store.
///     Handlers encapsulate the 'how' of a request, keeping the API thin and focused.
/// </summary>
/// <param name="session">Marten IDocumentSession for database access.</param>
/// <param name="logger">The logger instance for logging internal operations.</param>
internal class GetProductsQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}