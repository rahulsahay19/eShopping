using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllBrandsQuery : IRequest<IList<BrandResponse>>
{
    
}