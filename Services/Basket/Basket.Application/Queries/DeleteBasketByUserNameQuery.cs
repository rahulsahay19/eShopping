using MediatR;

namespace Basket.Application.Queries;

public class DeleteBasketByUserNameQuery : IRequest
{
    public string UserName { get; set; }

    public DeleteBasketByUserNameQuery(string userName)
    {
        UserName = userName;
    }
}