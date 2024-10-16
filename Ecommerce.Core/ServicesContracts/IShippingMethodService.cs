using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IShippingMethodService
{
    Task<ShippingMethodDto> AddShippingMethodAsync(ShippingMethodDto shippingMethodDto);
    Task<ShippingMethodDto> UpdateShippingMethodAsync(ShippingMethodDto shippingMethodDto);
    Task DeleteShippingMethodAsync(int id);
    Task<ShippingMethodDto> GetShippingMethodByIdAsync(int id);
    Task<IEnumerable<ShippingMethodDto>> GetAllShippingMethodsAsync();

    // Business methods
    Task<IEnumerable<ShippingMethodDto>> GetShippingMethodsByCostRangeAsync(decimal minCost, decimal maxCost);
    Task<ShippingMethodDto> GetCheapestShippingMethodAsync();
    Task<decimal> GetShippingPriceAsync(int shippingMethodId);
}