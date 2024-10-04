using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.Domain.Entities;


public class Shipper
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Address { get; set; }
    

    [DataType(DataType.PhoneNumber)]
    [StringLength(15)]
    [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$")]
    
    public string Phone { get; set; }
    public bool IsDefault { get; set; }
    public int AssignedOrdersCount { get; set; }
    // Other properties...

    // Navigation properties
    public ICollection<Order> Orders { get; set; }
}