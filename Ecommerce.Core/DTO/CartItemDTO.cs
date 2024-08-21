namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CartItemDTO
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}