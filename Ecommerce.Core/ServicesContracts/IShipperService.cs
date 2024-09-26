using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IShipperService
{
    Task<ShipperDto> CreateShipperAsync(ShipperDto shipperDto);
    Task<ShipperDto> GetShipperByIdAsync(int shipperId);
    Task<IEnumerable<ShipperDto>> GetAllShippersAsync();
    Task UpdateShipperAsync(ShipperDto shipperDto);
    Task DeleteShipperAsync(int shipperId);
    Task<ShipperDto> AssignOrderToLeastAssignedShipper();
    
}