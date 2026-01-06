using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(
    ISender sender,
    ILogger<BasketCheckoutEventHandler>
        logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message
            .GetType().Name);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message
            .EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message
            .Expiration, message.Cvv, message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            orderId,
            message.CustomerId,
            message.UserName,
            addressDto,
            addressDto,
            paymentDto,
            OrderStatus.Pending,
            [
                new OrderItemDto(orderId, new Guid("f3647b18-0801-4c00-824e-f76dacd11e68"), 2,
                    100000),
                new OrderItemDto(orderId, new Guid("199655c3-0a38-4fa7-9faa-1ff4fb49adaa"), 1,
                    5000)
            ]);

        return new CreateOrderCommand(orderDto);
    }
}