namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketRequest(string UserName);

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteBasketEndpoints")
            .Produces<DeleteBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete basket by username")
            .WithDescription("Delete basket by username");
    }
}