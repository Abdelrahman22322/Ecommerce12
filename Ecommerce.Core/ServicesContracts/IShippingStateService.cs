using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IShippingStateService
{
    Task<ShippingStateDto> AddShippingStateAsync(ShippingStateDto shippingStateDto);
    Task<ShippingStateDto> UpdateShippingStateAsync(ShippingStateDto shippingStateDto);
    Task DeleteShippingStateAsync(int id);
    Task<ShippingStateDto> GetShippingStateByIdAsync(int id);
    Task<IEnumerable<ShippingStateDto>> GetAllShippingStatesAsync();

    // Business methods
    Task<IEnumerable<ShippingStateDto>> GetShippingStatesByStatusAsync(ShippingStatus status);
}