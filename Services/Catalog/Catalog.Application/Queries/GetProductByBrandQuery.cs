using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductByBrandQuery :  IRequest<IList<ProductResponse>>
{
    public string Brandname { get; set; }

    public GetProductByBrandQuery(string brandname)
    {
        Brandname = brandname;
    }
}