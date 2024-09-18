using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IWishlistItemService
{
    Task<ItemDto> AddWishlistItemAsync(AddWishlistItemDto addWishlistItemDto);
    Task<ItemDto> DeleteWishlistItemAsync(int productId);
    Task<IEnumerable<ItemDto>> GetAllWishlistItemsAsync();
}