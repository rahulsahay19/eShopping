using MediatR;

namespace Catalog.Application.Queries;

public class DeleteProductByIdQuery : IRequest<bool>
{
    public string Id { get; set; }

    public DeleteProductByIdQuery(string id)
    {
        Id = id;
    }
}