using MediatR;

namespace Ordering.Application.Commands;

public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }

}