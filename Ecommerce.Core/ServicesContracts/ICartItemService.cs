using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICartItemService
{
    //Task<CartItemDto> AddCartItemAsync(CartItemDto cartItem);
    //Task<CartItemDto> UpdateCartItemAsync(CartItemDto cartItem);



    Task<ItemDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
    Task AddOrUpdateCartItemAsync(AddCartItemDto addCartItemDto);
    Task<ItemDto> DeleteCartItemAsync(int productId);
    Task<IEnumerable<ItemDto>> GetAllCartItemsAsync();
}