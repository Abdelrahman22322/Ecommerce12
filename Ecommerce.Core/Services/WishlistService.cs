using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WishlistService : IWishlistService
{
    private readonly IGenericRepository<Wishlist> _wishlistRepository;

    public WishlistService(IGenericRepository<Wishlist> wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task CreateWishlistForUserAsync(User user)
    {
        var wishlist = new Wishlist
        {
            UserId = user.Id,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            WishlistItems = new List<WishlistItem>()
        };

        await _wishlistRepository.AddAsync(wishlist);
        await _wishlistRepository.SaveAsync();
    }

    public async Task<Wishlist> GetWishlistByUserIdAsync(int userId)
    {
         return await _wishlistRepository.FindAsync1(w => w.UserId == userId, "WishlistItems");
    }
}