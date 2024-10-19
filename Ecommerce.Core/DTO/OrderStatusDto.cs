using Ecommerce.Core.Domain.Enums;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class OrderStatusDto
{
    public int Id { get; set; }
    public OrderState Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}