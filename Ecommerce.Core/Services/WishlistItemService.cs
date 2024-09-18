using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class WishlistItemService : IWishlistItemService
{
    private readonly IGenericRepository<WishlistItem> _wishlistItemRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IWishlistService _wishlistRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<WishlistItemService> _logger;

    public WishlistItemService(IGenericRepository<WishlistItem> wishlistItemRepository,
        IGenericRepository<Product> productRepository,
        IMapper mapper,
        IWishlistService wishlistRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<WishlistItemService> logger)
    {
        _wishlistItemRepository = wishlistItemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _wishlistRepository = wishlistRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

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

    public async Task<ItemDto> AddWishlistItemAsync(AddWishlistItemDto addWishlistItemDto)
    {
        int userId = GetUserIdFromContext();

        var product = await _productRepository.GetByIdAsync(addWishlistItemDto.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
        if (wishlist == null)
        {
            throw new KeyNotFoundException("Wishlist not found for the user.");
        }

        var existingWishlistItem = wishlist.WishlistItems.FirstOrDefault(w => w.ProductId == addWishlistItemDto.ProductId);

        if (existingWishlistItem != null)
        {
            throw new InvalidOperationException("Product is already in the wishlist.");
        }
        else
        {
            var newWishlistItem = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = addWishlistItemDto.ProductId
            };

            await _wishlistItemRepository.AddAsync(newWishlistItem);
            await _wishlistItemRepository.SaveAsync();

            var wishlistItemDto = _mapper.Map<ItemDto>(newWishlistItem);
            return wishlistItemDto;
        }
    }



    public async Task<ItemDto> DeleteWishlistItemAsync(int productId)
    {
        int userId = GetUserIdFromContext();

        var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
        if (wishlist == null)
        {
            throw new KeyNotFoundException("Wishlist not found for the given user.");
        }

        var wishlistItem = await _wishlistItemRepository.FindAsync1(w => w.WishlistId == wishlist.Id && w.ProductId == productId, null);
        if (wishlistItem == null)
        {
            throw new KeyNotFoundException("WishlistItem not found.");
        }

        await _wishlistItemRepository.DeleteAsync(wishlistItem);
        await _wishlistItemRepository.SaveAsync();

        var product = await _productRepository.GetByIdAsync(wishlistItem.ProductId);
        var mappedWishlistItemDto = _mapper.Map<ItemDto>(wishlistItem);
        mappedWishlistItemDto.ProductName = product.ProductName;

        return mappedWishlistItemDto;
    }

    public async Task<IEnumerable<ItemDto>> GetAllWishlistItemsAsync()
    {
        int userId = GetUserIdFromContext();

        var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
        if (wishlist == null)
        {
            throw new KeyNotFoundException("Wishlist not found for the given user.");
        }

        var wishlistItems = await _wishlistItemRepository.FindAsync(w => w.WishlistId == wishlist.Id, "Product,Product.Brand,Product.Category,Product.ProductImages");

        var wishlistItemDtos = _mapper.Map<List<ItemDto>>(wishlistItems);
        return wishlistItemDtos;
    }
}