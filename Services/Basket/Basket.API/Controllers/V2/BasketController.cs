using System.Net;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers.V2;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<BasketController> _logger;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;

    public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, ICorrelationIdGenerator correlationIdGenerator)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _correlationIdGenerator = correlationIdGenerator;
        _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
    }
    
    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
    {
        //Get existing basket with username
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        if (basket == null)
        {
            return BadRequest();
        }

        var eventMesg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);
        eventMesg.TotalPrice = basket.TotalPrice;
        eventMesg.CorrelationId = _correlationIdGenerator.Get();
        await _publishEndpoint.Publish(eventMesg);
        //remove the basket
        var deleteQuery = new DeleteBasketByUserNameQuery(basketCheckout.UserName);
        await _mediator.Send(deleteQuery);
        return Accepted();
    }
}