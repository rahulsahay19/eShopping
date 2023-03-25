using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductByNameQuery : IRequest<IList<ProductResponse>>
{
    public string Name { get; set; }

    public GetProductByNameQuery(string name)
    {
        Name = name;
    }
}