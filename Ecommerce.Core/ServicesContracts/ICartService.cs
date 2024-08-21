using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICartService
{
    Task<CartDTO> AddCartAsync(CartDTO cartDTO);
    Task<CartItemDTO> AddCartItemAsync(CartItemDTO cartItemDTO);
    Task<CartDTO> GetCartByUserIdAsync(int userId);
    Task RemoveCartItemAsync(int cartItemId);
}