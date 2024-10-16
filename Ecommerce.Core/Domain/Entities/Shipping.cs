using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.Domain.Entities;

public class Shipping
{
    public int Id { get; set; }
   
    public int ShipperId { get; set; }
    public Shipper Shipper { get; set; }
    public DateTime ShippingDate { get; set; }
    public string TrackingNumber { get; set; }
    public ICollection<Order> Orders { get; set; }

    public decimal ShippingCost { get; set; }

    // New relationships
    public int ShippingMethodId { get; set; }
    public ShippingMethod ShippingMethod { get; set; }

    public int ShippingStateId { get; set; }
    public ShippingState ShippingState { get; set; }
}
