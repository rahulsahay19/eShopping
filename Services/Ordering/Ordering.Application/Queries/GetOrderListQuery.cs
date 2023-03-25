using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries;

public class GetOrderListQuery : IRequest<List<OrderResponse>>
{
    public string UserName { get; set; }

    public GetOrderListQuery(string userName)
    {
        UserName = userName;
    }
}