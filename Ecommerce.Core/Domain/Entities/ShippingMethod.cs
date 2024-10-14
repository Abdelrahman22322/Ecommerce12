using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.Entities;

public class ShippingMethod
{
    [Key]
    public int Id { get; set; }
    public ShippingMethod Method { get; set; }
    public decimal Cost { get; set; }
    public ICollection<Shipping> Shippings { get; set; }
}