namespace Catalog.Api.Products.GetProductById;

// public record GetProductByIdRequest(Guid Id);

/// <summary>
///     Represents the response returned by the <see cref="GetProductByIdEndpoint" />.
///     Wraps the product data in a specific response object to maintain a consistent API contract
///     and allow for future expansion without breaking changes.
/// </summary>
/// <param name="Product">The product details returned to the client.</param>
public record GetProductByIdResponse(Product Product);

/// <summary>
///     Defines the HTTP GET endpoint for retrieving a product by its ID.
///     This class implements <see cref="ICarterModule" /> to register the route in the application.
///     It acts as the entry point, translating the HTTP request into a CQRS query.
/// </summary>
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By ID")
            .WithDescription("Get Product By ID");
    }
}