using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICartItemService
{
    //Task<CartItemDto> AddCartItemAsync(CartItemDto cartItem);
    //Task<CartItemDto> UpdateCartItemAsync(CartItemDto cartItem);



    Task<CartItemDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto, string token);
    Task AddOrUpdateCartItemAsync(AddCartItemDto addCartItemDto, string token);
    Task<CartItemDto> DeleteCartItemAsync(int productId, string token);
    Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync(string token);
}