using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.RepositoryContracts;
public class OrderDto
{
    public int Id { get; set; }
    public int OrderStatusId { get; set; }
    public OrderState OrderStatus { get; set; }
    public int PaymentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public string TrackingNumber { get; set; }
    public int ShippingMethodId { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
}