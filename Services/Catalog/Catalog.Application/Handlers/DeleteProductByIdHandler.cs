using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class DeleteProductByIdHandler: IRequestHandler<DeleteProductByIdQuery, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(DeleteProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.DeleteProduct(request.Id);
    }
}