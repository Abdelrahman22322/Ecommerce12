using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;

        public CartService(IGenericRepository<Cart> cartRepository, IGenericRepository<CartItem> cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartDTO> AddCartAsync(CartDTO cartDTO)
        {
            // Business logic: Check if the cart already exists for the user
            var existingCart = await GetCartByUserIdAsync(cartDTO.UserId);
            if (existingCart != null)
            {
                throw new InvalidOperationException("A cart already exists for this user.");
            }

            var cart = new Cart
            {
                UserId = cartDTO.UserId,
                CartItems = cartDTO.CartItems.Select(ci => new CartItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            await _cartRepository.AddAsync(cart);
            return cartDTO;
        }

        public async Task<CartItemDTO> AddCartItemAsync(CartItemDTO cartItemDTO)
        {
            // Business logic: Check if the item already exists in the cart
            var cart = await _cartRepository.GetByIdAsync(cartItemDTO.CartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var existingItem = cart.CartItems.FirstOrDefault(item => item.ProductId == cartItemDTO.ProductId);
            if (existingItem != null)
            {
                throw new InvalidOperationException("This item is already in the cart.");
            }

            var cartItem = new CartItem
            {
                CartId = cartItemDTO.CartId,
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity
            };

            await _cartItemRepository.AddAsync(cartItem);
            return cartItemDTO;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(int userId)
        {
            var carts = await _cartRepository.GetAllAsync(c => c.UserId == userId, "CartItems.Product");
            var cart = carts.FirstOrDefault();

            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(ci => new CartItemDTO
                {
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new InvalidOperationException("Cart item not found.");
            }

            await _cartItemRepository.DeleteAsync(cartItem);
        }
    }
}
