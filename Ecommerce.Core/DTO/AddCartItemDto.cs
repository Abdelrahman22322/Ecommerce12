namespace Ecommerce.Core.Domain.RepositoryContracts;

public class AddCartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}