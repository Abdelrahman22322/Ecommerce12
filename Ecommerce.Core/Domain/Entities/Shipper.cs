namespace Ecommerce.Core.Domain.Entities;


public class Shipper
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public int AssignedOrdersCount { get; set; }
    // Other properties...

    // Navigation properties
    public ICollection<Order> Orders { get; set; }
}