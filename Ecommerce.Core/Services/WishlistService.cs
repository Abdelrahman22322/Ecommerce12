using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IGenericRepository<Wishlist> _wishlistRepository;
        private readonly IGenericRepository<WishlistItem> _wishlistItemRepository;

        public WishlistService(IGenericRepository<Wishlist> wishlistRepository, IGenericRepository<WishlistItem> wishlistItemRepository)
        {
            _wishlistRepository = wishlistRepository;
            _wishlistItemRepository = wishlistItemRepository;
        }

        public async Task<WishlistDTO> AddWishlistAsync(WishlistDTO wishlistDTO)
        {
            // Business logic: Check if the wishlist already exists for the user
            var existingWishlist = await GetWishlistByUserIdAsync(wishlistDTO.UserId);
            if (existingWishlist != null)
            {
                throw new InvalidOperationException("A wishlist already exists for this user.");
            }

            var wishlist = new Wishlist
            {
                UserId = wishlistDTO.UserId,
                WishlistItems = wishlistDTO.WishlistItems.Select(wi => new WishlistItem
                {
                    ProductId = wi.ProductId
                }).ToList()
            };

            await _wishlistRepository.AddAsync(wishlist);
            return wishlistDTO;
        }

        public async Task<WishlistItemDTO> AddWishlistItemAsync(WishlistItemDTO wishlistItemDTO)
        {
            // Business logic: Check if the item already exists in the wishlist
            var wishlist = await _wishlistRepository.GetByIdAsync(wishlistItemDTO.WishlistId);
            if (wishlist == null)
            {
                throw new InvalidOperationException("Wishlist not found.");
            }

            var existingItem = wishlist.WishlistItems.FirstOrDefault(item => item.ProductId == wishlistItemDTO.ProductId);
            if (existingItem != null)
            {
                throw new InvalidOperationException("This item is already in the wishlist.");
            }

            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlistItemDTO.WishlistId,
                ProductId = wishlistItemDTO.ProductId
            };

            await _wishlistItemRepository.AddAsync(wishlistItem);
            return wishlistItemDTO;
        }

        public async Task<WishlistDTO> GetWishlistByUserIdAsync(int userId)
        {
            var wishlists = await _wishlistRepository.GetAllAsync(w => w.UserId == userId, "WishlistItems.Product");
            var wishlist = wishlists.FirstOrDefault();

            if (wishlist == null)
                throw new InvalidOperationException("Wishlist not found.");

            return new WishlistDTO
            {
                Id = wishlist.Id,
                UserId = wishlist.UserId,
                WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                {
                    WishlistId = wi.WishlistId,
                    ProductId = wi.ProductId
                }).ToList()
            };
        }

        public async Task RemoveWishlistItemAsync(int wishlistItemId)
        {
            var wishlistItem = await _wishlistItemRepository.GetByIdAsync(wishlistItemId);
            if (wishlistItem == null)
            {
                throw new InvalidOperationException("Wishlist item not found.");
            }

            await _wishlistItemRepository.DeleteAsync(wishlistItem);
        }
    }
}
