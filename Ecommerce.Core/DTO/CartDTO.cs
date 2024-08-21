namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CartDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemDTO> CartItems { get; set; }
}