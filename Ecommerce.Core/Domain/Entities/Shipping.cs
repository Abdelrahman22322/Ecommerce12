namespace Ecommerce.Core.Domain.Entities;

public class Shipping
{
    public int Id { get; set; }
    public string Method { get; set; }
    public int ShipperId { get; set; }
    public Shipper Shipper { get; set; }
    public ICollection<Order> Orders { get; set; }
}