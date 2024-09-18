using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class CartItemService : ICartItemService
{
    private readonly IGenericRepository<CartItem> _cartItemRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly ICartService _cartRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CartItemService> _logger;

    public CartItemService(IGenericRepository<CartItem> cartItemRepository,
                           IGenericRepository<Product> productRepository,
                           IMapper mapper,
                           ICartService cartRepository,
                           IHttpContextAccessor httpContextAccessor,
                           ILogger<CartItemService> logger)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _cartRepository = cartRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    // تعديل لاستبدال استخراج الـ userId باستخدام HttpContextAccessor
    private int GetUserIdFromContext()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("User is not authenticated.");
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "uid");
        if (userIdClaim != null)
        {
            _logger.LogInformation($"Extracted UID (UserId) from HttpContext: {userIdClaim.Value}");
            return int.Parse(userIdClaim.Value);
        }

        _logger.LogWarning("UID claim not found in HttpContext.");
        throw new UnauthorizedAccessException("User is not authorized.");
    }

    public async Task<ItemDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
    {
        int userId = GetUserIdFromContext(); // استخدام GetUserIdFromContext بدلاً من تمرير التوكن

        if (updateCartItemDto.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found for the given user.");
        }

        var cartItem = await _cartItemRepository.FindAsync1(c => c.Cart.UserId == cart.UserId && c.ProductId == updateCartItemDto.ProductId, "Product,Product.Brand,Product.Category,Product.ProductImages");
        if (cartItem == null)
        {
            throw new KeyNotFoundException("CartItem not found.");
        }

        cartItem.Quantity = updateCartItemDto.Quantity;

        await _cartItemRepository.UpdateAsync(cartItem);
        await _cartItemRepository.SaveAsync();

        var cartItemDto = _mapper.Map<ItemDto>(cartItem);
        return cartItemDto;
    }

    public async Task AddOrUpdateCartItemAsync(AddCartItemDto addCartItemDto)
    {
        int userId = GetUserIdFromContext(); // استخدام GetUserIdFromContext بدلاً من تمرير التوكن

        if (addCartItemDto.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        var product = await _productRepository.GetByIdAsync(addCartItemDto.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found for the user.");
        }

        var existingCartItem = cart.CartItems.FirstOrDefault(c => c.ProductId == addCartItemDto.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += addCartItemDto.Quantity;

            await _cartItemRepository.UpdateAsync(existingCartItem);
            await _cartItemRepository.SaveAsync();
        }
        else
        {
            var newCartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = addCartItemDto.ProductId,
                Quantity = addCartItemDto.Quantity
            };

            await _cartItemRepository.AddAsync(newCartItem);
            await _cartItemRepository.SaveAsync();
        }
    }

    public async Task<ItemDto> DeleteCartItemAsync(int productId)
    {
        int userId = GetUserIdFromContext(); // استخدام GetUserIdFromContext بدلاً من تمرير التوكن

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found for the given user.");
        }

        var cartItem = await _cartItemRepository.FindAsync1(c => c.CartId == cart.Id && c.ProductId == productId, null);
        if (cartItem == null)
        {
            throw new KeyNotFoundException("CartItem not found.");
        }

        await _cartItemRepository.DeleteAsync(cartItem);
        await _cartItemRepository.SaveAsync();

        var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        var mappedCartItemDto = _mapper.Map<ItemDto>(cartItem);
        mappedCartItemDto.ProductName = product.ProductName;

        return mappedCartItemDto;
    }

    public async Task<IEnumerable<ItemDto>> GetAllCartItemsAsync()
    {
        int userId = GetUserIdFromContext(); // استخدام GetUserIdFromContext بدلاً من تمرير التوكن

        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found for the given user.");
        }

        var cartItems = await _cartItemRepository.FindAsync(c => c.CartId == cart.Id, "Product,Product.Brand,Product.Category,Product.ProductImages");

        var cartItemDtos = _mapper.Map<List<ItemDto>>(cartItems);
        return cartItemDtos;
    }
}
