using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllTypesQuery : IRequest<IList<TypesResponse>>
{
    
}