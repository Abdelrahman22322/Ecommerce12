using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using System;
using System.Threading.Tasks;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Services
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart?> _cartRepository;

        public CartService(IGenericRepository<Cart?> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task CreateCartForUserAsync(User user)
        {
            var cart = new Cart
            {
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CartItems =  new List<CartItem>(),
                
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {

            return await _cartRepository.FindAsync1(c => c.UserId == userId, "User,CartItems");
        }
    }
}