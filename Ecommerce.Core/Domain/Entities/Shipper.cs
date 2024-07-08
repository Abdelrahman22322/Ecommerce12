namespace Ecommerce.Core.Domain.Entities;

public class Shipper
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Shipping> ShippingMethods { get; set; }
}