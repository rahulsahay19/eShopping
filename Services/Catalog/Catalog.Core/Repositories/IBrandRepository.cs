using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

public interface IBrandRepository
{
    Task<IEnumerable<ProductBrand>> GetAllBrands();
}