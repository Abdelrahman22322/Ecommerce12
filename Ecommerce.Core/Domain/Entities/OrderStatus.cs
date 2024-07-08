namespace Ecommerce.Core.Domain.Entities;

public class OrderStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public ICollection<Order> Orders { get; set; }
}