using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class RatingService : IRatingService
{
    private readonly IGenericRepository<Rating> _ratingRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RatingService(IGenericRepository<Rating> ratingRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _ratingRepository = ratingRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserIdFromContext()
    {
        var user = _httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "uid")
                          ?? throw new UnauthorizedAccessException("User is not authorized.");

        return int.Parse(userIdClaim.Value);
    }

    public async Task AddRatingAsync(RatingDto ratingDto)
    {
        var userId = GetUserIdFromContext();
        

        var existingRating = await _ratingRepository.FindAsync(r => r.ProductId == ratingDto.ProductId && r.UserId == userId, null);
        if (existingRating.Any())
        {
            var rating1 = _mapper.Map<Rating>(ratingDto);
            rating1.UserId = userId;
            await _ratingRepository.UpdateAsync(rating1);
            await _ratingRepository.SaveAsync();
        }

        var rating = _mapper.Map<Rating>(ratingDto);
        rating.UserId = userId;
        await _ratingRepository.AddAsync(rating);
        await _ratingRepository.SaveAsync();
    }

    //public async Task UpdateRatingAsync(RatingDto ratingDto)
    //{
    //    var userId = GetUserIdFromContext();
    //  //  ratingDto.UserId = userId;

    //    var rating = _mapper.Map<Rating>(ratingDto);
    //    await _ratingRepository.UpdateAsync(rating);
    //    await _ratingRepository.SaveAsync();
    //}

    public async Task<int> GetTotalRatingsForProductAsync(int productId)
    {
        var ratings = await _ratingRepository.FindAsync(r => r != null && r.ProductId == productId, null);
        return ratings.Count();
    }

    public async Task<Dictionary<int, int>> GetRatingDistributionForProductAsync(int productId)
    {
        var ratings = await _ratingRepository.FindAsync(r => r.ProductId == productId, null);
        return ratings
            .GroupBy(r => r.Value)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
