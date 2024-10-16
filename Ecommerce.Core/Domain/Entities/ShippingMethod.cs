using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.Entities;

public class ShippingMethod
{
    [Key]
    public int Id { get; set; }
    public string Method { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public ICollection<Shipping> Shippings { get; set; }

    //public static explicit operator ShippingMethod(int v)
    //{
    //    throw new NotImplementedException();
    //}
}