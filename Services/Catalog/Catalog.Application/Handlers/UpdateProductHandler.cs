using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = await _productRepository.UpdateProduct(new Product
        {
            Id = request.Id,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Name = request.Name,
            Price = request.Price,
            Summary = request.Summary,
            Brands = request.Brands,
            Types = request.Types
        });
        return productEntity;
    }
}