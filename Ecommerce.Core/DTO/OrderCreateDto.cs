using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class OrderCreateDto
{
    public int UserId { get; set; }
   
    public int ShippingMethodId { get; set; }
}