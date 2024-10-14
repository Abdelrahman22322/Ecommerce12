using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IWishlistService
{
    Task CreateWishlistForUserAsync(User user);
    Task<Wishlist> GetWishlistByUserIdAsync(int userId);
}
