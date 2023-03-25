using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public class CreateProductCommand: IRequest<ProductResponse>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ImageFile { get; set; }
    public decimal Price { get; set; }
    public ProductBrand Brands { get; set; }
    public ProductType Types { get; set; }
}