namespace Ecommerce.Core.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int PaymentId { get; set; }
    public Payment Payment { get; set; }
   
   
    public int ShippingMethodId { get; set; }
    public Shipping ShippingMethod { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}