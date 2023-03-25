using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductByIdQuery: IRequest<ProductResponse>
{
    public string Id { get; set; }

    public GetProductByIdQuery(string id)
    {
        Id = id;
    }
}