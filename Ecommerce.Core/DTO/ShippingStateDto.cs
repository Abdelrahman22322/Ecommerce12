using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class ShippingStateDto
{
    public int Id { get; set; }
    public ShippingStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}