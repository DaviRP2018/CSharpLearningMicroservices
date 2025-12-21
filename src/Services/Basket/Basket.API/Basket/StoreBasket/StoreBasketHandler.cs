namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can't be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

internal class StoreBasketCommandHandler
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken
        cancellationToken)
    {
        var cart = command.Cart;

        // TODO: store basket in DB (use Marten upsert)
        //  update cache

        return new StoreBasketResult(cart.UserName);
    }
}