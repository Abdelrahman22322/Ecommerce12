using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Domain.Entities;

public class Shipping
{
    
    public int Id { get; set; }
    public ShippingMethod Method { get; set; }
    public int ShipperId { get; set; }
    public Shipper Shipper { get; set; }
    public DateTime ShippingDate { get; set; }
    public decimal Price { get; set; }
    public string TrackingNumber { get; set; }
    public ShippingStatus Status { get; set; }
    public ICollection<Order> Orders { get; set; }
}
