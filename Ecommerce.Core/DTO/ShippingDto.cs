using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class ShippingDto
{
    public int Id { get; set; }
    public int ShipperId { get; set; }
    public ShippingMethod ShippingMethod { get; set; }
    public ShippingStatus ShippingStatus { get; set; }
    public decimal Cost { get; set; }
  //  public string TrackingCode { get; set; }
    public int OrderId { get; set; }
    
    public decimal Price { get; set; }
    public string TrackingNumber { get; set; }

    public string Address { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

}