namespace Ecommerce.Core.Domain.RepositoryContracts;

public class UpdateCartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}