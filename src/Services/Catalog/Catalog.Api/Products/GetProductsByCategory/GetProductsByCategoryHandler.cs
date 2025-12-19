namespace Catalog.Api.Products.GetProductsByCategory;

/// <summary>
///     Represents a query to retrieve products belonging to a specific category.
///     This record captures the filtering criteria, demonstrating how queries can be parameterized.
/// </summary>
/// <param name="Category">The category string used to filter the products.</param>
public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

/// <summary>
///     Represents the result of a <see cref="GetProductsByCategoryQuery" />.
///     Contains a collection of products that matched the requested category.
/// </summary>
/// <param name="Products">The list of filtered products.</param>
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

/// <summary>
///     The handler responsible for executing the <see cref="GetProductsByCategoryQuery" />.
///     It leverages Marten's querying capabilities to filter products by category in the database.
///     This illustrates how business logic (filtering) is isolated within the handler.
/// </summary>
/// <param name="session">The IDocumentSession used for database queries.</param>
/// <param name="logger">The logger for capturing diagnostic information.</param>
internal class GetProductsByCategoryQueryHandler(
    IDocumentSession session,
    ILogger<GetProductsByCategoryQueryHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "GetProductsByCategoryQueryHandler.Handle called with query {@Query}", query);
        var products = await session
            .Query<Product>()
            .Where(p => p.Category
                .Contains(query.Category)
            )
            .ToListAsync(cancellationToken);
        return new GetProductsByCategoryResult(products);
    }
}