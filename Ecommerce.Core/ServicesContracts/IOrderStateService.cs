using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IOrderStateService
{
    Task<OrderStatusDto> AddOrderStateAsync(OrderStatusDto orderStateDto);
    Task<OrderStatusDto> UpdateOrderStateAsync(OrderStatusDto orderStateDto);
    Task DeleteOrderStateAsync(int id);
    Task<OrderStatusDto> GetOrderStateByIdAsync(int id);
    Task<IEnumerable<OrderStatusDto>> GetAllOrderStatesAsync();
    //Task<IEnumerable<OrderStatusDto>> GetOrderStatesByStatusAsync(OrderStatus status);
}