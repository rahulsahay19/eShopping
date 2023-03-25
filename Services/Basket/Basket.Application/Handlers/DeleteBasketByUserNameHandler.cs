using Basket.Application.Queries;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class DeleteBasketByUserNameHandler: IRequestHandler<DeleteBasketByUserNameQuery>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketByUserNameHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    public async Task<Unit> Handle(DeleteBasketByUserNameQuery request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(request.UserName);
        return Unit.Value;
    }
}