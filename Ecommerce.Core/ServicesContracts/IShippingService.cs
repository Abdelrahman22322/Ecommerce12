using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IShippingService
{

    Task<ShippingDto> CreateShippingAsync(ShippingDto shippingDto);
    Task<ShippingDto> GetShippingByIdAsync(int shippingId);
    Task<IEnumerable<ShippingDto>> GetShippingsByStatusAsync(ShippingStatus status);
    Task<ShippingDto> GetShippingByTrackingCodeAsync(string trackingCode);
    Task UpdateShippingStatusAsync(int shippingId, ShippingStatus newStatus);
 //   Task AssignShippingMethodAsync(int shippingId, ShippingMethod method, decimal cost);
   // Task AssignShippingToOrderAsync(int orderId, int shipperId, ShippingMethod method, decimal cost, string trackingCode);
    Task<ShippingStatus> GetShippingStatusByTrackingCodeAsync(string trackingCode);
    //void AssignShippingCost(ShippingMethod method, decimal cost);
    //decimal GetShippingPrice(ShippingMethod method); 
    //void DisableShippingMethod(ShippingMethod method);
    //void EnableShippingMethod(ShippingMethod method);
    //public Task<bool> IsValidShippingMethodAsync(ShippingMethod method);
}