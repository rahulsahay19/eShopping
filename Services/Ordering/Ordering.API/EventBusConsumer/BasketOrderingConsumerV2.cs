using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer;

public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketOrderingConsumerV2> _logger;

    public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
    {
        using var scope =  _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
            context.Message.CorrelationId);
        var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
        //TODO: Need to add required address details.
        PopulateAddressDetails(command);
        var result = await _mediator.Send(command);
        _logger.LogInformation($"Basket checkout event completed!!!");
    }

    private static void PopulateAddressDetails(CheckoutOrderCommand command)
    {
        command.FirstName = "Rahul";
        command.LastName = "Sahay";
        command.EmailAddress = "rahulsahay@eshop.net";
        command.AddressLine = "Bangalore";
        command.Country = "India";
        command.State = "KA";
        command.ZipCode = "560001";
        command.PaymentMethod = 1;
        command.CardName = "Visa";
        command.CardNumber = "1234567890123456";
        command.Expiration = "12/25";
        command.CVV = "123";
    }
}