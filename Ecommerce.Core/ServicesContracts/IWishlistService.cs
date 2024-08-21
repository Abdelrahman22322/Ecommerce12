using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IWishlistService
{
    Task<WishlistDTO> AddWishlistAsync(WishlistDTO wishlistDTO);
    Task<WishlistItemDTO> AddWishlistItemAsync(WishlistItemDTO wishlistItemDTO);
    Task<WishlistDTO> GetWishlistByUserIdAsync(int userId);
    Task RemoveWishlistItemAsync(int wishlistItemId);
}