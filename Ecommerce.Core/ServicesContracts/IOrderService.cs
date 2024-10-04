using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto);
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task UpdateOrderAsync(OrderDto orderDto);
    Task DeleteOrderAsync(int orderId);
    Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderState status);
    Task ChangeOrderStatusAsync(int orderId, OrderState newStatus);
}