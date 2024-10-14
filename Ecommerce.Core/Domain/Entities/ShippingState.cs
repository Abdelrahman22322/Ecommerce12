using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.Entities;

public class ShippingState
{
    [Key]
    public int Id { get; set; }
    public ShippingStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Shipping> Shippings { get; set; }
}